using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using checkers;
using network;

namespace checkers_wf.Game
{
    

    class Game
    {
        private Form form;
        private Thread t;
       
        private Data data;
        private View view;
        private Network network;



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

            network = new Network(data);

            t = network.create_connection(); // threaded
            // sleep main
            form = new Form();
            form.Text = "Waiting on a connection...";
            Button b = new Button();
            b.Text = "cancel";
            b.Click += B_Click;
            form.Controls.Add(b);
            form.ControlBox = false;

            while (!network.isConnect())
            {
                form.ShowDialog();
                
                
            }

            if (network.isConnect())
            {
                // connect thread will have terminated on its own
                network.create_threadedStream();
            }


            
           
         
            //view.newGame();
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
