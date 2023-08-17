using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System;
using Newtonsoft.Json;

namespace DangerousD.GameCore.Network
{
    public class NetworkManager
    {
        public delegate void ReceivingHandler(NetworkTask networkTask);

        public event ReceivingHandler GetReceivingMessages;

        Socket socket;
        IPEndPoint endPoint;
        List<Socket> clientSockets = new List<Socket>();
        string state;

        private void Init(string IpAddress)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress address = IPAddress.Parse(IpAddress);
                int port = 8000;
                endPoint = new IPEndPoint(address, port);
            }
            catch { }
        }
        private void AcceptSockets()
        {
            while (true)
            {
                try
                {
                    Socket clientSocket = socket.Accept();
                    clientSockets.Add(clientSocket);
                    Thread receiveThread = new Thread(BeginHostReceive);
                    receiveThread.Start(clientSocket);
                }
                catch { }

            }
        }
        private void BeginHostReceive(object clSocket)
        {
            Socket clientSocket = clSocket as Socket;
            while (clientSocket != null)
            {
                try
                {
                    byte[] bytesCount = new byte[4];
                    clientSocket.Receive(bytesCount);
                    byte[] Data = new byte[BitConverter.ToInt32(bytesCount)];
                    StateObject so = new StateObject(clientSocket, Data);
                    IAsyncResult count = clientSocket.BeginReceive(so.buffer, 0, so.bufferSize, SocketFlags.None, AsyncReceiveCallback, so);
                }
                catch { }
            }
        }
        public void HostInit(string IpAddress)
        {
            try
            {
                Init(IpAddress);
                socket.Bind(endPoint);
                socket.Listen(4);
                Thread acceptThread = new Thread(AcceptSockets);
                acceptThread.Start();
                state = "Host";
            }
            catch { }
        }
        public void ClientInit(string IpAddress)
        {
            try
            {
                Init(IpAddress);
                socket.Connect(endPoint);
                state = "Client";
                Thread.Sleep(10);
                Thread ReceivingThread = new Thread(ReceiveMsgFromHost);
                ReceivingThread.Start();
            }
            catch { }
        }
        public void SendMsg(NetworkTask networkTask)
        { 
            byte[] Data = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(networkTask));
            int count = Data.Length;
            if (state == "Host")
            {
                try
                {
                    foreach (Socket socket in clientSockets)
                    {
                        socket.Send(BitConverter.GetBytes(count));
                        socket.Send(Data);
                    }
                }
                catch { }
            }
            else
            {
                try
                {
                    socket.Send(BitConverter.GetBytes(count));
                    socket.Send(Data);
                }
                catch { }
            }
        }
        private void ReceiveMsgFromHost()
        {
            while (true)
            {
                try
                {
                    byte[] bytesCount = new byte[4];
                    socket.Receive(bytesCount);
                    byte[] Data = new byte[BitConverter.ToInt32(bytesCount)];
                    StateObject so = new StateObject(socket, Data);
                    IAsyncResult count = socket.BeginReceive(so.buffer, 0, so.bufferSize, SocketFlags.None, AsyncReceiveCallback, so);
                }
                catch { }
            }
        }

        private void AsyncReceiveCallback(IAsyncResult ar)
        {
            try
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
                    GetReceivingMessages(JsonConvert.DeserializeObject<NetworkTask>(so.sb.ToString()));
                }
            }
            catch { }
        }
    }
}
