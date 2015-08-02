using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace checkers
{

    /* The logical Piece object class. This is placed 'on' the logical 
     * game board tiles. A tile may contain a piece and the piece can 
     * be moved from one tile to another. The piece also has 
     * access to the rules governing the types of moves it may make. */
    public class Piece
    {
        //private fields
        private string player;
        private string ptype;
        private Coord currentPosition;
        private List<Coord> posHistory;
        private string icon;

        public Piece(string player, string ptype, Coord initialPosition)
        {
            this.player = player;
            this.ptype = ptype;
            currentPosition = initialPosition; //eg initialPos has been initialised elsewhere, this is assignment
            posHistory = new List<Coord>();//initialise the variable len array of int[2] elements
            posHistory.Add(this.currentPosition);//add initial pos as the first element to it
            icon = (player == "white") ? "white" : "red";
       }

        public void updatePosition(Coord newPosition)
        {
            posHistory.Add(currentPosition);
            currentPosition = newPosition;
            
        }


        /* Returns a list of moves naively available to that piece.
         * It ignores the state of the board and any pieces that might be
         * in the way at this stage. */
        public List<Move> getAvailableMoves()
        {
            
            Coord fromPos = currentPosition; //the pos from which this piece will move

            List<Move> moves = new List<Move>();

            Dictionary<string, int> instruction;
            Move move;
            
            if ((ptype == "d_man") || (ptype == "d_king"))
            {
                // y direction determined from 'player'
                // the JMP moves will contain reference to the jmpd tile/piece
                instruction = MoveInstructionDatabase.getAdvanceMoveWest(player);
                move = new Move(fromPos, "mv", instruction);
                moves.Add(move);


                // since this is a jump type move, the move object will also know to
                // generate the coord which was jumped from the instruction
                // the instruction will accordingly provide this information
                instruction = MoveInstructionDatabase.getAdvanceJumpWest(player);
                move = new Move(fromPos, "jmp", instruction);
                moves.Add(move);

                instruction = MoveInstructionDatabase.getAdvanceMoveEast(player);
                move = new Move(fromPos, "mv", instruction);
                moves.Add(move);

                instruction = MoveInstructionDatabase.getAdvanceJumpEast(player);
                move = new Move(fromPos, "jmp", instruction);
                moves.Add(move);

            }

            if (ptype == "d_king") // also compute the move on the opposite
                                   // y direction (retreat) for the king piece
            {
          
                instruction = MoveInstructionDatabase.getRetreatMoveWest(player);
                move = new Move(fromPos, "mv", instruction);
                moves.Add(move);

                instruction = MoveInstructionDatabase.getRetreatJumpWest(player);
                move = new Move(fromPos, "jmp", instruction);
                moves.Add(move);

                instruction = MoveInstructionDatabase.getRetreatMoveEast(player);
                move = new Move(fromPos, "mv", instruction);
                moves.Add(move);

                instruction = MoveInstructionDatabase.getRetreatJumpEast(player);
                move = new Move(fromPos, "jmp", instruction);
                moves.Add(move);
            }
            return moves;

        }
        //upgradeToKing

        public void upgradeToKing()
        {
            ptype = "d_king";
            icon = (player == "red") ? "red" : "white";
        }

        public string Player
        {
            get
            {
                return player;
            }
        }

        public string Ptype
        {
            get
            {
                return ptype;
            }
        }

        public Coord CurrentPosition
        {
            get
            {
                return currentPosition;
            }
        }

        public string Icon
        {
            get
            {
                return icon;
            }
        }

    }
}
