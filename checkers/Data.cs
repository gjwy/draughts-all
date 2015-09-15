using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    public class Data
    {
        // acts like static
        public enum Gamestage { NoClick, OneClick, OngoingCapture_NoClick, OngoingCapture_OneClick, End, None };


        // flow state stuff
        // CONSIDER MOVING TO BOARD/MODEL?
        private string gametype;
        
        private Gamestage stage;
        private Dictionary<string, int> captured; // score
        private string winner;
        private Dictionary<string, string> options;



        // turn variables
        private string current_player; // the current player
        private Tile selected;
        private List<Move> potentialmoves;
        private bool isReadyToSend = true;

        public Data()
        {
            this.options = new Dictionary<string, string>()
            {
                {"Remote Port", "1727" },
                {"Remote Ip", "000.000.000.000" },
                {"Start Player", "red" }
            };


            this.stage = Gamestage.None;



        }

        public string Gametype
        {
            get
            {
                return this.gametype;
            }
            set
            {
                this.gametype = value;
            }
        }


        public Gamestage Stage
        {
            get
            {
                return this.stage;
            }
            set
            {
                this.stage = value;
            }
        }

        public Dictionary<string, int> Captured
        {
            get
            {
                return this.captured;
            }
            set
            {
                this.captured = value;
            }
        }

        public string Winner
        {
            get
            {
                return this.winner;
            }
            set
            {
                this.winner = value;
            }
        }

        public Dictionary<string, string> Options
        {
            get
            {
                return this.options;
            }
            set
            {
                this.options = value;
            }
        }

        public string Current_player
        {
            get
            {
                return this.current_player;
            }
            set
            {
                this.current_player = value;
            }

        }

        public Tile Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;
            }
        }


        public List<Move> Potentialmoves
        {
            get
            {
                return this.potentialmoves;
            }
            set
            {
                this.potentialmoves = value;
            }
        }

        public bool IsReadyToSend
        {
            get
            {
                return this.isReadyToSend;
            }
        }
    

    }
}
