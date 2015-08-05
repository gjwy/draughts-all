using checkers;
using System;

namespace checkers_wf
{
    partial class ViewControler
    {

        private System.Windows.Forms.Panel[][] guiTileRefs;

        private System.Windows.Forms.Panel testPanel;
        private System.Windows.Forms.Label testLabel;

        private void drawTest()
        {
            // 'this' refers to the Form instance
            // eg the same instance of this used in View.Designer.cs
            // these tiles will be laid over top of the tilePanel panel
            // eg they will start at the same location coord

            // create control
            this.testPanel = new System.Windows.Forms.Panel();
            this.testLabel = new System.Windows.Forms.Label();
            // suspend its layout (in case need to add controls to it)
            this.testPanel.SuspendLayout();
            // configure / optionally add more controls to it
            this.testPanel.Location = new System.Drawing.Point(470,250);
            this.testPanel.Name = "testPanel";
            this.testPanel.Size = new System.Drawing.Size(80, 100);
            this.testPanel.TabIndex = 0; // ?
            // add controls to this
            this.testLabel.Text = "testComponent\r\n\r\n(Added in the manual Form1-guiTiles.cs) (partial with Form1)";
            this.testLabel.Size = new System.Drawing.Size(80, 100);
            this.testPanel.Controls.Add(this.testLabel);
            // add this control to the parent control (View in this case)
            this.Controls.Add(this.testPanel);
            // perform Layout
            this.testPanel.ResumeLayout(false);
            this.testPanel.PerformLayout();
        }

        private void undrawTest()
        {
            this.Controls.Remove(testPanel); // the testLabel is also removed since its contained within this
        }

        /* Event response to the vs Computer game option clicked, firstly the board tiles should be drawn
         * the method will need to get the size of the board from the settings (logic/model)
         * it will use the same .py loop method for generating the physical placement of the gui components
         * ADDITIONALLY, it will place a referene of the compontents in an easy to access array. So when the 
         * logic needs to access the tiles it can do so without having to resort to the expensive find_withtag
         * gui method such was used in the .py version */
        private void renderTiles(Model modelBoard)
        {
            // get the size of the board from the logic
            //int size = logic.getSize();
            int size = modelBoard.Size;
            // initialise the refs array
            guiTileRefs = new System.Windows.Forms.Panel[size][];

            int tilePanelDimension = this.tilePanel.Size.Height; // height==width
            int tileSize = this.tilePanel.Size.Height / size;
            // tilePanel dimensions are 322*322, 
            // 8x40p-wide tiles with 1 pixel outermost border

            // starting board position relative to tilePanel == 1,1
            int baseLocationX = 1;
            int baseLocationY = 1;
            for (int row = 0; row < size; row ++) // multi by 
            {

                System.Windows.Forms.Panel[] listOfPanels = new System.Windows.Forms.Panel[size];

                for (int col = 0; col < size; col ++)
                {
                    // create the control
                    System.Windows.Forms.Panel tile = new System.Windows.Forms.Panel();
                    
                    // add control to 'this' form
                    // NOT TIMES PLUS ?=
                    // 
                    int newLocationX = baseLocationX + (col * tileSize); // col * size(tile)
                    int newLocationY = baseLocationY + (row * tileSize);
                    tile.Location = new System.Drawing.Point(newLocationX, newLocationY);
                    tile.Size = new System.Drawing.Size(tileSize, tileSize);
                    // get the tileIcon/color from the logical tile
                    // modelBoard obj already made previously
                    Coord modelCoord = new Coord(row, col);
                    string strColor = modelBoard.getTile(modelCoord).TileIcon;
                    tile.BackColor = System.Drawing.Color.FromName(strColor);
                    // add the event handler
                    tile.Click += new System.EventHandler(tileClicked);

                    tilePanel.Controls.Add(tile);
                    // now add the reference to the gui array
                    listOfPanels[col] = tile;
                    

                }
                guiTileRefs[row] = listOfPanels;
            }
            // after drawing the tiles, disable them from user input
            this.tilePanel.Enabled = false;
        }

        /* null the guiTileRefs, clear the tilePanel controls */
        private void undrawTiles(Model board)
        {
            guiTileRefs = null;
            tilePanel.Controls.Clear();
        }

        // go through the model board, find those with gui needs to be
        // updated set to true, and use the refs array to obtain the gui tile
        // finally set the model back to guineedsupdate=false
        private void renderPieces(Model modelBoard)
        {
            // use gui=true tiles from modelBoard.internalBoard
            // find the corresponding panels in guiTileRefs
            // ADD OR REMOVE THE CONTAINING PIECE ACCORDINGLY
            // mark the modelBoard tiles as gui=false

            

            int size = modelBoard.Size;

            for (int row = 0; row < size; row ++)
            {
                Tile[] rowOfTiles = modelBoard.InternalBoard[row];
                for (int col = 0; col < size; col ++)
                {
                    Tile tile = rowOfTiles[col];
                    if (tile.GuiMustBeUpdated)
                    {
                        try
                        {
                            System.Windows.Forms.Panel guiTile = guiTileRefs[row][col];
                        

                            // this concerns the logic tile at this stage
                            if (tile.IsOccupied) // then add a corresponding piece to the gui
                            {
                                System.Threading.Thread.Sleep(30);
                                // Todo:
                                // get the color/type of the piece

                                //string color = tile.OccupyingPiece.Player; (when properly added the model)
                                string color = tile.OccupyingPiece.Icon;
                            
                                // so create an appropriately colored component
                                // and add it to this guiTile
                                System.Windows.Forms.Panel piecePanel = new System.Windows.Forms.Panel();
                                piecePanel.Size = new System.Drawing.Size(20, 20);
                                piecePanel.BackColor = System.Drawing.Color.FromName(color);
                                piecePanel.Location = new System.Drawing.Point(10, 10);
                                // pass along the corresponding model coordinates of this piece...
                                piecePanel.Click += (sender, eventArgs) => { pieceClicked(sender, tile.TileCoord); };

                                // change it to circle / piece image eventually
                                guiTile.Controls.Add(piecePanel);
                            }
                            else // remove the corresponding gui piece
                                // since this means guineedsupdating=true, and there are NO pieces on the logic tile
                            {
                                // so remove the guiPiece which should currently be on PiecePanel
                                guiTile.Controls.Clear();
                            }
                        // finally mark the tile as gui=false, this is LOGIC CODE
                        }
                        catch (NullReferenceException e)
                        {
                            System.Console.Error.WriteLine("resetting board before its created");
                            // it is better practise to not have this type of error handled here
                            // it is better to have the option to click reset disabled while the
                            // board has not been created yet
                        }

                    }
                }
            }
        }
    }
}
