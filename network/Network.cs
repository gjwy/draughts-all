using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.ComponentModel;

using System.Runtime.Serialization.Formatters.Binary;


using checkers; // for Data
using System.IO;

namespace network
{
    public class Network
    {

        private Data d;
        private int local_port;
        private int remote_port;
        private IPAddress local_ip;
        private IPAddress remote_ip;

        private string local_player;
        private string remote_player;

        private TcpClient gameClient;
        private TcpListener connectionListener;

        private NetworkStream iostream = null;
        private bool connectionIsEstablished = false;

        private bool isRecvdItem = false;
        private bool isSentItem = false;
        private DataStreamObject recv = new DataStreamObject();
        private DataStreamObject send = new DataStreamObject(); // cant use auto-prop since wish to only set SEND etc

        public string flag { get; set; } = "";

        BinaryFormatter formatter = new BinaryFormatter();

        //Thread connectionThread;



        public Network(Data d)
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

     



        public void host(BackgroundWorker w, DoWorkEventArgs e)
        {
            // this while loop allows for the pending method to be used
            // this use prevents the accepttcpclient from blocking
            // this allows the abort event to be sent by the main thread 
            // if the process is cancelled by the user

            System.Console.WriteLine("====== NW WORKER ======");
            System.Console.WriteLine("= host ip is " + local_ip);
            System.Console.WriteLine("= host assigning players");
            local_player = d.Options["Start Player"];
            remote_player = (local_player == "red") ? "white" : "red";


            System.Console.WriteLine("= host creating ep");
            IPEndPoint ep = new IPEndPoint(local_ip, local_port);
            
            connectionListener = new TcpListener(ep);
            connectionListener.Start();

            while (!connectionIsEstablished)
            {
                // check to make sure the main thread hasnt issued a cancel work
                // to this worker thread
                if (w.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                if (connectionListener.Pending())
                {
                    gameClient = connectionListener.AcceptTcpClient();

                    System.Console.WriteLine("= received a connect response");
                    iostream = gameClient.GetStream();
                    connectionIsEstablished = true;
                }

            }






        }

        public void close_host_connection()
        {
            try
            {
                // will only close if its not null (was actually used)
                // this also closes the associated datastream (iostream)
                gameClient.Close();
            }
            catch (NullReferenceException e)
            {
                System.Console.WriteLine("canceled before a client connected");
                // as opposed to timeout or a normal use-case endgame closing
            }

            try
            {
                connectionListener.Stop();
            }
            catch (NullReferenceException e)
            {
                System.Console.WriteLine("the listener was null?");
            }

            
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

        public void create_threadedStream()
        {
            Thread t = new Thread(threadedStream);
            t.Start();
        }



        /* Run in worker thread, only called by host/join_connection functions */
        // places and reads from two field variables
        private void threadedStream()
        {
            System.Console.WriteLine("===========threaded stream created");
            while (true)
            {
                
                if (iostream.CanWrite && this.flag == "send" && !this.IsSentItem) // && send is set?
                {
                    while(!isSentItem)
                    {
                       // System.Console.WriteLine("=begin send");

                        formatter.Serialize(iostream, this.send);
                       // System.Console.WriteLine("=end send");

                        // syncronisation linmes

                        this.isSentItem = true;
                    }

                    // clear the send after sending?

                // clear the receive
                    


                }
                if (iostream.CanRead && this.flag == "recv" && !isRecvdItem)
                {
                    //System.Console.WriteLine("=begin read");
                    this.recv = (DataStreamObject) formatter.Deserialize(iostream);
                    isRecvdItem = true;
                    //System.Console.WriteLine("=end read");
                    
                }
            }
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

        public bool IsRecvItem
        {
            get
            {
                return this.isRecvdItem;
            }
            set
            {
                this.isRecvdItem = value;
            }
            
        }

        public DataStreamObject Recv()
        {
            // clear it ready for next receive
            return recv;
        }


        public DataStreamObject Send
        {
            set
            {

                send = value;
                isSentItem = false;
            }
        }

        public bool IsSentItem
        {
            get
            {
                return this.isSentItem;
            }
            set
            {
                this.isSentItem = value;
            }
        }

        public bool streamExists()
        {
            return !(null == this.iostream);
        }

        public bool isConnect()
        {
            return connectionIsEstablished;
        }


   

    }

}
