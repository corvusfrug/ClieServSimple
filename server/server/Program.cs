using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace server
{
    class Program
    {
        static int port = 1001;
        static IPEndPoint ipEndPoint;
        static void Main(string[] args)
        {
            const string QUIT = "exit";
            string command = "";
            while(command!=QUIT)
            {
                /*IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = ipHost.AddressList[0];
                //IPAddress ipAddr2 = new IPAddress(new byte[] { 10, 10, 10, 30});

                Console.WriteLine(ipAddr);*/
                Console.Write("Server: > ");
                command = Console.ReadLine();

                if(command.StartsWith("run"))
                {
                    IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                    IPAddress ip = ipHost.AddressList[0];

                    // Локальная конечная точка, чтобы это не значило...
                    ipEndPoint = new IPEndPoint(ip,port);

                    // Создаем сокет Tcp/Ip
                    Socket ListnerSocet = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    // Прослушиваем лок-ю, кон-ю точку:
                    Console.WriteLine("Listen Endpoint {0}", ipEndPoint);

                    ListnerSocet.Bind(ipEndPoint);
                    ListnerSocet.Listen(5);
                    /*while (true)
                    {

                    }*/
                }

                if (command.StartsWith("set port"))
                {
                    string[] tmp = command.Split(' ');
                    try
                    {
                        if (tmp.Length > 2) throw new Exception();
                        port = Int32.Parse(tmp[2]);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("{0} Not Correct port!", ex.Message);
                        port = 1001;
                    }
                    finally
                    {
                        Console.WriteLine("Port: {0}",port);
                    }
                }

            }
        }
    }
}
