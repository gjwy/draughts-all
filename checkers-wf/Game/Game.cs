using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using checkers;
using network;
using System.ComponentModel;

namespace checkers_wf.Game
{
    

    class Game
    {
        private Form form;
        BackgroundWorker bw;
        Form f;
        private Thread t;
        Thread t1;

        private Data data;
        private View view;
        private Network network;
        Network nw;



        public Game(View view)
        {
            // everythin is contained within view
            // Game is just a wrapper
            this.view = view;
            this.data = this.view.data;

            start();
            
        }

        // now have all the references we need

        private void start()
        {
            // if game is network set up that stuff
            if (data.Gametype == "host")
            {
                start_nw_host();
            }

            if (data.Gametype == "join")
            {
                start_nw_join();
            }
            if (data.Gametype == "vsPlayer")
            {
                start_vs_player();
            }

            // determine TURN and gamestate

            

            



            
        }

        private void start_nw_host()
        {
            data.Current_player = data.Options["Start Player"]; // should be host
            data.Stage = Data.Gamestage.NoClick;
            nw = new Network(data);

            //t1 = new Thread(threaded_start_nw_host);
            //t1.Start();

            bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();


            waitDialogue();

            // this loop, polls the result of the thread operation
            // but it is this loop which is freezing the gui
            //while (! nw.isConnect())
            //{
            //    Thread.Sleep(2000);
            //    System.Console.WriteLine("t1 not connected yet");
            //}

            //System.Console.WriteLine("t1 has established connection");







            //view.newGame();
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // worker thread completed, back to main thread
            if (e.Error != null)
            {
                System.Console.WriteLine("error1");
            }
            else if (e.Cancelled)
            {
                System.Console.WriteLine("aborted by main (user)");
            }
            else
            {
                System.Console.WriteLine("success");
                //
                // TODO
                // PROCEED WITH GAME
                // CONNECTION ESTABLISHED
                // SEND INITIAL DATA
                // START GAME INSTANCE
                // LOOP UNTIL ENDGAME(host identifies)
                // SEND FINAL DATA?
                // CLOSE CONNECTION

                // CLOSE CONNECTIONS AT END
            }
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Console.WriteLine("in bw thread");
            BackgroundWorker w = sender as BackgroundWorker;
            nw.host(w, e);
        }

        private void waitDialogue()
        {
            f = new Form();
            f.Text = "WAIT...";
            Button b = new Button();
            b.Text = "Cancel";
            b.Click += abortworker;
            f.Controls.Add(b);
            f.ShowDialog();
        }

        private void abortworker(Object sender, System.EventArgs e)
        {
            System.Console.WriteLine("f2 happened");
            System.Console.WriteLine("i was called by " + sender.ToString());
            // end the worker thread if need be etc
            // close the wait dialoge
            if (sender is Button) // OR WAIT TIMEOUT...
            {
                System.Console.WriteLine("user canceled");
                // user canceled, close dialogue and terminate thread
                f.Close();
                //t1.Abort(); // ends the thread upon which the connecting method is working
                // reset the cahnges to socket smade by the t1 thread
                // nwresets,,
                if (bw.WorkerSupportsCancellation == true)
                {
                    bw.CancelAsync();
                }
                nw.close_host_connection();


            }

        }


        private void threaded_start_nw_host()
        {
            
            Thread.Sleep(200); // allow time for dialogue to be displayed by main thread
            // nw.host();
            // if gets to here, connected
            abortworker(this, null);


        }


        private void B_Click(object sender, EventArgs e)
        {
            // if this button clicked
            // terminate the connect thread

            t.Abort();
            System.Console.WriteLine("ive been clicked");
            // call reset procedure
            view.resetProcedure();
            form.Close();
        }

        private void start_nw_join()
        {
            network = new Network(data);
            //network.join_connection();
            //view.newGame();
        }

        private void start_vs_player()
        {
            data.Current_player = data.Options["Start Player"];
            System.Console.WriteLine(data.Current_player + " is the current player");
            data.Stage = Data.Gamestage.NoClick;
            view.newGame();
        }

    }
}
