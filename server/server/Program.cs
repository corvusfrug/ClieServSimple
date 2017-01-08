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
        static IPEndPoint ipEndPoint = null;
        static IPAddress ip = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Введи help для получения справки");
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
                    if (ip == null)
                    {
                        IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                        ip = ipHost.AddressList[0];
                    }

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

                // Установить порт
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
                // Посмотреть что есть в сети
                if (command.StartsWith("get host net"))
                {
                    PrintHostNet();
                }
                if (command.StartsWith("set ip"))
                {
                    string[] tmp = command.Split(' ');
                    try
                    {
                        if (tmp.Length > 2) throw new Exception();
                        ip = new IPAddress(Int32.Parse(tmp[2]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0} Not Correct ip!", ex.Message);
                        ip = null;
                    }
                    finally
                    {
                        Console.WriteLine("IP: null");
                    }
                }
                // Текущее состояние
                if (command.StartsWith("status"))
                {
                    Console.WriteLine("Server IP: {0}", ip);
                    Console.WriteLine("Server port: {0}", port);
                    Console.WriteLine("Server EndPoint: {0}", ipEndPoint);
                }
                // Текущее состояние
                if (command.StartsWith("help"))
                {
                    PrintHelp();
                }

            }
        }
        static void PrintHostNet()
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            foreach(IPAddress i in ipHost.AddressList)
            {
                Console.WriteLine(i);
            }
        }
        static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("************************************************");
            Console.WriteLine("******************** Помощь ********************");
            Console.WriteLine("************************************************");
            Console.WriteLine();
            Console.WriteLine("Команды:");
            Console.WriteLine();

            Console.WriteLine("\thelp");
            Console.WriteLine("Введите эту команду для получения справки о программе");
            Console.WriteLine();

            Console.WriteLine("\tstatus");
            Console.WriteLine("Введите эту команду для получения текущих значений парамметров сервера");
            Console.WriteLine();

            Console.WriteLine("\texit");
            Console.WriteLine("Введите эту команду для выключения сервера и выхода из программы");
            Console.WriteLine();

            Console.WriteLine("\trun");
            Console.WriteLine("Введите эту команду для запуска сервера");
            Console.WriteLine();

            Console.WriteLine("\tset port");
            Console.WriteLine("Введите эту команду и через пробел значение нового порта, порт по умолчанию: 1001");
            Console.WriteLine("Пример: set port 1029");
            Console.WriteLine();

            Console.WriteLine("\tset ip");
            Console.WriteLine("Введите эту команду и через пробел значение нового ip, ip по умолчанию: localhost, т.е. 127.0.0.1");
            Console.WriteLine("Пример: set ip 10.10.10.10");
            Console.WriteLine();

            Console.WriteLine("\tget host net");
            Console.WriteLine("На самом деле фиг знает, что она выводит...");
            Console.WriteLine("\tIPHostEntry ipHost = Dns.GetHostEntry(\"localhost\");");
            Console.WriteLine("\tforeach (IPAddress i in ipHost.AddressList)");
            Console.WriteLine("\t{");
            Console.WriteLine("\t\tConsole.WriteLine(i);");
            Console.WriteLine("\t}");
            Console.WriteLine();

            Console.WriteLine("************************************************");
            Console.WriteLine("************************************************");
        }
    }
}
