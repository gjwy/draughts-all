using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace draughts_ai
{
    class Program
    {
        static void Main(string[] args)
        {

            AI ai = new AI();

            // get a Board object (contains state), and a AIplayer="red"
            ai.receive(Board board); //
            
        }
    }
}
