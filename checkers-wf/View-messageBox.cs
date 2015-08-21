using System.Collections.Generic;


namespace checkers_wf
{
    partial class ViewControler
    {
        /* This function is called at various stages of the programs execution to
         * update the main message box display under the game display. It can provide
         * an instruction or general information to the player. */


       
        private void changeDisplayMessage(string newMessage)
        {
            this.label1.Text = newMessage;
        }

        private void changeScoreMessage(Dictionary<string, int> captured = null)
        {
            string score = "Score\n";

            if (captured != null)
            {
                foreach (KeyValuePair<string, int> item in captured)
                {
                    score += item.Value + " " + item.Key + " pieces captured\n";
                }
            }

            //this.label1.Text = score;
        }

        private void changeCapturedDisplay(Dictionary<string, int> captured = null)
        {
            // if null dont render for any pile
            // else render them as if they had been placed on the board
            // draw a pile of num PLAYER tiles for each side
            if (captured != null)
            {
                // TODO
                // DRAW a pile of captured pieces
                // implement huffing rule / options / change starting player
                // piece graphics, kinging, highlighting
            }
        }

        private void playAgain()
        {
            this.label3.Text = "Would you like to play again?";
            this.button1.Enabled = true;
            this.button1.Visible = true;
            this.button2.Enabled = true;
            this.button2.Visible = true;
        }

        private void resetPlayAgain()
        {
            this.label3.Text = "";
            this.button1.Enabled = false;
            this.button1.Visible = false;
            this.button2.Enabled = false;
            this.button2.Visible = false;
        }
    }
}
