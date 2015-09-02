using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace network
{
    public class NetworkInterface
    {
        string current_player; // ref which is polled

        string local_player;
        string remote_player;

        int local_port;
        int remote_port;
        IPAddress local_ip;
        IPAddress remote_ip;

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
            System.Console.WriteLine("nw- call the connect method");
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

        /* host the connection */
        public void connect()
        {
            Socket listenerS = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket acceptS;

            local_ip = getLocalIp();
            System.Console.WriteLine("ip is ", local_ip.ToString());

            // create endpoint
            IPEndPoint ep = new IPEndPoint(local_ip, local_port);

            // attach to socket
            try
            {
                listenerS.Bind(ep);
            }
            catch (SocketException e)
            {
                System.Console.WriteLine(e.GetBaseException());
                System.Console.WriteLine(e.ErrorCode);
            }
            


            // listen
            System.Console.WriteLine("nw- listening");
            try
            {
                listenerS.Listen(1);
            }
            catch (SocketException e)
            {
                System.Console.WriteLine(e.GetBaseException());
                System.Console.WriteLine(e.ErrorCode);
            }


            // accept a connection
            try
            {
                acceptS = listenerS.Accept();
            }
            catch (SocketException e)
            {
                System.Console.WriteLine(e.GetBaseException());
                System.Console.WriteLine(e.ErrorCode);
            }

            System.Console.WriteLine("nw- accepted connection");
        }

        /* http://stackoverflow.com/questions/6803073/get-local-ip-address */
        private IPAddress getLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return IPAddress.Parse(ip.ToString());
                }
            }
            throw new Exception("IP not found");
        }



    }

}
