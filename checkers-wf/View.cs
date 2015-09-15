using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

using checkers;
using network;
using checkers_wf.Game;


namespace checkers_wf
{
    public partial class View : Form
    {

        // internal model
        private Board board;
        // contains game status data
        public Data data;




        

        public View(Board board)
        {
            this.DoubleBuffered = true;
            this.data = new Data();
            this.board = board; //model
            InitializeComponent(); //view
            renderTiles(board);
        }


        /****************** Menu Item Handlers *********************/

        private void options_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm(data.Options); // <-- options obj
            optionForm.ShowDialog();
        }

        private void about_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void quit_Click(object sender, EventArgs e)
        {
            // cleanup stuff
            // eg save open gme option
            // finally quit
            this.Close();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            resetProcedure();
        }

        private void loadGame_Click(object sender, EventArgs e)
        {
            // TODO
            // draw the tiles
            // load a game state
            // also contains player state
        }

        private void saveGame_Click(object sender, EventArgs e)
        {
            // TODO:
        }

        // versus computer Player
        private void vsComputer_Click(object sender, EventArgs e)
        {
            // TODO
        }

        // versus local Player
        private void vsLocalPlayer_Click(object sender, EventArgs e)
        {
            // drawTiles()
            // establish connection (host)
            // etc
            // LOCAL VS PLAYER GAME
            data.Gametype = "vsPlayer";
            Game.Game game = new Game.Game(this);
        }

        // versus multiplayer (host)
        private void hostMultiplayer_Click(object sender, EventArgs e)
        {
            
            data.Gametype = "host";
            Game.Game game = new Game.Game(this);

           

        }

        // versus multiplayer (join)
        private void joinMultiplayer_Click(object sender, EventArgs e)
        {
            //d.Gametype = "join";
            //nw = new NetworkInterface(d.Options, "blue");
            //nwThread = new Thread(nw.joinGame);
            //nwThread.Start();
            //d.Stage = Data.Gamestage.NoClick;
            //newGame();
         
            
            
        }


        /****************** Situational Handlers *********************/

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


        /****************** Code used by handlers *********************/

        public void newGame()
        {

            board.populateGameBoard();       // model method



            resetToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.ToolTipText = "A game is currently in progress";




            data.Captured = new Dictionary<string, int>();
            data.Captured.Add("white", 0);
            data.Captured.Add("red", 0);
            data.Winner = "";


            

            renderAllPieces(board);             // gui method
            changeCapturedDisplay(data.Captured);
            changeDisplayMessage("Player " + data.Current_player + "'s turn");

            // expect next event to be a player click, dont need to check for valid since its first turn and valid is garunteed
        }

