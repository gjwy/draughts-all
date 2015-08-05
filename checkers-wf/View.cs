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
        private string startPlayer = "red";

        // flow state stuff
        private string GAMETYPE = "";
        private enum Gamestage { NoClick, OneClick };
        private Gamestage Stage;
        //private int GAMESTAGE; //enumerate eg 0->wating for first user click etc or a set of bools
        private string PLAYER = ""; // the current player


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
            board.populateGameBoard();       // model method
            renderTiles(board);              // gui method
            renderPieces(board);             // gui method
            
            this.tilePanel.Enabled = true; // allows the tiles to be clicked
            resetToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.ToolTipText = "A game is currently in progress";
            changeDisplayMessage("ready for local player vs player game");

            this.GAMETYPE = "vsPlayer";
            this.Stage = (int)Gamestage.NoClick;
            this.PLAYER = startPlayer;

            // expect next event to be a player click, dont need to check for valid since its first turn and valid is garunteed
        }





        private void tileClicked(object sender, EventArgs e)
        {
            System.Console.WriteLine("tile has been clicked");
            System.Windows.Forms.Panel tile = sender as System.Windows.Forms.Panel;
            System.Console.WriteLine(tile.BackColor);
        }

        /* the logic chosen in response to a piece clicked is dependant on the current
         GAMESTATE */
        private void pieceClicked(object sender, Coord coord)
        {
            System.Console.WriteLine("piece has been clicked");
            System.Windows.Forms.Panel piece = sender as System.Windows.Forms.Panel;

            Tile tileClicked = board.getTile(coord);
            if (this.Stage == Gamestage.NoClick)
            {
                // if its the initial click make sure its of PLAYER
                if (tileClicked.OccupyingPiece.Player == this.PLAYER)
                {
                    System.Console.WriteLine("piece clicked is of cur player");
                    // enforce the must jump priority
                    List<Move> availableMoves = board.getValidAvailableMoves(coord, true);
                    if (availableMoves.Count > 0)
                    {
                        System.Console.WriteLine("three are available jumps to be made by the current player");
                        // so must enforce that click2 makes a move that is in this list
                    }
                    else
                    {
                        System.Console.WriteLine("three are NOT available jumps to be made by the current player");
                        // so remove the restriction on only jumps
                        availableMoves = board.getValidAvailableMoves(coord, false);
                    }

                    // at this stage there should atleast be some moves available in availableMoves
                    // (garunteed by check for moves at end of last turn)
                    board.setHighlightTag(availableMoves, true); // the topos is marked for gui highlighting

                }
                else
                {
                    System.Console.WriteLine("piece clicked is not of cur player");
                }

            }

            else if (this.Stage == Gamestage.OneClick)
            {
                // TODO
            }


            // at very end of this function (changes have been made to model)
            // so update the gui with the changes

        }

       

    }
}
