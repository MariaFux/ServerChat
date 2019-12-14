using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerChat
{
    class Program
    {
        //private const string _serverHost = "home-pc";
        private const string _serverHost = "localhost";
        //private const string _serverHost = "169.254.73.105";
        private const int _serverPort = 9933;
        private static Thread _serverThread;
        
        //запускающий метод
        static void Main(string[] args)
        {
            _serverThread = new Thread(startServer);
            _serverThread.IsBackground = true;
            _serverThread.Start();
            while (true)
            {
                handlerCommands(Console.ReadLine());
            }                
        }

        //метод обработки подключений новых пользователей
        private static void handlerCommands(string cmd)
        {
            cmd = cmd.ToLower();
            if (cmd.Contains("/getusers"))
            {
                int countUsers = Server.Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Console.WriteLine("[{0}]: {1}", i, Server.Clients[i].UserName);
                }
            }
        }

        //запуск сервера
        private static void startServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(_serverHost);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, _serverPort);
            Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(iPEndPoint);
            socket.Listen(1000);
            Console.WriteLine("Сервер запущен по IP: {0}.", iPEndPoint);
            while(true)
            {
                try
                {
                    Socket user = socket.Accept();
                    Server.NewClient(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка: {0}.", e.Message);
                }
            }
        }
    }
}
