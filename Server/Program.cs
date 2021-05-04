using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.Json;
using Server.Models;
using Server.Models.Data;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        private static byte[] result = new byte[1024];
        private static int myPort = 8885;
        static Socket serverSocket;
        static DateTime dateSync;
        static List<Message> sendMessages;


        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myPort));
            serverSocket.Listen(10);
            
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();

            Thread generateMessage = new Thread(GenerateMessages);
            generateMessage.Start();
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
                    Thread startAutorize = new Thread(StartAutorize);
                    startAutorize.Start(serverSocket);
                }
                
            }
        }


        /// <summary>
        /// Проверка на наличие токена
        /// </summary>
        /// <param name="serverAcceptSocket"></param>
        /// <returns>true - начать отправку сообщений. false - запросить логин/пароль</returns>
        static bool ClientAutorize(object serverAcceptSocket)
        {
            Socket myServerAcceptSocket = (Socket)serverAcceptSocket;
            while (true)
            {
                int n = myServerAcceptSocket.Receive(result);
                // Существует/актуален ли токен пользователя
                using (UserDataContext db = new UserDataContext())
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var user = db.Users.FirstOrDefault(x => x.user_token == tokenHandler.ReadToken(Encoding.UTF8.GetString(result, 0, n)));
                    if (user != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }            
            }
        }

        // Отправка ссобщений на клиент
        static void SendMessage(object serverAcceptSocket)
        {
            Socket socket = (Socket)serverAcceptSocket;
            while (true)
            {
                using (SurGardDataContext db = new SurGardDataContext())
                {
                    foreach (var item in sendMessages)
                    {
                        socket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(item)));
                    }
                }
                sendMessages.Clear();
                Thread.Sleep(800);
            }
        }

        // Авторизация с последущей выдачей токена
        static void StartAutorize(object serverAcceptSocket)
        {
            Socket socket = (Socket)serverAcceptSocket;
            socket.Send(Encoding.UTF8.GetBytes("NO_AUTORIZE"));
            while (true)
            {
                int n = socket.Receive(result);
                string[] res = Encoding.UTF8.GetString(result).Split(";");
                using (UserDataContext db = new UserDataContext())
                {
                    if (db.Users.Any(x => x.Login == res[0] && x.Password == res[1]))
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenKey = Encoding.UTF8.GetBytes(res[0]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, res[0])
                            }),
                            Expires = DateTime.UtcNow.AddHours(1),
                            SigningCredentials = new SigningCredentials(
                                    new SymmetricSecurityKey(tokenKey),
                                    SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var user = db.Users.FirstOrDefault(x => x.Login == res[0]);
                        user.user_token = tokenHandler.WriteToken(token);
                        db.Users.Update(user);
                        db.SaveChanges();
                        socket.Send(Encoding.UTF8.GetBytes(tokenHandler.WriteToken(token)));
                    }
                    else
                    {
                        socket.Send(Encoding.UTF8.GetBytes("INVALID_AUTORIZE"));
                        socket.Shutdown(SocketShutdown.Both);
                    }
                }
                socket.Shutdown(SocketShutdown.Both);
                break;
            }
        }

        static void GenerateMessages()
        {
            // Генерация сообщений для отправки клиенту
            Random rand = new Random();
            while (true)
            {
                sendMessages.Add(DataWorker.get_message());
                Thread.Sleep(rand.Next(800, 7000));
            }
        }

    }
}
