using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    // Done, c onsider changing x,y accessors to a single method
    public class Coord
    {
        private int x;
        private int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public string repr()
        {
            string repr = System.String.Format("({0},{1})", x, y);
            return repr;
        }

        /* Apply an instruction to this coord, resulting in a 
         * returned second coord. Doesnt change this coord. */
        public Coord applyToFindToPos(Dictionary<string, int> instruction)
        {
            Coord result = new Coord(x + instruction["x"], y + instruction["y"]);
            return result;
        }

        public Coord applyToFindJumpedPos(Dictionary<string, int> instruction)
        {
            Coord result = new Coord(x + instruction["jmpdx"], y + instruction["jmpdy"]);
            return result;
        }
    }
}
