using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using checkers;
/* THIS IS REALLY THE CONTROL FLOW, HANDLERS FOR GUI USER INPUT AND CREATES
NEW GAEMS ETC */
namespace checkers_wf
{
    public partial class ViewControler : Form
    {
        

        //options stuff to be put in options etc
        private string startPlayer = "";

        // flow state stuff
        // CONSIDER MOVING TO BOARD/MODEL?
        private string GAMETYPE;
        private enum Gamestage { NoClick, OneClick, OngoingCapture, End };
        private Gamestage STAGE;
        private Dictionary<string, int> CAPTURED; // score
        private string WINNER;

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
            OptionForm optionForm = new OptionForm();
            optionForm.ShowDialog();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void vsCompToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
            // ASSERT BOARDSTATE==PREGAME


            //drawTest();
            renderTiles(board); // to implement in View-guiTiles.cs
            // requires the board object to get the colors decided for the tiles in the model
            // this is done in the control::
            board.populateGameBoard();
            // now the populated tiles (containing a piece) 
            // have been marked as guimustbeupdated

            // wait for a small delay then update the gui accordingly
            //System.Threading.Thread.Sleep(1000);
            renderAllPieces(board);
            // offload to logic

            // enable the clicking of the resetMenu_Click item
            resetToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.ToolTipText = "A game is currently in progress";
            // give some success message to the display
            //changeDisplayMessage("(gui) board has been populated");

        }

        private void resetMenu_Click(object sender, EventArgs e)
        {
            
            // unpopulate the game board:
            board.clearGameBoard();
            renderAllPieces(board); // removes all the guipieces
            undrawTiles(board); // undraw the tiles
            resetToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.ToolTipText = null;


            //changeScoreMessage(); // when called without args resets
            changeCapturedDisplay();
            changeDisplayMessage("(gui) board has been reset");
        }

        private void loadGameMenu_Click(object sender, EventArgs e)
        {
            // draw the tiles
            // load a game state
            // also contains player state
        }

        /* player vs player button clicked, setup */
        private void vsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // drawTiles()
            // establish connection (host)
            // etc
            // LOCAL VS PLAYER GAME
            playerVsplayer();
        }




        // ##################################################################
        /* player vs player game flow set appropriate state
         * could be moved somewhere else, but it requires
         * access to the gui elements as well as model methods */

            // reorganise this between this func and the immediate click handler
        private void playerVsplayer()
        {
            startPlayer = "red";
            board.populateGameBoard();       // model method


            
            resetToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.ToolTipText = "A game is currently in progress";

            
            
            GAMETYPE = "vsPlayer";
            CAPTURED = new Dictionary<string, int>();
            CAPTURED.Add("white", 0);
            CAPTURED.Add("red", 0);
            WINNER = "";

            STAGE = Gamestage.NoClick;
            PLAYER = startPlayer;

            //renderTiles(board);              // gui method
            renderAllPieces(board);             // gui method
            this.tilePanel.Enabled = true; // allows the tiles to be clicked (must be after gui renders)
            //changeScoreMessage(CAPTURED);
            changeCapturedDisplay(CAPTURED);
            changeDisplayMessage("Player " + PLAYER + "'s turn");

            // expect next event to be a player click, dont need to check for valid since its first turn and valid is garunteed
        }








        private void tileClickedHandler(object sender, Coord coord)
        {
            // interpret clicks based on the Stage variable

            List<Coord> tilesWhichHaveChanged = new List<Coord>();

            Tile tileClicked = board.getTile(coord);

            // eg if stage was noclicks, then this call to the func represents the first click
            if (this.STAGE == Gamestage.NoClick)
            {
                tilesWhichHaveChanged = firstClickMade(tileClicked);
            }
            else if (this.STAGE == Gamestage.OneClick)
            {
                tilesWhichHaveChanged = secondClickMade(tileClicked);
            }
            else if (STAGE == Gamestage.OngoingCapture)
            {
                secondClickMadeOfContinuedCapture();
            }

            // finally do stuff eg send the tiles which have changed to be refreshed by the gui display
            // tilesWhichHaveChanged goes in here
            finaly();


        }

        // done
        private List<Coord> firstClickMade(Tile tileClicked)
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
        


        private List<Coord> secondClickMade(Tile tileClicked)
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

                    // if move did not result in a king
                    if (!result.Item2)
                    {
                        // then check for further moves
                        List<Move> availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, true);
                        if (availableMoves.Count > 0)
                        {
                            POTENTIALMOVES = availableMoves;
                            STAGE = Gamestage.OngoingCapture;
                        }
                    }

                    changeCapturedDisplay(CAPTURED);
                    // if NOT an ongoing capture, then change the player etc
                    if (! (STAGE == Gamestage.OngoingCapture) )
                    {
                        PLAYER = (PLAYER == "red") ? "white" : "red";
                        changeDisplayMessage("Player " + PLAYER + "'s turn");
                        STAGE = Gamestage.NoClick;
                    }

                }
                // else a piece wasnt captured
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
                    tilesWhichHaveChanged.AddRange(theTiles);
                    STAGE = Gamestage.NoClick;
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

        private void secondClickMadeOfContinuedCapture()
        {
            if (tileClicked == SELECTED)
            {
                List<Coord> tilesToHighlight = new List<Coord>();
                foreach (Move move in POTENTIALMOVES) // remove the highlights
                {
                    tilesToHighlight.Add(move.ToPos); // add those whows highlight value WILL be changed..
                }

                board.setHighlightTag(tilesToHighlight, false);
                tilesToChange.AddRange(tilesToHighlight);
                SELECTED = tileClicked; // redundant?
                STAGE = Gamestage.OneClick;
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
        }

        private void finaly()
        {
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

            // check that game has ended
            if (STAGE == Gamestage.End)
            {
                // display winner / scores
                // no valid moves for ~PLAYER, 
                changeDisplayMessage("Player " + WINNER + " wins!");

                // TODO: cleanup game and prog to be ready for next game

            }

            // at very end of this function (changes have been made to model)
            // so update the gui with the changes
            //renderAllPieces(board); // gui method taking model arg
            //changeScoreMessage(CAPTURED); // gui method taking control arg

            updateGuiTiles(tilesToChange);
            changeCapturedDisplay(CAPTURED);
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
