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
        private string GAMETYPE = "";
        private enum Gamestage { NoClick, OneClick, OngoingCapture };
        private Gamestage Stage;
        private Dictionary<string, int> CAPTURED;
        //private int GAMESTAGE; //enumerate eg 0->wating for first user click etc or a set of bools
        private string PLAYER = ""; // the current player
        private Tile SELECTED;
        private List<Move> POTENTIALMOVES;


        private Model board;

        public ViewControler(Model board)
        {
            this.board = board; //model

            InitializeComponent(); //view
            // drawTiles(); // will be moved out of the initialiser <<==
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
            renderPieces(board);
            // offload to logic

            // enable the clicking of the resetMenu_Click item
            resetToolStripMenuItem.Enabled = true;

            // give some success message to the display
            changeDisplayMessage("(gui) board has been populated");

        }

        private void resetMenu_Click(object sender, EventArgs e)
        {
            
            // unpopulate the game board:
            board.clearGameBoard();
            renderPieces(board); // removes all the guipieces
            undrawTiles(board); // undraw the tiles
            resetToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.ToolTipText = null;
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
        private void playerVsplayer()
        {
            startPlayer = "red";
            board.populateGameBoard();       // model method
            renderTiles(board);              // gui method
            renderPieces(board);             // gui method
            
            this.tilePanel.Enabled = true; // allows the tiles to be clicked
            resetToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.ToolTipText = "A game is currently in progress";

            

            CAPTURED = new Dictionary<string, int>();
            CAPTURED.Add("white", 0);
            CAPTURED.Add("red", 0);
            GAMETYPE = "vsPlayer";
            Stage = (int)Gamestage.NoClick;
            PLAYER = startPlayer;

            changeDisplayMessage("it is " + PLAYER + " turn");

            // expect next event to be a player click, dont need to check for valid since its first turn and valid is garunteed
        }







        /* the logic chosen in response to a piece clicked is dependant on the current
         GAMESTATE */
        private void tileClickedHandler(object sender, Coord coord)
        {
            System.Console.WriteLine("STAGE is {0}", Stage);
            //System.Windows.Forms.Panel piece = sender as System.Windows.Forms.Panel;

            Tile tileClicked = board.getTile(coord);
            if (this.Stage == Gamestage.NoClick)
            {
                // enforce must jump rule if there are pieces with jumps available
                // so gives error message and prevents state from proceeding
                // requiring the user to offer a better input
                // it is an expensive check...?
                // (for each player piece, check for a jump)
                // onlyjumps=true
                List<Tile> tilesContainingPlayerPiecesWithJumps = board.getTilesContainingPlayerPiecesWithValidMoves(PLAYER, true);
                // if such a tile exists AND the tileClicked is NOT one of them 
                if (tilesContainingPlayerPiecesWithJumps.Count > 0
                    && !tilesContainingPlayerPiecesWithJumps.Contains(tileClicked))
                {
                    changeDisplayMessage("you must capture a piece if possible");
                }
                else
                {
                    // if its the initial click make sure its of a PIECE AND that pieceis of PLAYER
                    if (tileClicked.IsOccupied && tileClicked.OccupyingPiece.Player == this.PLAYER)
                    {
                        //System.Console.WriteLine("piece clicked is of cur player");
                        // enforce the must jump priority
                        List<Move> availableMoves = board.getValidAvailableMoves(coord, true);
                        if (availableMoves.Count == 0)
                        {

                            //System.Console.WriteLine("three are NOT available jumps to be made by the current player");
                            // so remove the restriction on only jumps
                            availableMoves = board.getValidAvailableMoves(coord, false);
                        }

                        // at this stage there should atleast be some moves available in availableMoves
                        // (garunteed by check for moves at end of last turn)
                        board.setHighlightTag(availableMoves, true); // the topos is marked for gui highlighting
                        // now the model has been updated correctly (highlights at this stage)
                        // so update the state
                        SELECTED = tileClicked;
                        POTENTIALMOVES = availableMoves;
                        Stage = Gamestage.OneClick;
                    }
                    else
                    {
                        System.Console.WriteLine("piece clicked is not of cur player");
                    }
                }

            }

            else if (this.Stage == Gamestage.OneClick)
            {
                // can assume SELECTED holds a tile (with a valid piece on it)
                // and POTENTIALMOVES contain some

                // checks the second click is on a piece/tile thats highlighted
                if (tileClicked.IsHighlighted)
                {
                    System.Console.WriteLine("is highlighted!");
                    Tuple<Piece, bool> result = board.movePiece(SELECTED, tileClicked, POTENTIALMOVES);
                    System.Console.WriteLine("piece moved");
                    board.setHighlightTag(POTENTIALMOVES, false);
                    // check if a piece was captured
                    if (result.Item1 != null)
                    {
                        Piece captured = result.Item1;
                        CAPTURED[captured.Player] += 1;
                        SELECTED = tileClicked;
                        // check if the move resulted in a kinging
                        if (result.Item2)
                        {
                            // then the turn has ended so change player etc
                            PLAYER = (PLAYER == "red") ? "white" : "red";
                            changeDisplayMessage("it is " + PLAYER + " turn");
                            Stage = Gamestage.NoClick;
                        }
                        // else not kinged so check for further moves to jump
                        else
                        {
                            List<Move> availableMoves = board.getValidAvailableMoves(coord, true);
                            if (availableMoves.Count > 0)
                            {
                                POTENTIALMOVES = availableMoves;
                                Stage = Gamestage.OngoingCapture;
                            }
                            else
                            {
                                PLAYER = (PLAYER == "red") ? "white" : "red";
                                changeDisplayMessage("it is " + PLAYER + " turn");
                                Stage = Gamestage.NoClick;
                            }
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("changing turn");
                        PLAYER = (PLAYER == "red") ? "white" : "red";
                        changeDisplayMessage("it is " + PLAYER + " turn");
                        Stage = Gamestage.NoClick;
                    }
                }
                // else the player has clicked on a non highlighted one of their pieces
                else if ((!tileClicked.IsHighlighted) && tileClicked.IsOccupied && tileClicked.OccupyingPiece.Player == PLAYER)
                {
                    System.Console.WriteLine("it thinkks tile is not highlighted AND its occupied by the cur player piece?");
                    ////////////
                    // ALSO THE NW TILE IS CONSIDERED INVALID
                    // SO SOME ISSUE HERE
                    System.Console.WriteLine("{0} {1} {2}", tileClicked.IsHighlighted, tileClicked.IsOccupied, tileClicked.OccupyingPiece.Player);
                    System.Console.WriteLine("potential moves are:");
                    foreach( Move move in POTENTIALMOVES)
                    {
                        System.Console.WriteLine(move.ToPos.repr());
                    }
                    System.Console.WriteLine("and the tile clicked was {0}", tileClicked.TileCoord.repr());





                    // this if-else block to make the highlight response nicer

                    // if previous clicked tile is not same as just clicked tile
                    if (SELECTED.TileCoord != tileClicked.TileCoord)
                    {
                        // then just update the highlight
                        board.setHighlightTag(POTENTIALMOVES, false);
                        Stage = Gamestage.NoClick;
                        this.tileClickedHandler(null, coord); // problem could be here
                    }
                    else
                    {
                        // just remove the highlight
                        board.setHighlightTag(POTENTIALMOVES, false);
                        Stage = Gamestage.NoClick;
                    }
                }

                // sles player has clicked on some invalid tile so remove highlight
                // CONSIDER REMOVING PREVIOUS ELSE ?
                else
                {
                    System.Console.WriteLine("some invalid tile?");
                    board.setHighlightTag(POTENTIALMOVES, false);
                    Stage = Gamestage.NoClick;
                }


            }

            else if (Stage == Gamestage.OngoingCapture)
            {
                if (tileClicked == SELECTED)
                {
                    board.setHighlightTag(POTENTIALMOVES, true);
                    SELECTED = tileClicked; // redundant?
                    Stage = Gamestage.OneClick;
                }
                else
                {
                    changeDisplayMessage(PLAYER + ", continue the capture sequence");
                }
            }


            // at very end of this function (changes have been made to model)
            // so update the gui with the changes
            renderPieces(board);

        }

       

    }
}
