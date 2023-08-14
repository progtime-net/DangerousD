using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System;

namespace DangerousD.GameCore.Network
{
    public class NetworkManagerTest
    {
        public delegate void ReceivingHandler(string msg);

        public event ReceivingHandler GetMsg;

        Socket socket;
        IPEndPoint endPoint;
        List<Socket> clientSockets = new List<Socket>();
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
                byte[] bytesCount = new byte[4];
                clientSocket.Receive(bytesCount);
                byte[] Data = new byte[BitConverter.ToInt32(bytesCount)];
                StateObject so = new StateObject(clientSocket, Data);
                IAsyncResult count = clientSocket.BeginReceive(so.buffer, 0, so.bufferSize, SocketFlags.None, AsyncReceiveCallback, so);
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
            Thread ReceivingThread = new Thread(ReceiveMsgFromHost);
            ReceivingThread.Start();
        }
        public void SendMsg(string msg)
        {
            byte[] Data = Encoding.Unicode.GetBytes(msg);
            int count = Data.Length;
            if (state == "Host")
            {
                foreach (Socket socket in clientSockets)
                {
                    socket.Send(BitConverter.GetBytes(count));
                    socket.Send(Data);
                }
            }
            else
            {
                socket.Send(BitConverter.GetBytes(count));
                socket.Send(Data);
            }
        }
        public void ReceiveMsgFromHost()
        {
            while (true)
            {
                byte[] bytesCount = new byte[4];
                socket.Receive(bytesCount);
                byte[] Data = new byte[BitConverter.ToInt32(bytesCount)];
                StateObject so = new StateObject(socket, Data);
                IAsyncResult count = socket.BeginReceive(so.buffer, 0, so.bufferSize, SocketFlags.None, AsyncReceiveCallback, so);
            }
        }

        public void AsyncReceiveCallback(IAsyncResult ar)
        {
            StateObject so = ar.AsyncState as StateObject;
            Socket clientSocket = so.workSocket;
            int readCount = clientSocket.EndReceive(ar);
            so.UploadedBytesCount += readCount;
            so.sb.Append(Encoding.Unicode.GetString(so.buffer, 0, readCount));
            if (so.UploadedBytesCount < so.bufferSize)
            {
                clientSocket.BeginReceive(so.buffer, 0, so.bufferSize, SocketFlags.None, new AsyncCallback(AsyncReceiveCallback), so);
            }
            else
            {
                GetMsg(so.sb.ToString());
            }
        }
    }
}
