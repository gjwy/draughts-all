using System;
using System.Collections.Generic;
using System.Windows.Forms;

using checkers;
/* THIS IS REALLY THE CONTROL FLOW, HANDLERS FOR GUI USER INPUT AND CREATES
NEW GAEMS ETC */
namespace checkers_wf
{
    public partial class ViewControler : Form
    {


        //options stuff to be put in options etc
        private string startPlayer = "red";

        // flow state stuff
        // CONSIDER MOVING TO BOARD/MODEL?
        private string GAMETYPE;
        private enum Gamestage { NoClick, OneClick, OngoingCapture_NoClick, OngoingCapture_OneClick, End, None };
        private Gamestage STAGE;
        private Dictionary<string, int> CAPTURED; // score
        private string WINNER;
        //private NetMod NW;
        private Dictionary<string, string> options = new Dictionary<string, string>()
        {
            {"Remote Port", "8888" },
            {"Remote Ip", "000.000.000.000" }
        };


        // turn variables
        private string PLAYER; // the current player
        private Tile SELECTED;
        private List<Move> POTENTIALMOVES;


        private Board board;

        public ViewControler(Board board)
        {



            // prevent flickering double buffering

            this.DoubleBuffered = true;

            this.board = board; //model

            InitializeComponent(); //view
            // drawTiles(); // will be moved out of the initialiser <<==
            renderTiles(board);
            STAGE = Gamestage.None;

        }

       

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // cleanup stuff
            // eg save open gme option
            // finally quit
            this.Close();
        }

