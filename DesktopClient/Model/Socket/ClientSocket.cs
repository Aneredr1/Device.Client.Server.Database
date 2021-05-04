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
            if (data.CurrentUser == null)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes("1"));
            }
            else 
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(data.CurrentUser.user_token));
            }

            int n = clientSocket.Receive(result);

            
            if (Encoding.UTF8.GetString(result, 0, n) == "NO_AUTORIZE")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void GetMessages()
        {
            while (true)
            {
                int n = clientSocket.Receive(result);
                newMessage = JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(result));
            }
        }

        public static bool SendLoginAndPassword(string login, string pasword)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes(login + ";" + pasword));
            int n = clientSocket.Receive(result);
            if (Encoding.UTF8.GetString(result, 0, n) == "INVALID_AUTORIZE")
            {
                return false;
            }
            else
            {
                data.CurrentUser.Login = login;
                data.CurrentUser.Password = pasword;
                data.CurrentUser.user_token = Encoding.UTF8.GetString(result, 0, n);
                data.SerializeUser();
                return true;
            }
        }
    }
}
