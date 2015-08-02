using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    public class Logic
    {

        private Board board;
        private Gui gui;

        public Logic(Board board, Gui gui)
        {
            this.board = board;
            this.gui = gui;
        }

        public void test()
        {
            Console.WriteLine("test1");
            Board b = new Board();
            b.populateGameBoard();
            Coord c = new Coord(2, 2); //this piece
            List<Move> vm = b.getValidAvailableMoves(c);
            Console.WriteLine("board created and populated");
            Console.WriteLine(">: NUM OF VALID MOVES FOR THIS PIECE {0}", vm.Count); //this piece
        }



        
    }
}
