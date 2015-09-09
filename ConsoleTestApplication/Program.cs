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

            Thread.Sleep(3453455);
            c.Close();

        }

    }
}
