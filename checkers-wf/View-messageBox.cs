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
            }
        }
    }
}
