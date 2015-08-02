using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    /* This class obj will provide methods for generating
     * various types of move. 
     * -- Could be static... */
    public class MoveInstructionDatabase
    {
        
        public MoveInstructionDatabase()
        {

        }
        

        /* Returns a moveinstruction object which contains
         * info about what values to add to the fromPos/
         * startPos of a move, to obtain the toPos/endPos */
        public static Dictionary<string, int> getAdvanceMoveWest(string player) // (player for y dir)
        {
            // change the advance-y direction depending on which player
            // it is.
            int yCorrection = (player == "white") ? 1 : -1;

            Dictionary<string, int> instruction = new Dictionary<string, int>();
            instruction.Add("x", -1);
            instruction.Add("y", 1 * yCorrection);
            return instruction;
        }

        /* Do same for other moves, FOR JMP moves, include 
         * the jumped middle coord (variation of 1,1) */

        public static Dictionary<string, int> getAdvanceJumpWest(string player)
        {
            int yCorrection = (player == "white") ? 1 : -1;
            Dictionary<string, int> instruction = new Dictionary<string, int>();
            instruction.Add("x", -2);
            instruction.Add("y", 2 * yCorrection);
            // since jump move, add instruction for computing jumped coord (from fromPos)
            instruction.Add("jmpdx", -1);
            instruction.Add("jmpdy", 1 * yCorrection);
            return instruction;
        }
        
        public static Dictionary<string, int> getAdvanceMoveEast(string player)
        {
            int yCorrection = (player == "white") ? 1 : -1;
            Dictionary<string, int> instruction = new Dictionary<string, int>();
            instruction.Add("x", 1);
            instruction.Add("y", 1 * yCorrection);
            return instruction;
        }

        public static Dictionary<string, int> getAdvanceJumpEast(string player)
        {
            int yCorrection = (player == "white") ? 1 : -1;
            Dictionary<string, int> instruction = new Dictionary<string, int>();
            instruction.Add("x", 2);
            instruction.Add("y", 2 * yCorrection);
            // since jump move, add instruction for computing jumped coord (from fromPos)
            instruction.Add("jmpdx", 1);
            instruction.Add("jmpdy", 1 * yCorrection);
            return instruction;

        }
        // ======= ***IFFF type==king, get above, but also get reverse y =======
        // this decission is made in the caller (piece) and 
        // should be easy to make it the same call
        // (get same set of moves) but also (get the reverse y set)

        public static Dictionary<string, int> getRetreatMoveWest(string player)
        {
            int yCorrection = (player == "white") ? -1 : 1;
            // it is yCorrection which is inverted
            Dictionary<string, int> instruction = new Dictionary<string, int>();
            instruction.Add("x", -1);
            instruction.Add("y", 1 * yCorrection);
            return instruction;
        }

        public static Dictionary<string, int> getRetreatJumpWest(string player)
        {
            int yCorrection = (player == "white") ? -1 : 1;
            Dictionary<string, int> instruction = new Dictionary<string, int>();
            instruction.Add("x", -2);
            instruction.Add("y", 2 * yCorrection);

            instruction.Add("jmpdx", -1);
            instruction.Add("jmpdy", 1 * yCorrection);
            return instruction;
        }

        public static Dictionary<string, int> getRetreatMoveEast(string player)
        {
            int yCorrection = (player == "white") ? -1 : 1;
            Dictionary<string, int> instruction = new Dictionary<string, int>();
            instruction.Add("x", 1);
            instruction.Add("y", 1 * yCorrection);
            return instruction;
        }

        public static Dictionary<string, int> getRetreatJumpEast(string player)
        {
            int yCorrection = (player == "white") ? -1 : 1;
            Dictionary<string, int> instruction = new Dictionary<string, int>();
            instruction.Add("x", 2);
            instruction.Add("y", 2 * yCorrection);

            instruction.Add("jmpdx", 1);
            instruction.Add("jmpdy", 1 * yCorrection);
            return instruction;
        }
         
    }
}

