﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using checkers;
/* THIS IS REALLY THE CONTROL FLOW, HANDLERS FOR GUI USER INPUT AND CREATES
NEW GAEMS ETC */
namespace checkers_wf
{
    public partial class ViewControler : Form
    {
        

        //options stuff to be put in options etc
        private string startPlayer = "";

        // flow state stuff
        // CONSIDER MOVING TO BOARD/MODEL?
        private string GAMETYPE;
        private enum Gamestage { NoClick, OneClick, OngoingCapture, End };
        private Gamestage STAGE;
        private Dictionary<string, int> CAPTURED; // score
        private string WINNER;

        // turn variables
        private string PLAYER; // the current player
        private Tile SELECTED;
        private List<Move> POTENTIALMOVES;


        private Board board;

        public ViewControler(Board board)
        {
            // prevent flickering double buffering
            this.DoubleBuffered = true;

            this.board = board; //model

            InitializeComponent(); //view
            // drawTiles(); // will be moved out of the initialiser <<==
            renderTiles(board);

        }

       

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // cleanup stuff
            // eg save open gme option
            // finally quit
            this.Close();
        }


        private void optionsMenu_Click(object sender, EventArgs e)
        {
            OptionForm optionForm = new OptionForm();
            optionForm.ShowDialog();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void vsCompToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
            // ASSERT BOARDSTATE==PREGAME


            //drawTest();
            renderTiles(board); // to implement in View-guiTiles.cs
            // requires the board object to get the colors decided for the tiles in the model
            // this is done in the control::
            board.populateGameBoard();
            // now the populated tiles (containing a piece) 
            // have been marked as guimustbeupdated

            // wait for a small delay then update the gui accordingly
            //System.Threading.Thread.Sleep(1000);
            renderAllPieces(board);
            // offload to logic

            // enable the clicking of the resetMenu_Click item
            resetToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.ToolTipText = "A game is currently in progress";
            // give some success message to the display
            //changeDisplayMessage("(gui) board has been populated");

        }

        private void resetMenu_Click(object sender, EventArgs e)
        {
            
            // unpopulate the game board:
            board.clearGameBoard();
            renderAllPieces(board); // removes all the guipieces
            undrawTiles(board); // undraw the tiles
            resetToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.ToolTipText = null;


            //changeScoreMessage(); // when called without args resets
            changeCapturedDisplay();
            changeDisplayMessage("(gui) board has been reset");
        }

        private void loadGameMenu_Click(object sender, EventArgs e)
        {
            // draw the tiles
            // load a game state
            // also contains player state
        }