        public void resetProcedure()
        {
            // reset the model 
            board.clearGameBoard();

            // reset state variables
            data.Stage = Data.Gamestage.None;
            data.Selected = null;
            data.Potentialmoves = null;
            data.Gametype = null;
            data.Captured = null;
            data.Winner = null;
            data.Current_player = null;

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


        /****************** Game Handler and code *********************/

        private void tileClickedHandler(object sender, Coord coord)
        {
            // interpret clicks based on the Stage variable


            if (data.Stage == Data.Gamestage.None)
            {
                // do not process any clicks as a game is not initiated
            }
            else
            {
                List<Coord> tilesWhichHaveChanged = new List<Coord>();
                Tile tileClicked = board.getTile(coord);

                // eg if existing stage was noclicks, then this call to the func represents the first click
                if (data.Stage == Data.Gamestage.NoClick)
                {
                    tilesWhichHaveChanged = processFirstClick(tileClicked);
                }

                else if (data.Stage == Data.Gamestage.OneClick)
                {
                    tilesWhichHaveChanged = processSecondClick(tileClicked);
                }

                else if (data.Stage == Data.Gamestage.OngoingCapture_NoClick)
                {
                    tilesWhichHaveChanged = processOngoingCaptureFirstClick(tileClicked);
                }

                else if (data.Stage == Data.Gamestage.OngoingCapture_OneClick)
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
            List<Tile> tilesContainingPlayerPiecesWithJumps = board.getTilesContainingPlayerPiecesWithValidMoves(data.Current_player, true);

            if (tilesContainingPlayerPiecesWithJumps.Count > 0
                && !tilesContainingPlayerPiecesWithJumps.Contains(tileClicked))
            {
                changeDisplayMessage("You must capture a piece if possible");
            }

            else if (tileClicked.IsOccupied && tileClicked.OccupyingPiece.Player == data.Current_player)
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
                data.Selected = tileClicked;
                data.Potentialmoves = availableMoves;
                data.Stage = Data.Gamestage.OneClick;
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
                Tuple<Piece, bool> result = board.movePiece(data.Selected, tileClicked, data.Potentialmoves);

                List<Coord> theTiles = setHighlightsForTiles(data.Potentialmoves, false);
                tilesWhichHaveChanged.AddRange(theTiles);

                tilesWhichHaveChanged.Add(data.Selected.TileCoord);
                tilesWhichHaveChanged.Add(tileClicked.TileCoord);

                // check if a piece was captured
                if (result.Item1 != null)
                {
                    Piece captured = result.Item1;
                    tilesWhichHaveChanged.Add(captured.CurrentPosition); // the tile which was jumped must be updated

                    data.Captured[captured.Player] += 1;
                    data.Selected = tileClicked;

                    // if piece was captured and move did not result in a king
                    if (!result.Item2)
                    {
                        // then check for further moves
                        List<Move> availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, true);
                        if (availableMoves.Count > 0)
                        {
                            data.Potentialmoves = availableMoves;
                            data.Stage = Data.Gamestage.OngoingCapture_NoClick;
                        }
                    }

                    changeCapturedDisplay(data.Captured);
                    // if NOT an ongoing capture, then change the player etc
                    if (! (data.Stage == Data.Gamestage.OngoingCapture_NoClick) )
                    {
                        data.Current_player = (data.Current_player == "red") ? "white" : "red";
                        changeDisplayMessage("Player " + data.Current_player + "'s turn");
                        data.Stage = Data.Gamestage.NoClick;
                    }

                }
                // else a piece wasnt captured (just normal move) so end turn
                else
                {
                    data.Current_player = (data.Current_player == "red") ? "white" : "red";
                    changeDisplayMessage("Player " + data.Current_player + "'s turn");
                    data.Stage = Data.Gamestage.NoClick;
                }

            }

            // else the player has clicked on a non highlighted one of their own pieces
            else if ((!tileClicked.IsHighlighted) && tileClicked.IsOccupied && tileClicked.OccupyingPiece.Player == data.Current_player)
            {

                // if previous clicked tile is not same as just clicked tile
                if (data.Selected.TileCoord != tileClicked.TileCoord)
                {
                    // then just remove -> update the highlight (by calling this function again)
                    List<Coord> theTiles = setHighlightsForTiles(data.Potentialmoves, false);
                    //tilesWhichHaveChanged.AddRange(theTiles);
                    data.Stage = Data.Gamestage.NoClick;
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
                    List<Coord> theTiles = setHighlightsForTiles(data.Potentialmoves, false);
                    tilesWhichHaveChanged.AddRange(theTiles);
                    data.Stage = Data.Gamestage.NoClick;
                }
            }

            // else player has clicked on a non-highlighted, non piece of theirs
            else
            {
                List<Coord> theTiles = setHighlightsForTiles(data.Potentialmoves, false);
                tilesWhichHaveChanged.AddRange(theTiles);
                data.Stage = Data.Gamestage.NoClick;
            }

            return tilesWhichHaveChanged;
        }

