using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using checkers; // for Data

namespace network
{
    public class NetworkInterface
    {

        private Data d;
        private int local_port;
        private int remote_port;
        private IPAddress local_ip;
        private IPAddress remote_ip;

        private string local_player;
        private string remote_player;

        private TcpListener connectionListener; // offloads a connection 
        // to the gameClient

        private TcpClient gameClient;
        private NetworkStream iostream;

        public NetworkInterface(Data d)
        {
      
            try
            {
                local_port = remote_port = Int32.Parse(d.Options["Remote Port"]);
                remote_ip = IPAddress.Parse(d.Options["Remote Ip"]);
            }
            catch (FormatException)
            {
                System.Console.WriteLine("local port not valid setting");
            }

            // only required for connecting
            local_ip = getLocalIp();
            this.d = d;
        }

        /* if hosting, determine currentplayer (startplayer) */
        // THREADED METHOD
        public void host()
        {
            System.Console.WriteLine("====== NW WORKER ======");
            System.Console.WriteLine("= host ip is " + local_ip);
            System.Console.WriteLine("= host assigning players");
            local_player = d.Options["Start Player"];
            remote_player = (local_player == "red") ? "white" : "red";


            System.Console.WriteLine("= host creating ep");
            IPEndPoint ep = new IPEndPoint(local_ip, local_port);
            connectionListener = new TcpListener(ep);
            connectionListener.Start();
            while(true)
            {
                System.Console.WriteLine("= wait for connection response");
                gameClient = connectionListener.AcceptTcpClient();

                System.Console.WriteLine("= received a connect response");
                iostream = gameClient.GetStream();

                System.Console.WriteLine("= send the initial p assignments");
                // send the initial information (player assignmnents)
                // currentplayer, remoteplayer is you
                // using iostream.send / w/e

                System.Console.WriteLine("= begin the main worker procedure");
                // then write to and read from this stream with seperate threads
                // while not receive particular instruction
                while (true) // until terminate instruction received
                {
                    if (d.Current_player == local_player) // AND move ready to be sent
                    {
                        // try to send the move
                        // then clear it
                        // update current player (after this is sent!)
                    }
                    if (d.Current_player == remote_player)
                    {
                        // try to receive a move
                        // apply the move to this
                        // clear it
                        // update current_player
                    }
                }
            }
            

            // its received a particular instruction so close
            // connection and close the thread
            // System.Console.WriteLine("current player is " + d.Current_player);

            //while (current_player == "red")
            //{
            //    // repeatedly send stuff over this socket
                
            //    System.Console.WriteLine(current_player + local_player + " should variable be cahnung");
            //    string message = "Hello client!" + new Random().Next();
            //    byte[] data = Encoding.ASCII.GetBytes(message);
            //    acceptS.Send(data);

            //    System.Console.WriteLine("Sent a message: '{0}'", message);
            //}
            //acceptS.Close();


            // loop etc
            // wait for a connect
            // while ()
            // on connect, tell the client their assigned player (not thisplayer)
            // tell them currentplayer (startplayer)
            // send(info)

        }


        public void test()
        {
            while (d.Current_player == "red")
            {
                //System.Console.WriteLine("current plater is " + d.Current_player);
                
            }

            // its received a particular instruction so close
            System.Console.WriteLine("current player is " + d.Current_player);
        }

        /* if joining, host determines currentplayer/startplayer */
        // THREADED METHOD
        public void joinGame()
        {


            Socket joinS = join();
            byte[] recvBuff = new byte[256];

            while (true)
            {
                // if ready to be sent,
                //{
                // send
               
                //}

                // clear recvbuff;
                recvBuff = new byte[256];
                int size = joinS.Receive(recvBuff);
                // if ready to receive,
                //{
                string message = Encoding.UTF8.GetString(recvBuff);
                System.Console.WriteLine("Received a message: '{0}'", message);
                // clear recvbuff
                //}

            }
            

            



            

            // wait to connect
            // receive assigned player, receive currentplayer/move
            // apply move, chage state to allow own move
            // send own move back
        }



        private Socket join()
        {
            Socket joinS = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(remote_ip, remote_port);
            joinS.Connect(ep);
            return joinS;
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
