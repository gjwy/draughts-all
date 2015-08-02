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

namespace checkers_wf
{
    public partial class View : Form
    {

        private Model board;

        public View(Model board)
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
            renderTiles(board);              // gui method

            board.populateGameBoard();       // model method

            renderPieces(board);             // gui method
            resetToolStripMenuItem.Enabled = true;
            changeDisplayMessage("ready for local player vs player game");

            // start game
            playerVsplayer();

        }

        /* player vs player game flow
         * could be moved somewhere else, but it requires
         * access to the gui elements as well as model methods */
        private void playerVsplayer()
        {
            string current = "red";
            List<Tile> listOftiles = board.getTilesContainingPlayerPiecesWithValidMoves(current);
            bool isMoves = listOftiles.Count != 0;
            while (isMoves) //TODO change to a 'break' more efficient version
            {
                changeDisplayMessage("waiting on {red} to select a move(a)"); // current
                // 1. get a move from the player ... (enable tiles to now be clicked) (wait till one is clicked)
                
                isMoves = false;
            }
        }



        private void optionsMenu_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm();
            optionForm.ShowDialog();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tileClicked(object sender, EventArgs e)
        {
            System.Console.WriteLine("tile has been clicked");
            System.Windows.Forms.Panel tile = sender as System.Windows.Forms.Panel;
            System.Console.WriteLine(tile.BackColor);
        }

        private void pieceClicked(object sender, Coord coord)
        {
            System.Console.WriteLine("piece has been clicked");
            System.Windows.Forms.Panel piece = sender as System.Windows.Forms.Panel;
            System.Console.WriteLine(piece.BackColor);
            System.Console.WriteLine("coord is {0}", coord.repr());
        }

       

    }
}
