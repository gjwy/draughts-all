using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Runtime.Serialization.Formatters.Binary;

using network;

namespace ConsoleTestApplication
{
    class Program
    {
        private static int r_port = 1727;
        private static IPAddress r_ip = IPAddress.Parse("192.168.20.100");
        private static TcpClient c;

        
        static void Main(string[] args)
        {

            System.Console.WriteLine("===== NW WORKER =====");
            System.Console.WriteLine("= client connecting");
            c = new TcpClient();
            c.Connect(r_ip, r_port);
            System.Console.WriteLine("= refer host");

            NetworkStream iostream = c.GetStream();
            string curP = "red";

            int i = 0;
            int i2 = 0;
            byte[] dataRead = new byte[1024]; // shpuld be large enough for my stuff
            DataStreamObject sendmessage;
            DataStreamObject recvdMessage;

            BinaryFormatter formatter = new BinaryFormatter();
            string mode = "recv";

            while (i <= 20)
            {

                if (iostream.CanWrite && mode == "send") // eg us
                {
                    //System.Console.WriteLine("=begin send");
                    sendmessage = new DataStreamObject();
                    sendmessage.AddInfo = i2.ToString();
                    i2++;
                    formatter.Serialize(iostream, sendmessage);
                    //System.Console.WriteLine("=end send");
                    mode = "recv";
                    
                }
                if (iostream.CanRead && mode == "recv") // them, read their move
                {
                    //System.Console.WriteLine("=begin read");
                    recvdMessage = (DataStreamObject) formatter.Deserialize(iostream);
                    System.Console.WriteLine("======= recvd: {0}", recvdMessage.AddInfo);
                    //System.Console.WriteLine("=end read");
                    mode = "send";
                }
                i++;
            }
            sendmessage = new DataStreamObject();
            sendmessage.AddInfo = "STOP";
            formatter.Serialize(iostream, sendmessage);
            //c.Close(); ~let the host close it

            System.Console.WriteLine("null | null | null");

            Console.ReadKey();

        }

    }
}
