using DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace DesktopClient.Model.Sockets
{
    public static class ClientSocket
    {
        static Socket clientSocket;
        static byte[] result = new byte[1024];
        static Data data = new Data();
        static private Message message;

        static public Message newMessage
        {
            get { return message; }
            set { message = value; }
        }


        public static void CreateConnection()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(new IPEndPoint(ip, 8080));

            

            
            
        }

        public static bool TryAutorize()
        {
            clientSocket.Send(Encoding.UTF8.GetBytes(data.CurrentUser.user_token));
            int n = clientSocket.Receive(result);
            if (Encoding.UTF8.GetString(result, 0, n) == "NO_AUTORIZE")
            {
                return false;
                //clientSocket.Send(Encoding.UTF8.GetBytes(data.CurrentUser.Login + ";" + data.CurrentUser.Password));

            }
            else
            {
                return true;
                /*while (true)
                {
                    n = clientSocket.Receive(result);
                    // Выводим данные на вьюшку
                }*/
            }
        }

        static void SendToken()
        {
            clientSocket.Send(Encoding.UTF8.GetBytes(data.CurrentUser.user_token));
        }

        public static void GetMessages()
        {
            while (true)
            {
                int n = clientSocket.Receive(result);
                newMessage = JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(result));
            }
        }
    }
}
