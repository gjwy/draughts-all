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

            int i = 0;
            byte[] data;
            byte[] dataRead = new byte[1024]; // shpuld be large enough for my stuff
            int sizeRead;
            string recvdMessage = "";
            while (recvdMessage !=  "(from host) last message")
            {
                Thread.Sleep(0000);
                if (iostream.CanWrite)
                {
                    data = Encoding.ASCII.GetBytes("(from client) " + i.ToString());
                    iostream.Write(data, 0, data.Length);
                    i++;
                }
                if (iostream.CanRead)
                {
                    sizeRead = iostream.Read(dataRead, 0, dataRead.Length);
                    recvdMessage = Encoding.ASCII.GetString(dataRead, 0, sizeRead);
                    System.Console.Write("received: {0}", recvdMessage);
                    System.Console.Write("| s: " + true.ToString());
                    System.Console.Write("| on thread " + Thread.CurrentThread.ManagedThreadId);
                    System.Console.WriteLine(" (main standin)");
                }
            }
            data = Encoding.ASCII.GetBytes("(from client) last message");
            iostream.Write(data, 0, data.Length);
            c.Close();

            System.Console.WriteLine("null | null | null");

            Console.ReadKey();

        }

    }
}