        private List<Coord> processOngoingCaptureFirstClick(Tile tileClicked)
        {
            List<Coord> tilesWhichHaveChanged = new List<Coord>();
            if (tileClicked == data.Selected)
            {
                List<Coord> tilesToHighlight = new List<Coord>();
                foreach (Move move in data.Potentialmoves) // remove the highlights
                {
                    tilesToHighlight.Add(move.ToPos); // add those whows highlight value WILL be changed..
                }

                board.setHighlightTag(tilesToHighlight, true);
                tilesWhichHaveChanged.AddRange(tilesToHighlight);
                data.Selected = tileClicked; // redundant?
                data.Stage = Data.Gamestage.OngoingCapture_OneClick;
                // PROBLEM LIKELY HERE
                // ONECLICK STATE ISNT ENTIRELY CORRECT, SINCE THERE IS A RESTRICTION THAT THE PIECE DOING SUCCESSIVE CAPTURES IN THE
                // 'SEQUENCE' MUST BE THE SAME PIECE ALL THE WAY THROUGH, EG TILECLICKED MUST BE THE PREV (SELECTED)
                // IF ONECLICK IS SET HERE THIS MEANS SECONDCLICKMADE FUNCTION IS CALLED AND IF AN ERROR IS DETECTED (CLICKING NOT ON THE HIGHLIGHTS)
                // SECONDCLICKMADE FUNCTION WILL HANDLE THE ERROR AND ALLOW THE USER TO SELECT ANY OF HIS PIECES AS A POTENTIAL VALID PIECE A
                // RATHER THAN RESTRICTING IT TO THE PREVIOUS PIECE (THE PNE IN THE SEQUENCE) 
            }
            else
            {
                changeDisplayMessage("Player " + data.Current_player + ", continue the capture sequence");
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
                Tuple<Piece, bool> result = board.movePiece(data.Selected, tileClicked, data.Potentialmoves);

                List<Coord> theTiles = setHighlightsForTiles(data.Potentialmoves, false);
                tilesWhichHaveChanged.AddRange(theTiles);

                tilesWhichHaveChanged.Add(data.Selected.TileCoord);
                tilesWhichHaveChanged.Add(tileClicked.TileCoord);

                if (result.Item1 != null)
                {
                    Piece captured = result.Item1;
                    tilesWhichHaveChanged.Add(captured.CurrentPosition);
                    data.Captured[captured.Player] += 1;
                    data.Selected = tileClicked;
                    // resulted in a further capture..
                    // and if did not result in a king
                    if (!result.Item2)
                    {
                        List<Move> availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, true);
                        if (availableMoves.Count > 0)
                        {
                            data.Potentialmoves = availableMoves;
                            data.Stage = Data.Gamestage.OngoingCapture_NoClick;
                        }
                    }

                    changeCapturedDisplay(data.Captured);
                    // if NOT an ongoing capture, then change the player etc
                    if (!(data.Stage == Data.Gamestage.OngoingCapture_NoClick))
                    {
                        data.Current_player = (data.Current_player == "red") ? "white" : "red";
                        changeDisplayMessage("Player " + data.Current_player + "'s turn");
                        data.Stage = Data.Gamestage.NoClick;
                    }
                }
                // else a piece wasnt captured (just normal move) so end turn
                else
                {
                    data.Current_player = (data.Current_player == "red") ? "white" : "red";
                    changeDisplayMessage("Player " + data.Current_player + "'s turn");
                    data.Stage = Data.Gamestage.NoClick;
                }
            }
            // else the player has clicked on a non highlighted tile
            // we must enforce capture
            // 1. player has reclicked on tileA
            // 2. player has clicked on a non highlighted tile (outside of the enforced capture)
            else if (tileClicked.TileCoord == data.Selected.TileCoord)
            {
                // simply toggle highlight for consistency and back to first click
                List<Coord> theTiles = setHighlightsForTiles(data.Potentialmoves, false);
                tilesWhichHaveChanged.AddRange(theTiles);
                data.Stage = Data.Gamestage.OngoingCapture_NoClick;
            }
            else
            // toggle highlight for consistency and enforce the capture
            {
                List<Coord> theTiles = setHighlightsForTiles(data.Potentialmoves, false);
                tilesWhichHaveChanged.AddRange(theTiles);
                data.Stage = Data.Gamestage.OngoingCapture_NoClick;
                changeDisplayMessage("enforce continued capture");
            }
            
            




            return tilesWhichHaveChanged;
        }

        /* Calls the updateGui method with only the board tiles which have changed,
           updates the captured/score display : TODO pile of tiles ...
           checks if a move exists for the next player and if not declares the winner
           provides a palyAgain prompt if game ends */
        private void finalSteps(List<Coord> tilesWhichHaveChanged)
        {

            updateGuiTiles(tilesWhichHaveChanged);
            changeCapturedDisplay(data.Captured);
            // at the end of the processing of the input, check that there
            // are tiles containing pieces with moves for the next player's turn
            // not restricted to onlyJumps
            // PLAYER is already set to this next player (during the function)
            // this is the win condition
            List<Tile> tilesContainingPlayerPiecesWithMoves = board.getTilesContainingPlayerPiecesWithValidMoves(data.Current_player, false);
            if (tilesContainingPlayerPiecesWithMoves.Count == 0)
            {
                data.Stage = Data.Gamestage.End;
                // winner = changePlayer
                data.Winner = (data.Current_player == "red") ? "white" : "red";
            }

            if (data.Stage == Data.Gamestage.End)
            {
                changeDisplayMessage("Player " + data.Winner + " wins!");
                playAgain();
            }
            // also move changeplayer into this finalsteps

            // if game is multiplayer, at the end of each turn
            // change state to .turncompleted
            // also, (recentmost move / turn outcome is saved ready to be sent)
            if (data.Gametype == "host" || data.Gametype == "join")
            {
                // data.multiplayerstate = turncompleted
            }





        }

        /* Helper function, given a list of moves, sets the toTile to the given highlight value
           then returns all of those tiles which have been set, ready to be sent to the guiUpdate 
           procedure ~ could be moved out of this file really*/
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
