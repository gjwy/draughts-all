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
            this.label2.Text = newMessage;
        }

        private void changeScoreMessage(Dictionary<string, int> captured)
        {
            string score = "Score\n";
            foreach (KeyValuePair<string, int> item in captured)
            {
                score += item.Value + " " + item.Key + " pieces captured";
            }
        }
    }
}
