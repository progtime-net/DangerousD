﻿using System.Net.Sockets;
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
        public delegate void ReceivingHandler(List<NetworkTask> networkTask);

        public event ReceivingHandler GetReceivingMessages;

        Socket socket;
        IPEndPoint endPoint;
        List<Socket> clientSockets = new List<Socket>();
        string state;

        private void Init(string IpAddress)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse(IpAddress);
            int port = 51873;
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

            }
        }
        private void BeginHostReceive(object clSocket)
        {
            Socket clientSocket = clSocket as Socket;
            while (clientSocket != null)
            {
                byte[] bytesCount = new byte[4];
                clientSocket.Receive(bytesCount);
                int length = BitConverter.ToInt32(bytesCount);
                byte[] Data = new byte[length];
                StateObject so = new StateObject(clientSocket, Data);
                while (so.UploadedBytesCount < length)
                {
                    int count = clientSocket.Receive(so.buffer, so.UploadedBytesCount, length - so.UploadedBytesCount, SocketFlags.None);
                    so.UploadedBytesCount += count;
                }
                List<NetworkTask> tasks = JsonConvert.DeserializeObject<List<NetworkTask>>(Encoding.Unicode.GetString(so.buffer, 0, length));
                GetReceivingMessages(tasks);
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
            AppManager.Instance.SetMultiplayerState(MultiPlayerStatus.Host);
        }
        public void ClientInit(string IpAddress)
        {
            Init(IpAddress);
            socket.Connect(endPoint);
            state = "Client";
            Thread.Sleep(10);
            Thread ReceivingThread = new Thread(ReceiveMsgFromHost);
            ReceivingThread.Start();
            NetworkTask connectionTask = new NetworkTask("Player");
            AppManager.Instance.NetworkTasks.Add(connectionTask);
            AppManager.Instance.SetMultiplayerState(MultiPlayerStatus.Client);
        }
        public void SendMsg(List<NetworkTask> networkTasks, Socket ignoreSocket = null)
        {
            Thread optThread = new Thread(SendMsgOpt);
            optThread.Start(networkTasks);

        }
        public void SendMsgOpt(object objList)
        {
            List<NetworkTask> networkTasks = (List<NetworkTask>)objList;
            byte[] Data = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(networkTasks));
            int count = Data.Length;
            if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host)
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
        private void ReceiveMsgFromHost()
        {
            while (true)
            {
                byte[] bytesCount = new byte[4];
                socket.Receive(bytesCount);
                int length = BitConverter.ToInt32(bytesCount);
                byte[] Data = new byte[length];
                StateObject so = new StateObject(socket, Data);
                while (so.UploadedBytesCount < length)
                {
                    int count = socket.Receive(so.buffer, so.UploadedBytesCount, length-so.UploadedBytesCount, SocketFlags.None);
                    so.UploadedBytesCount += count;
                }
                List<NetworkTask> tasks = JsonConvert.DeserializeObject<List<NetworkTask>>(Encoding.Unicode.GetString(so.buffer, 0, length));
                GetReceivingMessages(tasks);
            }
        }
    }
}
