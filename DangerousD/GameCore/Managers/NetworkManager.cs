using Microsoft.Xna.Framework;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System;

namespace DangerousD.GameCore
{
    class NetworkManager
    {
        public delegate void ReceivingHandler(string msg);

        public event ReceivingHandler GetMsg;

        Socket socket;
        IPEndPoint endPoint;
        List<Socket> clientSockets = new List<Socket>();
        Socket HostSocket;
        string state;

        private void Init(string IpAddress)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse(IpAddress);
            int port = 8000;
            endPoint = new IPEndPoint(address, port);
        }
        private void AcceptSockets()
        {
            while (true)
            {
                Socket clientSocket = socket.Accept();
                clientSockets.Add(clientSocket);
                Thread receiveThread = new Thread(BeginHostReceive);
                receiveThread.Start(clientSocket);
                Console.WriteLine("Connected");

            }
        }
        private void BeginHostReceive(object clSocket)
        {
            Socket clientSocket = clSocket as Socket;
            while (clientSocket != null)
            {
                byte[] Data = new byte[256];
                int count = clientSocket.Receive(Data);
                string msg = Encoding.Unicode.GetString(Data, 0, count);
                GetMsg(msg);
            }
        }
        public void HostInit(string IpAddress)
        {
            Init(IpAddress);
            socket.Bind(endPoint);
            socket.Listen(4);
            Thread acceptThread = new Thread(AcceptSockets);
            acceptThread.Start();
            state = "Host";
            Console.WriteLine("Start Accept");
        }
        public void ClientInit(string IpAddress)
        {
            Init(IpAddress);
            socket.Connect(endPoint);
            state = "Client";
            Thread.Sleep(10);
        }
        public void SendMsg(string msg)
        {
            byte[] Data = Encoding.Unicode.GetBytes(msg);
            if (state == "Host")
            {
                foreach (Socket socket in clientSockets)
                {
                    socket.Send(Data);
                }
            }
            else
            {
                socket.Send(Data);
            }
        }
        public string ReceiveMsgFromHost()
        {
            byte[] Data = new byte[256];
            int count = socket.Receive(Data);
            return Encoding.Unicode.GetString(Data, 0, count);
        }
    }
}
