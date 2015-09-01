using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace network
{
    public class NetworkInterface
    {
        string current_player;
        string this_player;
        int local_port;
        int remote_port;
        string remote_ip;

        public NetworkInterface(Dictionary<string, string> options, string current_player)
        {
            this_player = options["thisPlayer"];
            try
            {
                local_port = Int32.Parse(options["Remote Port"]);
            }
            catch (FormatException)
            {
                System.Console.WriteLine("local port not valid setting");
            }
           
            this.current_player = current_player;
        }

        /* if hosting, determine currentplayer (startplayer) */
        // THREADED METHOD
        public void hostGame()
        {
            // wait for a connect
            // while ()
            // on connect, tell the client their assigned player (not thisplayer)
            // tell them currentplayer (startplayer)
            // send(info)

        }

        /* if joining, host determines currentplayer/startplayer */
        // THREADED METHOD
        public void joinGame()
        {
            // wait to connect
            // receive assigned player, receive currentplayer/move
            // apply move, chage state to allow own move
            // send own move back
        }


    }

}
