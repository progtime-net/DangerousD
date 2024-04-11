using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.Network
{
    public class StateObject
    {
        public Socket workSocket;
        public int bufferSize;
        public byte[] buffer;
        public int UploadedBytesCount;

        public StateObject(Socket socket, byte[] buffer)
        {
            workSocket = socket;
            this.buffer = buffer;
            bufferSize = buffer.Length;
            UploadedBytesCount = 0;
        }
    }
}
