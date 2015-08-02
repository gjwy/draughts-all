using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    class Program
    {
        static void Main(string[] args)
        {
            Model board = new Model();           // model
            Gui gui = new Gui();                 // view
            Controller logic = new Controller(board, gui); // controller - model, view goes in here
            
            logic.test();


            /* 
             * board b contains internal board
             * puplic generateboard, clearboard, populateBoard, addPiecestorows, getTile (ref), getPiecesValidAMoves, 
             * getTilesWithPieceMoves, isCoordTileOnBoard, isPieceOnKingRow, movePiece
             * */






        }
    }
}