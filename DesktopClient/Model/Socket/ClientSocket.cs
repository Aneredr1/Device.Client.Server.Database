using DesktopClient.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace DesktopClient.Model.Sockets
{
    public static class ClientSocket
    {
        public static Thread StartRecieve;
        public static Socket clientSocket;
        static byte[] result = new byte[1024];
        static private Message message;
        static SecurityToken Token;

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

        public static bool TryAutorize(string login, string password)
        {
            try
            {
                clientSocket.Send(Encoding.UTF8.GetBytes("login:" + login + ";" + password));
                int n = clientSocket.Receive(result);
                switch (Encoding.UTF8.GetString(result, 0, n))
                {
                    case "invalid_auth": return false;
                    default: { 

                                var tokenHandler = new JwtSecurityTokenHandler();
                                Token = tokenHandler.ReadToken(Encoding.UTF8.GetString(result, 0, n));
                                return true; 
                             }
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }


        public static bool StartReceiveMessages()
        {
                ClientSocket.CreateConnection();
                var tokenHandler = new JwtSecurityTokenHandler();
                clientSocket.Send(Encoding.UTF8.GetBytes("token:" + tokenHandler.WriteToken(Token)));
                
                int n = clientSocket.Receive(result);

            switch (Encoding.UTF8.GetString(result, 0, n))
            {
                case "valid_auth": StartRecieve = new Thread(ReceiveMessages); return true;
                case "invalid_auth": return false;
                default: return false;
            }
            
        }
        public static void ReceiveMessages() 
        {
            while (true)
            {
                int n = clientSocket.Receive(result);
                try
                {
                    newMessage = JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(result, 0, n));
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

