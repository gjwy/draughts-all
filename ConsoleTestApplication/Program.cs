using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
            byte[] data;
            byte[] dataRead = new byte[1024]; // shpuld be large enough for my stuff
            int sizeRead;
            string sendmessage = "hippo";
            string recvdMessage = "";
            while (i <= 10)
            {
                Thread.Sleep(4000);

                if (iostream.CanWrite && curP == "white") // eg us
                {
                    data = Encoding.ASCII.GetBytes(sendmessage);
                    iostream.Write(data, 0, data.Length);
                    System.Console.Write("i have sent: {0}", sendmessage);
                    data = new byte[1024];
                    curP = "red"; // cahange it to them
                }
                if (iostream.CanRead && curP == "red") // them, read their move
                {
                    sizeRead = iostream.Read(dataRead, 0, dataRead.Length);
                    recvdMessage = Encoding.ASCII.GetString(dataRead, 0, sizeRead);
                    dataRead = new byte[1024];
                    System.Console.Write("i have received: {0}", recvdMessage);
                    // do stuf with it
                    curP = "white";
                }
                i++;
            }
            data = Encoding.ASCII.GetBytes("STOP");
            iostream.Write(data, 0, data.Length);
            //c.Close(); ~let the host close it

            System.Console.WriteLine("null | null | null");

            Console.ReadKey();

        }

    }
}
