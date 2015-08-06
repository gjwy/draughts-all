using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace checkers
{
    public class Tile
    {
        private bool isOccupied;
        private Piece occupyingPiece;
        private string tileIcon;
        private System.Drawing.Image realTileIcon;
        bool isOccupyingPieceKing;
        Coord tileCoord;
        bool isHighlighted;
        bool guiMustBeUpdated;


        public Tile()
        {
            isOccupied = false;
            occupyingPiece = null;
            tileIcon = null;
            realTileIcon = null;
            isOccupyingPieceKing = false;
            tileCoord = null;
            isHighlighted = false;
            guiMustBeUpdated = false;

        }

        public bool IsOccupied
        {
            get
            {
                return isOccupied;
            }
            set
            {
                isOccupied = value;
            }
        }

        public Piece OccupyingPiece
        {
            get
            {
                return occupyingPiece;
            }
            set
            {
                occupyingPiece = value;
            }
        }

        public Coord TileCoord
        {
            set
            {
                tileCoord = value;
            }
            get
            {
                return tileCoord;
            }
        }

        public string TileIcon
        {
            set
            {
                tileIcon = value;
            }
            get
            {
                return tileIcon;
            }
        }

        public bool IsHighlighted
        {
            set
            {
                isHighlighted = value;
            }
            get
            {
                return isHighlighted;
            }
        }

        public bool GuiMustBeUpdated
        {
            set
            {
                guiMustBeUpdated = value;
            }
            get
            {
                return guiMustBeUpdated;
            }
        }

        public Image RealTileIcon
        {
            get
            {
                return realTileIcon;
            }
            set
            {
                realTileIcon = value;
            }
        }


    }
}