        private void optionsMenu_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(options); // <-- options obj
            optionForm.ShowDialog();
        }

        private void vsCompToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGame();
            GAMETYPE = "vsPlayerNW";
            STAGE = Gamestage.NoClick;

        }

        
        private void resetMenu_Click(object sender, EventArgs e)
        {
            resetProcedure();
        }

        private void resetProcedure()
        {
            // reset the model 
            board.clearGameBoard();

            // reset state variables
            STAGE = Gamestage.None;
            SELECTED = null;
            POTENTIALMOVES = null;
            GAMETYPE = null;
            CAPTURED = null;
            WINNER = null;
            PLAYER = null;

            // reset the gui (removes the pieces etc from the tiles)
            clearGuiTiles(board);

            // reset toolbar elements
            resetToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.ToolTipText = "Start a new game versus a player or the ai";

            // reset non guitile display elements
            changeCapturedDisplay(); // when called with no args, clear the pile
            changeDisplayMessage("(gui) board has been reset");
            resetPlayAgain();
        }

        private void loadGameMenu_Click(object sender, EventArgs e)
        {
            // draw the tiles
            // load a game state
            // also contains player state
        }




        // ##################################################################
        /* player vs player game flow set appropriate state
         * could be moved somewhere else, but it requires
         * access to the gui elements as well as model methods */

        // reorganise this between this func and the immediate click handler
        /* player vs player button clicked, setup */
        private void vsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // drawTiles()
            // establish connection (host)
            // etc
            // LOCAL VS PLAYER GAME
            newGame();
            GAMETYPE = "vsPlayer";
            STAGE = Gamestage.NoClick;
        }

        private void newGame()
        {
            
            board.populateGameBoard();       // model method


            
            resetToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.ToolTipText = "A game is currently in progress";

            
            
            
            CAPTURED = new Dictionary<string, int>();
            CAPTURED.Add("white", 0);
            CAPTURED.Add("red", 0);
            WINNER = "";

            
            PLAYER = startPlayer;

            renderAllPieces(board);             // gui method
            changeCapturedDisplay(CAPTURED);
            changeDisplayMessage("Player " + PLAYER + "'s turn");

            // expect next event to be a player click, dont need to check for valid since its first turn and valid is garunteed
        }

        private void playAgainButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Text == "yes")
            {
                resetProcedure();
                newGame();
            }
            else
            {
                resetProcedure();
            }
        }





        private void tileClickedHandler(object sender, Coord coord)
        {
            // interpret clicks based on the Stage variable


            if (STAGE == Gamestage.None)
            {
                // do not process any clicks as a game is not initiated
            }
            else
            {
                List<Coord> tilesWhichHaveChanged = new List<Coord>();
                Tile tileClicked = board.getTile(coord);

                // eg if existing stage was noclicks, then this call to the func represents the first click
                if (STAGE == Gamestage.NoClick)
                {
                    tilesWhichHaveChanged = processFirstClick(tileClicked);
                }

                else if (STAGE == Gamestage.OneClick)
                {
                    tilesWhichHaveChanged = processSecondClick(tileClicked);
                }

                else if (STAGE == Gamestage.OngoingCapture_NoClick)
                {
                    tilesWhichHaveChanged = processOngoingCaptureFirstClick(tileClicked);
                }

                else if (STAGE == Gamestage.OngoingCapture_OneClick)
                {
                    tilesWhichHaveChanged = processOngoingCaptureSecondClick(tileClicked);
                }



                // moves, player changes have already been applied
                // finally update the display with those tilesWhichHaveChanged or been CAPTURED
                // check if the game has ended or not

                finalSteps(tilesWhichHaveChanged); // for the end of this clicks processing

                

                // IF the end of this click processing has resulted in the end of the paylers turn:
                //if (GAMETYPE == "vsPlayerNW") // and currentplayer not thisplayer
                //{
                    //NW.send(board.InternalBoard);
                    //List<Move> moves = NW.getResult(); //ensure this is based on the previous sent // make these calls wait
                    //foreach(Move m in moves)
                    //{
                        //apply, updateGui, wait
                        // tileswhichhavechanged
                    //}
                    // currentPlayer = thisplayer, STATE
                    //finalSteps();//twhc
                //}
            }




        }
        
        private List<Coord> processFirstClick(Tile tileClicked)
        {
            List<Coord> tilesWhichHaveChanged = new List<Coord>();
            // enforce must jump rule if there are pieces with jumps available
            List<Tile> tilesContainingPlayerPiecesWithJumps = board.getTilesContainingPlayerPiecesWithValidMoves(PLAYER, true);

            if (tilesContainingPlayerPiecesWithJumps.Count > 0
                && !tilesContainingPlayerPiecesWithJumps.Contains(tileClicked))
            {
                changeDisplayMessage("You must capture a piece if possible");
            }

            else if (tileClicked.IsOccupied && tileClicked.OccupyingPiece.Player == this.PLAYER)
            {
                // prioritise jumps
                List<Move> availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, true);
                if (availableMoves.Count == 0)
                {
                    // then normal moves
                    availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, false);
                }



                List<Coord> theTiles = setHighlightsForTiles(availableMoves, true);

                tilesWhichHaveChanged.AddRange(theTiles);

                // the model has been changed, and a record is kept of which corresponding tiles must be chaged in the gui
                // update the state
                SELECTED = tileClicked;
                POTENTIALMOVES = availableMoves;
                STAGE = Gamestage.OneClick;
            }

            else
            {
                System.Console.WriteLine("piece clicked is not of cur player");
            }

            return tilesWhichHaveChanged;

        }

        private List<Coord> processSecondClick(Tile tileClicked)
        {
            List<Coord> tilesWhichHaveChanged = new List<Coord>();
            // can assume SELECTED holds a tile (with a valid piece on it)
            // and POTENTIALMOVES contain some


            // checks the second click is on a piece/tile thats highlighted (thus its in potential moves)
            if (tileClicked.IsHighlighted)
            {
                System.Console.WriteLine("is highlighted!");
                Tuple<Piece, bool> result = board.movePiece(SELECTED, tileClicked, POTENTIALMOVES);

                List<Coord> theTiles = setHighlightsForTiles(POTENTIALMOVES, false);
                tilesWhichHaveChanged.AddRange(theTiles);

                tilesWhichHaveChanged.Add(SELECTED.TileCoord);
                tilesWhichHaveChanged.Add(tileClicked.TileCoord);

                // check if a piece was captured
                if (result.Item1 != null)
                {
                    Piece captured = result.Item1;
                    tilesWhichHaveChanged.Add(captured.CurrentPosition); // the tile which was jumped must be updated

                    CAPTURED[captured.Player] += 1;
                    SELECTED = tileClicked;

                    // if piece was captured and move did not result in a king
                    if (!result.Item2)
                    {
                        // then check for further moves
                        List<Move> availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, true);
                        if (availableMoves.Count > 0)
                        {
                            POTENTIALMOVES = availableMoves;
                            STAGE = Gamestage.OngoingCapture_NoClick;
                        }
                    }

                    changeCapturedDisplay(CAPTURED);
                    // if NOT an ongoing capture, then change the player etc
                    if (! (STAGE == Gamestage.OngoingCapture_NoClick) )
                    {
                        PLAYER = (PLAYER == "red") ? "white" : "red";
                        changeDisplayMessage("Player " + PLAYER + "'s turn");
                        STAGE = Gamestage.NoClick;
                    }

                }
                // else a piece wasnt captured (just normal move) so end turn
                else
                {
                    PLAYER = (PLAYER == "red") ? "white" : "red";
                    changeDisplayMessage("Player " + PLAYER + "'s turn");
                    STAGE = Gamestage.NoClick;
                }

            }

            // else the player has clicked on a non highlighted one of their own pieces
            else if ((!tileClicked.IsHighlighted) && tileClicked.IsOccupied && tileClicked.OccupyingPiece.Player == PLAYER)
            {

                // if previous clicked tile is not same as just clicked tile
                if (SELECTED.TileCoord != tileClicked.TileCoord)
                {
                    // then just remove -> update the highlight (by calling this function again)
                    List<Coord> theTiles = setHighlightsForTiles(POTENTIALMOVES, false);
                    //tilesWhichHaveChanged.AddRange(theTiles);
                    STAGE = Gamestage.NoClick;
                    // since a recursive call is made here,
                    // it wont reach the updateGuiTiles normally, 
                    // so must call it manually here

                    // this recursive call is needed, since in this situation when a user clicks off an already highlighted option
                    // onto another option, it simply removes the initial highlight and reapplies the new highlight
                    updateGuiTiles(theTiles);
                    this.tileClickedHandler(null, tileClicked.TileCoord); // problem could be here
                }
                else
                {
                    // just remove the highlight
                    List<Coord> theTiles = setHighlightsForTiles(POTENTIALMOVES, false);
                    tilesWhichHaveChanged.AddRange(theTiles);
                    STAGE = Gamestage.NoClick;
                }
            }

            // else player has clicked on a non-highlighted, non piece of theirs
            else
            {
                List<Coord> theTiles = setHighlightsForTiles(POTENTIALMOVES, false);
                tilesWhichHaveChanged.AddRange(theTiles);
                STAGE = Gamestage.NoClick;
            }

            return tilesWhichHaveChanged;
        }

        private List<Coord> processOngoingCaptureFirstClick(Tile tileClicked)
        {
            List<Coord> tilesWhichHaveChanged = new List<Coord>();
            if (tileClicked == SELECTED)
            {
                List<Coord> tilesToHighlight = new List<Coord>();
                foreach (Move move in POTENTIALMOVES) // remove the highlights
                {
                    tilesToHighlight.Add(move.ToPos); // add those whows highlight value WILL be changed..
                }

                board.setHighlightTag(tilesToHighlight, true);
                tilesWhichHaveChanged.AddRange(tilesToHighlight);
                SELECTED = tileClicked; // redundant?
                STAGE = Gamestage.OngoingCapture_OneClick;
                // PROBLEM LIKELY HERE
                // ONECLICK STATE ISNT ENTIRELY CORRECT, SINCE THERE IS A RESTRICTION THAT THE PIECE DOING SUCCESSIVE CAPTURES IN THE
                // 'SEQUENCE' MUST BE THE SAME PIECE ALL THE WAY THROUGH, EG TILECLICKED MUST BE THE PREV (SELECTED)
                // IF ONECLICK IS SET HERE THIS MEANS SECONDCLICKMADE FUNCTION IS CALLED AND IF AN ERROR IS DETECTED (CLICKING NOT ON THE HIGHLIGHTS)
                // SECONDCLICKMADE FUNCTION WILL HANDLE THE ERROR AND ALLOW THE USER TO SELECT ANY OF HIS PIECES AS A POTENTIAL VALID PIECE A
                // RATHER THAN RESTRICTING IT TO THE PREVIOUS PIECE (THE PNE IN THE SEQUENCE) 
            }
            else
            {
                changeDisplayMessage("Player " + PLAYER + ", continue the capture sequence");
            }
            return tilesWhichHaveChanged;
        }

        private List<Coord> processOngoingCaptureSecondClick(Tile tileClicked)
        {
            // this function almost identical to processSecondClick
            // but ENFORCES THE CONTINUED CAPTURE (WHICH IS IDENTIFIED IN THE FIRST CLICK)
            // input:
            //  SELECTED := the posA tile (which has valid moves (represented by list of posB in POTENTIALMOVES))
            //  tileClicked := the posB tile

            // assert tileClicked is in Potnential moves, then make the move, then check for kinging / further jumps
            // if not in potential moves -> error message, firstclickongoingcapture

            List<Coord> tilesWhichHaveChanged = new List<Coord>();

            if (tileClicked.IsHighlighted) // easier than searching through potentialmoves
            {
                System.Console.WriteLine("is highlighted!");
                Tuple<Piece, bool> result = board.movePiece(SELECTED, tileClicked, POTENTIALMOVES);

                List<Coord> theTiles = setHighlightsForTiles(POTENTIALMOVES, false);
                tilesWhichHaveChanged.AddRange(theTiles);

                tilesWhichHaveChanged.Add(SELECTED.TileCoord);
                tilesWhichHaveChanged.Add(tileClicked.TileCoord);

                if (result.Item1 != null)
                {
                    Piece captured = result.Item1;
                    tilesWhichHaveChanged.Add(captured.CurrentPosition);
                    CAPTURED[captured.Player] += 1;
                    SELECTED = tileClicked;
                    // resulted in a further capture..
                    // and if did not result in a king
                    if (!result.Item2)
                    {
                        List<Move> availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, true);
                        if (availableMoves.Count > 0)
                        {
                            POTENTIALMOVES = availableMoves;
                            STAGE = Gamestage.OngoingCapture_NoClick;
                        }
                    }

                    changeCapturedDisplay(CAPTURED);
                    // if NOT an ongoing capture, then change the player etc
                    if (!(STAGE == Gamestage.OngoingCapture_NoClick))
                    {
                        PLAYER = (PLAYER == "red") ? "white" : "red";
                        changeDisplayMessage("Player " + PLAYER + "'s turn");
                        STAGE = Gamestage.NoClick;
                    }
                }
                // else a piece wasnt captured (just normal move) so end turn
                else
                {
                    PLAYER = (PLAYER == "red") ? "white" : "red";
                    changeDisplayMessage("Player " + PLAYER + "'s turn");
                    STAGE = Gamestage.NoClick;
                }
            }
            // else the player has clicked on a non highlighted tile
            // we must enforce capture
            // 1. player has reclicked on tileA
            // 2. player has clicked on a non highlighted tile (outside of the enforced capture)
            else if (tileClicked.TileCoord == SELECTED.TileCoord)
            {
                // simply toggle highlight for consistency and back to first click
                List<Coord> theTiles = setHighlightsForTiles(POTENTIALMOVES, false);
                tilesWhichHaveChanged.AddRange(theTiles);
                STAGE = Gamestage.OngoingCapture_NoClick;
            }
            else
            // toggle highlight for consistency and enforce the capture
            {
                List<Coord> theTiles = setHighlightsForTiles(POTENTIALMOVES, false);
                tilesWhichHaveChanged.AddRange(theTiles);
                STAGE = Gamestage.OngoingCapture_NoClick;
                changeDisplayMessage("enforce continued capture");
            }
            
            




            return tilesWhichHaveChanged;
        }

        private void finalSteps(List<Coord> tilesWhichHaveChanged)
        {

            updateGuiTiles(tilesWhichHaveChanged);
            changeCapturedDisplay(CAPTURED);
            // at the end of the processing of the input, check that there
            // are tiles containing pieces with moves for the next player's turn
            // not restricted to onlyJumps
            // PLAYER is already set to this next player (during the function)
            // this is the win condition
            List<Tile> tilesContainingPlayerPiecesWithMoves = board.getTilesContainingPlayerPiecesWithValidMoves(PLAYER, false);
            if (tilesContainingPlayerPiecesWithMoves.Count == 0)
            {
                STAGE = Gamestage.End;
                // winner = changePlayer
                WINNER = (PLAYER == "red") ? "white" : "red";
            }

            if (STAGE == Gamestage.End)
            {
                changeDisplayMessage("Player " + WINNER + " wins!");
                playAgain();
            }
            // also move changeplayer into this finalsteps





        }

        private List<Coord> setHighlightsForTiles(List<Move> availableMoves, bool highlightBool)
        {
            List<Coord> tilesToHighlight = new List<Coord>();
            foreach (Move move in availableMoves)
            {
                tilesToHighlight.Add(move.ToPos);
            }
            board.setHighlightTag(tilesToHighlight, highlightBool);
            return tilesToHighlight;
        }


    }
}
