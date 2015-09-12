using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using checkers;
using checkers_wf.Controller;

namespace checkers_wf
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            // creates the controller
            Controller.Controller controller = new Controller.Controller();

        }
    }
}
