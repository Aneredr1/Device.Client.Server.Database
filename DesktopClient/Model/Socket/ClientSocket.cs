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

        public static void CreateConnection()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(new IPEndPoint(ip, 8080));
        }

        static void SendToken(object socket)
        {
            Socket socket1 = (Socket)socket;


        }
    }
}
