using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.Network
{
    [Serializable]
    public enum NetworkTaskOperationEnum
    {
        DeleteObject, SendSound, CreateEntity, SendPosition, ChangeState, ConnectToHost, GetClientPlayerId, AddConnectedPlayer, KillPlayer
    }
}
