using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace checkers
{
    public class Board
    {

        private Tile[][] internalBoard; // make 2-dimensional
        private int size;

        public Board(int size = 8)
        {
            this.size = size;
            internalBoard = new Tile[size][]; //creates size-rows
            generateGameBoard();
            //clearGameBoard();
        }

        /* Creates and adds the tiles to the board */
        private void generateGameBoard()
        {
            for (int row = 0; row < size; row ++)
            {
                //int tile = 4;
                Tile[] listOfTiles = new Tile[size];
                //now add the tiles to the list
                for (int col = 0; col < size; col ++)
                {
                    //create the tile based on col / row values
                    Tile tile = new Tile();
                    tile.TileCoord = new Coord(col, row);
                    
                    
                    // assign it its colour (based on position)
                    if ((row % 2) == 0)
                        tile.TileIcon = ((col % 2) == 0) ? "white" : "black";
                    else
                        tile.TileIcon = ((col % 2) == 0) ? "black" : "white";

                    listOfTiles[col] = tile;
                }
                //add the tileList to the board at that row position
                internalBoard[row] = listOfTiles; //for each row, create size-cols
            }
        }

        /* Access all tile objects in the internal board and reset them */
        public void clearGameBoard()
        {
            for (int row = 0; row < size; row ++)
            {
                // for col
                Tile[] rowOfTiles = internalBoard[row]; // list of tiles
                for (int col = 0; col < size; col ++)
                {
                    Tile tile = rowOfTiles[col];
                    // clear the tiles
                    tile.OccupyingPiece = null;
                    tile.IsOccupied = false;
                    tile.IsHighlighted = false;
                    tile.GuiMustBeUpdated = true;
                    // done
                    //System.Console.WriteLine("clearing tile at {0}", tile.TileCoord.repr());
                }
            }
        }

        /* Fills the board with player pieces in the initial board 
         * state and flags them as needing to be updated accordingly 
         * by the gui code. */
        public void populateGameBoard(string rules = null)
        {
            // its better logic to place the pieces on the tile depending
            // on tile coord or tile colour? --tile colour for now
            
            // add men to row (0 - 2) for player white
            addPiecesToRows(0, 2, "white", "d_man");

            addPiecesToRows(size - 3, size - 1, "red", "d_man"); // 5, 7 for player red
        }

        private void addPiecesToRows(int i, int j, string player, string ptype)
        {
            // i and j are the row numbers inclusive which need to be populated
            for (int y = i; y <= j; y++)
            {
                Tile[] row = internalBoard[y];
                for (int x = 0; x < size; x++)
                {
                    Tile tile = row[x];
                    if (tile.TileIcon == "black")
                    {
                        Coord coord = new Coord(x, y);
                        Piece piece = new Piece(player, ptype, coord);
                        tile.OccupyingPiece = piece;
                        tile.IsOccupied = true;
                        tile.GuiMustBeUpdated = true;
                    }
                }
            }
        }

        /* Given a coordinate, return the tile at that position */
        public Tile getTile(Coord coord)
        {

            return internalBoard[coord.Y][coord.X];
        }

        /* Takes as input coordinates and retrieves the logical tile object,
         * then gets a list of *naive available* moves the piece on that 
         * tile can make. The function further sanity checks the available 
         * moves to make sure they are legally on the board and are not 
         * obstructed by other playing pieces. The function returns this 
         * valid set of available moves for the piece on the given start/from position.*/ 
        public List<Move> getValidAvailableMoves(Coord fromCoord, bool onlyJumps=false)
        {
            List<Move> naiveAvailableMoves = new List<Move>();
            Piece piece = null;
            // get the starting tile position
            Tile tile = getTile(fromCoord);
            if (tile.IsOccupied)
            {
                System.Console.WriteLine("tile is occupied");
                // todo

                // 1. get list of naive available moves
                // eg. only some of these moves will be possible
                piece = tile.OccupyingPiece;
                naiveAvailableMoves = piece.getAvailableMoves();
            }

            // 2. filter the impossible/invalid moves from the naive to
            // retrieve a valid subset
            
            List<Move> validAvailableMoves = new List<Move>();
          
            // check each move in the list for validity
            foreach (Move move in naiveAvailableMoves)
            {
                bool moveIsValid = true;
                // check that the coordinates in each toPos are
                // within the range of the board
                moveIsValid = isToPosOnBoard(move); //TODO: change to tile check method

                // now if passed this preliminary check, check if toPos
                // is already occupied
                if (moveIsValid)
                {
                    // get the tile at the toPos
                    Tile endTile = getTile(move.ToPos);
                    if (endTile.IsOccupied)
                    {
                        moveIsValid = false;
                    }
                    // if its jump type move and the toPos is
                    // unoccupied, also must check that the jumped tile
                    // contains a piece and that piece is of opposite 
                    // player
                    // else its unoccupied and type is jump 
                    else if (move.MoveType == "jmp")
                    {
                        // check that the jmpd tile contains a piece
                        // and its the other players
                        Tile jumpedTile = getTile(move.JumpedPos);
                        // if jumped tile is not occupied OR OR jumped piece is the same as jumping player
                        // then the result is an invalid move
                        if (!jumpedTile.IsOccupied || jumpedTile.OccupyingPiece.Player == piece.Player)
                        {
                            moveIsValid = false;
                        }
                    }
                    // else its just unoccupied and that is fine
                    // eg moveIsValid = true
                }

                // now if the move is valid after these checks, add it to
                // the list of valid available moves
                if (moveIsValid)
                {
                    validAvailableMoves.Add(move);
                }
            }
            
            return validAvailableMoves;
        
        }

        /* Check if a given move's toPosition is lying legally on the
         * board. If it is not within the legal range, false is returned.
         * Todo: more elegant way of doing this with a range? */
        private bool isToPosOnBoard(Move move)
        {
            int x = move.ToPos.X;
            int y = move.ToPos.Y;
            return (x >= 0) && (x < size) && (y >= 0) && (y < size);
        }


        /* Return a list containing the tiles which 1) contain the 'player'
         * piece and 2) those pieces have valid available moves
         * eg. getValidAvailableMoves() called for each player piece to
         * determine if the tile/piece has validAvailableMoves. --Expensive */
        private List<Tile> getTilesContainingPlayerPiecesWithValidMoves(string player)
        {
            List<Tile> tilesContainingPlayerPiecesWithValidMoves = new List<Tile>();

            // 1) find all the tiles containing players piece
            for (int row = 0; row < size; row++)
            {
                Tile[] rowOfTiles = internalBoard[row];
                for (int col = 0; col < size; col++)
                {
                    Tile tile = rowOfTiles[col];
                    if (tile.IsOccupied && tile.OccupyingPiece.Player == player)
                    {
                        // 2) use each tiles coord --> (arg fromCoord) to find 
                        //    the available moves from this piece
                        List<Move> validAvailableMoves = getValidAvailableMoves(tile.TileCoord);
                        // 3) if there are valid moves from this tile/piece, add this tile to the 
                        // 3) list
                        if (validAvailableMoves.Count != 0) //there are some valid moves from this tile/piece
                        {
                            // then add the tile/piece (represented by the tile its on)
                            tilesContainingPlayerPiecesWithValidMoves.Add(tile);
                        }
                    }
                }
            }

            return tilesContainingPlayerPiecesWithValidMoves;
        }


        /* Takes a list of logical move objects and marks the 
         * toPos tiles in each move for highlighting. The gui
         * code will then be able to apply the actual highlights
         * accordingly. 
         * 
         * Input is the result returned from the getValidAvailableMoves
         * function. That is, a list of moves for a certain piece. */
        private void setHighlightTag(List<Move> validAvailableMoves, bool highlightValue)
        {
            foreach (Move mv in validAvailableMoves)
            {
                // set the tile in the board at that toPos to the highightValue
                getTile(mv.ToPos).IsHighlighted = highlightValue;
                // since the func returns ref, this should work 
            }
        }

        private bool isPieceOnKingRow(Piece piece)
        {
            int kingRow = (piece.Player == "red") ? 0 : (size-1);
            // check if the row (y) coordinate of the piece is on the kingrow
            bool isOnKingRow = (kingRow == piece.CurrentPosition.Y);
            return isOnKingRow;
        }


        private Tuple<Piece, bool> movePiece(Tile fromTile, Tile toTile, List<Move> listOfMoves)
        {
            // req lsOfMoves to identify the move which has
            // been made, (uses tileA and B to determine)
            // got from getValidAvailableMoves function
            Move identifiedMove = null;
            foreach (Move move in listOfMoves)
            {
                if (move.FromPos == fromTile.TileCoord && move.ToPos == toTile.TileCoord)
                {
                    identifiedMove = move;
                    break;
                }
            }
            if (identifiedMove.Equals(null))
            {
                System.Console.WriteLine("Critical Error (282)");
            }

            // now copy the piece from fromTile to toTile
            toTile.OccupyingPiece = fromTile.OccupyingPiece;
            // make sure the piece now on toTile knows it is
            toTile.OccupyingPiece.updatePosition(toTile.TileCoord);

            // remove the old piece from the fromTile and
            // update its properties
            fromTile.OccupyingPiece = null;
            fromTile.IsOccupied = false;
            fromTile.GuiMustBeUpdated = true;

            // update the properties of the toTile
            toTile.IsOccupied = true;
            toTile.GuiMustBeUpdated = true;

            // now, using knowledge of the move, determine if a
            // piece has been captured
            Piece captured = null; // ============================

            if (identifiedMove.MoveType == "jmp")
            {
                Tile jumpedTile = getTile(identifiedMove.JumpedPos); // ref
                captured = jumpedTile.OccupyingPiece;
                jumpedTile.OccupyingPiece = null;
                jumpedTile.IsOccupied = false;
                jumpedTile.GuiMustBeUpdated = true;
            }

            // now look at the piece and check whether its reached the king row
            bool pieceKinged = false; // =============================

            bool onKingRow = isPieceOnKingRow(toTile.OccupyingPiece);
            if (toTile.OccupyingPiece.Ptype != "d_king" && onKingRow)
            {
                toTile.OccupyingPiece.upgradeToKing();
                pieceKinged = true;

            }

            
            // returns a move outcome, contains whether a piece is 
            // kinged or not (boolean)
            // and a piece (if it is captured otherwise null)
            Tuple<Piece, bool> moveOutcome = Tuple.Create(captured, pieceKinged);
            return moveOutcome;
        }
            



        // Accessors
        public Tile[][] InternalBoard
        {
            get
            {
                return internalBoard;
            }
        }
        public int Size
        {
            get
            {
                return size;
            }
        }

    }
}