        /* player vs player button clicked, setup */
        private void vsPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // drawTiles()
            // establish connection (host)
            // etc
            // LOCAL VS PLAYER GAME
            playerVsplayer();
        }




        // ##################################################################
        /* player vs player game flow set appropriate state
         * could be moved somewhere else, but it requires
         * access to the gui elements as well as model methods */

            // reorganise this between this func and the immediate click handler
        private void playerVsplayer()
        {
            startPlayer = "red";
            board.populateGameBoard();       // model method


            
            resetToolStripMenuItem.Enabled = true;
            newGameToolStripMenuItem.Enabled = false;
            newGameToolStripMenuItem.ToolTipText = "A game is currently in progress";

            
            
            GAMETYPE = "vsPlayer";
            CAPTURED = new Dictionary<string, int>();
            CAPTURED.Add("white", 0);
            CAPTURED.Add("red", 0);
            WINNER = "";

            STAGE = Gamestage.NoClick;
            PLAYER = startPlayer;

            //renderTiles(board);              // gui method
            renderAllPieces(board);             // gui method
            this.tilePanel.Enabled = true; // allows the tiles to be clicked (must be after gui renders)
            //changeScoreMessage(CAPTURED);
            changeCapturedDisplay(CAPTURED);
            changeDisplayMessage("Player " + PLAYER + "'s turn");

            // expect next event to be a player click, dont need to check for valid since its first turn and valid is garunteed
        }








        private void tileClickedHandler(object sender, Coord coord)
        {


            List<Coord> tilesWhichHaveChanged = new List<Coord>();

            Tile tileClicked = board.getTile(coord);

            // eg if stage was noclicks, then this call to the func represents the first click
            if (this.STAGE == Gamestage.NoClick)
            {
                tilesWhichHaveChanged = firstClickMade(tileClicked);
            }
            else if (this.STAGE == Gamestage.OneClick)
            {
                secondClickMade();
            }
            else if (STAGE == Gamestage.OngoingCapture)
            {
                secondClickMadeOfContinuedCapture();
            }

            // finally do stuff eg send the tiles which have changed to be refreshed by the gui display
            finaly();


        }


        private List<Coord> firstClickMade(Tile tileClicked)
        {
            List<Coord> tilesWhichHaveChanged = new List<Coord>();
            // enforce must jump rule if there are pieces with jumps available
            List<Tile> tilesContainingPlayerPiecesWithJumps = board.getTilesContainingPlayerPiecesWithValidMoves(PLAYER, true);

            if (tilesContainingPlayerPiecesWithJumps.Count > 0
                && !tilesContainingPlayerPiecesWithJumps.Contains(tileClicked))
            {
                changeDisplayMessage("You must capture a piece if possible");
            }

            else if (tileClicked.IsOccupied && tileClicked.OccupyingPiece.Player == this.PLAYER)
            {
                // prioritise jumps
                List<Move> availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, true);
                if (availableMoves.Count == 0)
                {
                    // then normal moves
                    availableMoves = board.getValidAvailableMoves(tileClicked.TileCoord, false);
                }



                List<Coord> theTiles = setHighlightsForTiles(availableMoves, true);

                tilesWhichHaveChanged.AddRange(theTiles);

                // the model has been changed, and a record is kept of which corresponding tiles must be chaged in the gui
                // update the state
                SELECTED = tileClicked;
                POTENTIALMOVES = availableMoves;
                STAGE = Gamestage.OneClick;
            }
            else
            {
                System.Console.WriteLine("piece clicked is not of cur player");
            }

            return tilesWhichHaveChanged;

        }
        


        private void secondClickMade()
        {
            // can assume SELECTED holds a tile (with a valid piece on it)
            // and POTENTIALMOVES contain some


            // checks the second click is on a piece/tile thats highlighted (thus its in potential moves)
            if (tileClicked.IsHighlighted)
            {
                System.Console.WriteLine("is highlighted!");
                Tuple<Piece, bool> result = board.movePiece(SELECTED, tileClicked, POTENTIALMOVES);

                List<Coord> tilesToHighlight = new List<Coord>();
                foreach (Move move in POTENTIALMOVES) // remove the highlights
                {
                    tilesToHighlight.Add(move.ToPos); // add those whows highlight value WILL be changed..
                }
                board.setHighlightTag(tilesToHighlight, false);

                // the tiles involced in the move will have changed (tileFrom and TileTo) ALSO potentially tileJumped
                tilesToChange.AddRange(tilesToHighlight);
                tilesToChange.Add(SELECTED.TileCoord);
                tilesToChange.Add(tileClicked.TileCoord);

                // check if a piece was captured
                if (result.Item1 != null)
                {
                    Piece captured = result.Item1;
                    tilesToChange.Add(captured.CurrentPosition); // the tile which was jumped must be updated

                    CAPTURED[captured.Player] += 1;
                    SELECTED = tileClicked;
                    // check if the move resulted in a kinging
                    if (result.Item2)
                    {
                        // then the turn has ended so change player etc
                        PLAYER = (PLAYER == "red") ? "white" : "red";
                        changeDisplayMessage("Player " + PLAYER + "'s turn");
                        STAGE = Gamestage.NoClick;
                    }
                    // else not kinged so check for further moves to jump
                    else
                    {
                        List<Move> availableMoves = board.getValidAvailableMoves(coord, true);
                        if (availableMoves.Count > 0)
                        {
                            POTENTIALMOVES = availableMoves;
                            STAGE = Gamestage.OngoingCapture;
                        }
                        else
                        {
                            PLAYER = (PLAYER == "red") ? "white" : "red";
                            changeDisplayMessage("Player " + PLAYER + "'s turn");
                            STAGE = Gamestage.NoClick;
                        }
                    }
                    // for each time the CAPTURED value is changed
                    // update it on the display
                    //changeScoreMessage(CAPTURED);
                    changeCapturedDisplay(CAPTURED);
                }
                else
                {
                    System.Console.WriteLine("changing turn");
                    PLAYER = (PLAYER == "red") ? "white" : "red";
                    changeDisplayMessage("Player " + PLAYER + "'s turn");
                    STAGE = Gamestage.NoClick;
                }

            }
            // else the player has clicked on a non highlighted one of their pieces
            else if ((!tileClicked.IsHighlighted) && tileClicked.IsOccupied && tileClicked.OccupyingPiece.Player == PLAYER)
            {
                System.Console.WriteLine("it thinkks tile is not highlighted AND its occupied by the cur player piece?");
                ////////////
                // ALSO THE NW TILE IS CONSIDERED INVALID
                // SO SOME ISSUE HERE
                System.Console.WriteLine("{0} {1} {2}", tileClicked.IsHighlighted, tileClicked.IsOccupied, tileClicked.OccupyingPiece.Player);
                System.Console.WriteLine("potential moves are:");
                foreach (Move move in POTENTIALMOVES)
                {
                    System.Console.WriteLine(move.ToPos.repr());
                }
                System.Console.WriteLine("and the tile clicked was {0}", tileClicked.TileCoord.repr());





                // this if-else block to make the highlight response nicer

                List<Coord> tilesToHighlight = new List<Coord>();
                foreach (Move move in POTENTIALMOVES) // remove the highlights
                {
                    tilesToHighlight.Add(move.ToPos); // add those whows highlight value WILL be changed..
                }

                // if previous clicked tile is not same as just clicked tile
                if (SELECTED.TileCoord != tileClicked.TileCoord)
                {
                    // then just update the highlight


                    board.setHighlightTag(tilesToHighlight, false);
                    tilesToChange.AddRange(tilesToHighlight);
                    STAGE = Gamestage.NoClick;
                    this.tileClickedHandler(null, coord); // problem could be here
                }
                else
                {
                    // just remove the highlight
                    board.setHighlightTag(tilesToHighlight, false);
                    tilesToChange.AddRange(tilesToHighlight);
                    STAGE = Gamestage.NoClick;
                }
            }

            // sles player has clicked on some invalid tile so remove highlight
            // CONSIDER REMOVING PREVIOUS ELSE ?
            else
            {
                System.Console.WriteLine("some invalid tile?");

                List<Coord> tilesToHighlight = new List<Coord>();
                foreach (Move move in POTENTIALMOVES) // remove the highlights
                {
                    tilesToHighlight.Add(move.ToPos); // add those whows highlight value WILL be changed..
                }

                board.setHighlightTag(tilesToHighlight, false);
                tilesToChange.AddRange(tilesToHighlight);
                STAGE = Gamestage.NoClick;
            }
        }

        private void secondClickMadeOfContinuedCapture()
        {
            if (tileClicked == SELECTED)
            {
                List<Coord> tilesToHighlight = new List<Coord>();
                foreach (Move move in POTENTIALMOVES) // remove the highlights
                {
                    tilesToHighlight.Add(move.ToPos); // add those whows highlight value WILL be changed..
                }

                board.setHighlightTag(tilesToHighlight, false);
                tilesToChange.AddRange(tilesToHighlight);
                SELECTED = tileClicked; // redundant?
                STAGE = Gamestage.OneClick;
            }
            else
            {
                changeDisplayMessage("Player " + PLAYER + ", continue the capture sequence");
            }
        }

        private void finaly()
        {
            // at the end of the processing of the input, check that there
            // are tiles containing pieces with moves for the next player's turn
            // not restricted to onlyJumps
            // PLAYER is already set to this next player (during the function)
            // this is the win condition
            List<Tile> tilesContainingPlayerPiecesWithMoves = board.getTilesContainingPlayerPiecesWithValidMoves(PLAYER, false);
            if (tilesContainingPlayerPiecesWithMoves.Count == 0)
            {
                STAGE = Gamestage.End;
                // winner = changePlayer
                WINNER = (PLAYER == "red") ? "white" : "red";
            }

            // check that game has ended
            if (STAGE == Gamestage.End)
            {
                // display winner / scores
                // no valid moves for ~PLAYER, 
                changeDisplayMessage("Player " + WINNER + " wins!");

                // TODO: cleanup game and prog to be ready for next game

            }

            // at very end of this function (changes have been made to model)
            // so update the gui with the changes
            //renderAllPieces(board); // gui method taking model arg
            //changeScoreMessage(CAPTURED); // gui method taking control arg

            updateGuiTiles(tilesToChange);
            changeCapturedDisplay(CAPTURED);
        }

        private List<Coord> setHighlightsForTiles(List<Move> availableMoves, bool highlightBool)
        {
            List<Coord> tilesToHighlight = new List<Coord>();
            foreach (Move move in availableMoves)
            {
                tilesToHighlight.Add(move.ToPos);
            }
            board.setHighlightTag(tilesToHighlight, highlightBool);
            return tilesToHighlight;
        }

    }
}
