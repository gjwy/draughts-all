using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using checkers;
using System.Runtime.Serialization;

namespace network

{
    /* The DataStreamObject is str obj dictionary with additional
       methods for encoding and decoding it into a byte array.
       The object may then be transmitted over a network stream. 
       When reading from this object the dictionary values will need to be cast
       correctly. */
       
    [Serializable]
    public class DataStreamObject : Dictionary<String, object>
    {

        //
         
        public DataStreamObject()
        {
            Add("CurrentPlayer", "");
            Add("HostPlayer", "");
            Add("JoiningPlayer", "");
            Add("MoveTotal", null);
            Add("AddInfo", "");
        }

        // the deserialiser requires this contrusctor
        public DataStreamObject(SerializationInfo info, StreamingContext context) : base(info, context) {

        }









        // ACCESSORS

        public string CurrentPlayer
        {
            get
            {
                return (string) this["CurrentPlayer"];
            }
            set
            {
                this["CurrentPlayer"] = value;
            }

        }

        public string HostPlayer
        {
            get
            {
                return (string)this["HostPlayer"];
            }
            set
            {
                this["HostPlayer"] = value;
            }
        }

        public string JoiningPlayer
        {
            get
            {
                return (string)this["JoiningPlayer"];
            }
            set
            {
                this["JoiningPlayer"] = value;
            }
        }

        public Move MoveTotal
        {
            get
            {
                return (Move) this["MoveTotal"];
            }
            set
            {
                this["MoveTotal"] = value;
            }
        }

        public string AddInfo
        {
            get
            {
                return (string)this["AddInfo"];
            }
            set
            {
                this["AddInfo"] = value;
            }
        }
    }
}
