using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace checkers
{
    /* .py uses one Move object for both 1,1 and 2,2 reaches.
     * .cs will use a Move object for each reach. This will require
     * an additional property to identify what the reach is. The 
     * isJump is only required on the 2,2 reach. 
     * -- perhaps additional property not needed then...
     * */
    public class Move
    {
        private Coord fromPos;
        private Coord toPos;
        private string moveType; //adjacent or jump
        private Coord jumpedPos = null; //to be added if type is jump

        public Move(Coord fromPos, Coord toPos, string moveType)
        {
            this.fromPos = fromPos;
            this.toPos = toPos;
            this.moveType = moveType;
        }

        // constructor for msking moves based on movetype
        public Move(Coord fromPos, string moveType, Dictionary<string, int> instruction)
        {
            this.fromPos = fromPos;
            this.moveType = moveType;
            // apply the instruction to obtain the toPos
            this.toPos = this.fromPos.applyToFindToPos(instruction);
            // TODO::
            // if moveType == JMP: then instruction should contain
            // a reference for the tile coord which was jumped
            // this will be added etc
            if (moveType == "jmp")
            {
                this.jumpedPos = this.fromPos.applyToFindJumpedPos(instruction);
            }




        }

        public Coord FromPos
        {
            get
            {
                return fromPos;
            }
        }

        public Coord ToPos
        {
            get
            {
                return toPos;
            }
            // set
        }

        public Coord JumpedPos
        {
            get
            {
                return jumpedPos;
            }
        }

        public string MoveType
        {
            get
            {
                return moveType;
            }
        }

    }
}
