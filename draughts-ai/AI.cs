using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace draughts_ai
{
    class AI
    {
        // private Board board; 
        // for testing, specify the format of the board
        private List<?> board;
        public AI() { }

        /* the board/player info recvd in form of Board obj
         * this function primarily ensures it is of the correct 
         * format and no errors, uses the supporting private methods to
         * return the best move*/
        public Move findBestMove(Board board)
        {

        }


        /* need to finish basic implementation first
        * and ensure
        1. player changes model/logic or ai selects a move to make to change the logic
        2. the gui is refreshed

        1. player changes model/logic
        2. the gui is refreshed
        3. the state is sent to next player
        4. LOOP


        /* typical AI turn:
         * receive state
         * syncronise with ais piece positions array (this allows just needing to check pieces, rather than empty squares)
         * check for any priority jumps
         * if jumps, select the *best*
         * else check for normal moves
         * if normal moves, select *best*
         * else GOTO "noValidMoves" -> sendback STATE plus end game
         * ai gives back its move selection
         * it is used to change model/logic
         * the gui is refreshed
         *
         * maintain an array of ais piece locations in ai
         * updated when ai moves on its turn
         * and when state is received at start of turn (just check everything still as before)
         * only thing is some prev pieces could be gone (jumped in player's turn)

    }
}
