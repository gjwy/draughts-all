using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace network
{
    public class NetworkInterface
    {
        string current_player; // ref which is polled

        string local_player;
        string remote_player;

        int local_port;
        int remote_port;
        string remote_ip;

        public NetworkInterface(Dictionary<string, string> options, string current_player)
        {
      
            try
            {
                local_port = Int32.Parse(options["Remote Port"]);
            }
            catch (FormatException)
            {
                System.Console.WriteLine("local port not valid setting");
            }
            // player is the CURRENT_PLAYER / HOST player
            this.current_player = current_player;
        }

        /* if hosting, determine currentplayer (startplayer) */
        // THREADED METHOD
        public void hostGame()
        {
            // if hosting set local player to currentplayer
            local_player = current_player;
            remote_player = (local_player == "red") ? "white" : "red";

            connect();
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

        public void connect()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // https://msdn.microsoft.com/en-us/library/system.net.sockets.socket.listen(v=vs.110).aspx
        }


    }

}
