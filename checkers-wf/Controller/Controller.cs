using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using checkers;
using network;


namespace checkers_wf.Controller
{
    public class Controller
    {

        private Data data;
        private Network network;
        private Board board;
        private View view;

        // newgame() set up gui/internal state
        // set up network service
        // main control loop 
        public Controller()
        {

            // starts the View module running
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            board = new Board();
            view = new View(board);
            Application.Run(view);

            begin();
        }

        private void begin()
        {
            System.Console.WriteLine("ASDASDASDASDASDASDASDASDASDASDASDPPP");
        }


    }
}
