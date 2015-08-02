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
    public partial class Form1 : Form
    {

        private Board board;

        public Form1()
        {
            board = new Board(); //model

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
            //drawTest();
            drawTiles(board); // to implement in Form1-guiTiles.cs
            // requires the board object to get the colors decided for the tiles in the model
            // this is done in the control::
            board.populateGameBoard();
            // now the populated tiles (containing a piece) 
            // have been marked as guimustbeupdated

            // wait for a small delay then update the gui accordingly
            //System.Threading.Thread.Sleep(1000);
            updatePiecesGui(board);
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
            updatePiecesGui(board); // removes all the guipieces
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

        private void vsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // drawTiles()
            // establish connection (host)
            // etc
        }

        private void optionsMenu_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm();
            optionForm.ShowDialog();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
