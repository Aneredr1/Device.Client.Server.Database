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
using System.Security.Principal;

namespace Server
{
    class Program
    {


        private static byte[] result = new byte[1024];
        private static int myPort = 8080;
        static Socket serverSocket;
        static DateTime dateSync;
        private const string key = "MasterOfPuppets-PuppetsOfMaster"; 
        static List<Message> sendMessages = new List<Message>();

        //static List<SecurityToken> Tokens = new List<SecurityToken>();


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
                Socket serverr = serverSocket.Accept();

                ClientAutorize(serverr);
            }
        }


        /// <summary>
        /// Проверка на наличие токена
        /// </summary>
        /// <param name="serverAcceptSocket"></param>
        /// <returns>true - начать отправку сообщений. false - запросить логин/пароль</returns>
        static void ClientAutorize(object serverr)
        {
            Socket myServer = (Socket)serverr;
                
                    int n = myServer.Receive(result);

                    string[] res = Encoding.UTF8.GetString(result, 0, n).Split(":");
                    var tokenHandler = new JwtSecurityTokenHandler();
                    switch (res[0]) 
                    {
                        case "login": 
                            {
                                using (UserDataContext db = new UserDataContext())
                                {
                                
                                    string[] res1 = res[1].Split(";");
                                    if (db.Users.Any(x => x.Login == res1[0] && x.Password == res1[1]))
                                    {
                                    
                                        var tokenKey = Encoding.UTF8.GetBytes(res1[0] + "kostilbolshoyaue");
                                        var tokenDescriptor = new SecurityTokenDescriptor
                                        {
                                            Subject = new ClaimsIdentity(new Claim[]
                                            {
                                        new Claim(ClaimTypes.Name, res1[0])
                                            }),
                                            Expires = DateTime.UtcNow.AddHours(1),
                                            SigningCredentials = new SigningCredentials(
                                                    new SymmetricSecurityKey(tokenKey),
                                                    SecurityAlgorithms.HmacSha256Signature)
                                            
                                        };
                                        var token = tokenHandler.CreateToken(tokenDescriptor);
                                       // Tokens.Add(token);
                                    myServer.Send(Encoding.UTF8.GetBytes(tokenHandler.WriteToken(token)));
                                    myServer.Shutdown(SocketShutdown.Both);
                                    }
                                    else
                                    {
                                    myServer.Send(Encoding.UTF8.GetBytes("NoAuf"));
                                    myServer.Shutdown(SocketShutdown.Both);
                                    }
                                }
                            } break;
                        case "token":
                            {
                        SecurityToken receivedToken = tokenHandler.ReadToken(res[1]);
                        var handler = new JwtSecurityTokenHandler();
                       // handler.ValidateToken(receivedToken,);
                        //Tokens.Any(x => x.SecurityKey == tokenHandler.ReadToken(res[1]).SecurityKey) && (tokenHandler.ReadToken(res[1]).ValidTo.Date >= DateTime.Now)
                        if (true)
                                {
                                    Thread thread = new Thread(SendMessage);
                                    thread.Start(myServer);
                                }
                        
                            } break;
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
                    foreach (Message mes in sendMessages)
                    {
                        socket.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize<Message>(mes)));
                    }
                }
                sendMessages.Clear();
                Thread.Sleep(800);
            }
        }

        
        private static string GenerateToken(string login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: "Server",
                audience: login,
                claims: new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, login)
                },
                expires: DateTime.UtcNow.AddDays(1));

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }

        private static bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Server",
                ValidAudience = "Client",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) // The same key as the one that generate the token
            };
        }

        static void GenerateMessages()
            {
                // Генерация сообщений для отправки клиенту
                Random rand = new Random();
                while (true)
                {
                    string device_mes = DataWorker.get_surgard();

                    if (DataWorker.write_journal(device_mes))
                    {
                        sendMessages.Add(DataWorker.make_message());
                        Console.WriteLine("Я записав");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка записи сообщения");
                    }
                    Thread.Sleep(rand.Next(800, 7000));
                }
            }

        }
}
