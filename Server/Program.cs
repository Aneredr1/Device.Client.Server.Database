using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.Json;
using Server.Models;

namespace Server
{
    class Program
    {
        private static byte[] result = new byte[1024];
        private static int myPort = 8885;
        static Socket serverSocket;


        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myPort));
            serverSocket.Listen(10);
            Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            //通过Clientsocket发送数据
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            Console.ReadLine();

        }

        static void ListenClientConnect()
        {
            while (true)
            {
                serverSocket.Accept();
                if (ClientAutorize(serverSocket))
                {
                    Thread sendMessage = new Thread(SendMessage);
                    sendMessage.Start(serverSocket);
                }
                else
                {

                }
                Thread thread = new Thread(MessageHandler);
                thread.Start(serverSocket);
            }
        }


        // Авторизован ли клиент
        static bool ClientAutorize(object serverAcceptSocket)
        {
            Socket myServerAcceptSocket = (Socket)serverAcceptSocket;
            while (true)
            {
                int n = myServerAcceptSocket.Receive(result);
                // чо то там с токенами проверка

                if (true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Отправка ссобщений на клиент
        static void SendMessage(object serverAcceptSocket)
        {

        }

    }
}
