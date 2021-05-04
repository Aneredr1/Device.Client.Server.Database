using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DesktopClient.Model.Sockets
{
    public static class ClientSocket
    {
        static Socket clientSocket;
        static byte[] result = new byte[1024];

        public static void CreateConnection()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(new IPEndPoint(ip, 8080));

            Data data = new Data();

            clientSocket.Send(Encoding.UTF8.GetBytes(data.CurrentUser.user_token));

            int n = clientSocket.Receive(result);
            if (Encoding.UTF8.GetString(result, 0, n) == "NO_AUTORIZE")
            {
                // Открыть окно ввода логинапароля

                clientSocket.Send(Encoding.UTF8.GetBytes(data.CurrentUser.Login + ";" + data.CurrentUser.Password));

            }
            else
            {
                while (true)
                {
                    n = clientSocket.Receive(result);
                    // Выводим данные на вьюшку
                }
            }
        }

        static void SendToken(object socket)
        {
            Socket socket1 = (Socket)socket;


        }
    }
}
