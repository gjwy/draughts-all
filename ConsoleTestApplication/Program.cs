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
            byte[] dataRead = new byte[1024]; // shpuld be large enough for my stuff
            DataStreamObject sendmessage;
            DataStreamObject recvdMessage;

            BinaryFormatter formatter = new BinaryFormatter();
            while (i <= 10)
            {
                System.Console.WriteLine(curP);

                if (iostream.CanWrite && curP == "white") // eg us
                {
                    sendmessage = new DataStreamObject();
                    sendmessage.AddInfo = "response";
                    formatter.Serialize(iostream, sendmessage);
                    System.Console.Write("client has finished sending {0}", sendmessage.AddInfo);
                    curP = "red"; // cahange it to them
                }
                if (iostream.CanRead && curP == "red") // them, read their move
                {
                    System.Console.WriteLine("here1");

                    recvdMessage = (DataStreamObject) formatter.Deserialize(iostream);
                    
                    System.Console.Write("i have received: {0}", recvdMessage.AddInfo);
                    // do stuf with it
                    curP = "white";
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
