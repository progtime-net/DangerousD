using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.Network
{
    [Serializable]
    public class NetworkTask
    {
        public NetworkTaskOperationEnum operation { get; set; }
        public string name { get; set; }
        public int value { get; set; }
        public bool isParam { get; set; }    
        public int objId { get; set; }
        public Vector2 position { get; set; }
        public Vector2 velocity { get; set; }
        public Type type { get; set; }

        public NetworkTask() { }
        /// <summary>
        /// Проиграть звук на позиции
        /// </summary>
        /// <param name="SoundPosition"></param>
        /// <param name="SoundName"></param>
        public NetworkTask(Vector2 SoundPosition, string SoundName)
        {
            operation = NetworkTaskOperationEnum.SendSound;
            position = SoundPosition;
            name = SoundName;
        }

        /// <summary>
        /// Создать сущность на позиции с заданной скоростью и присвоить её родительской сущности
        /// </summary>
        /// <param name="EntityType"></param>
        /// <param name="EntityPosition"></param>
        /// <param name="ParentId"></param>
        public NetworkTask(Type EntityType, Vector2 EntityPosition, int ParentId, Vector2 velocity)
        {
            operation = NetworkTaskOperationEnum.CreateEntity;
            type = EntityType;
            position = EntityPosition;
            this.velocity = velocity;
            objId = ParentId;
        }

        /// <summary>
        /// Изменить позицию сущности со сложной логикой(игрок)
        /// </summary>
        /// <param name="EntityId"></param>
        /// <param name="EntityPosition"></param>
        public NetworkTask(int EntityId, Vector2 EntityPosition)
        {
            operation = NetworkTaskOperationEnum.SendPosition;
            objId = EntityId;
            position = EntityPosition;
        }
        /// <summary>
        /// Изменяет состояние и/или скорость сущности
        /// </summary>
        /// <param name="EntityId"></param>
        /// <param name="StateName"></param>
        /// <param name="EntityVelocity"></param>
        public NetworkTask(int EntityId, string StateName, Vector2 EntityVelocity)
        {
            operation = NetworkTaskOperationEnum.ChangeState;
            objId = EntityId;
            name = StateName;
            velocity = EntityVelocity;
        }

        /// <summary>
        /// Подключается к хосту и передаёт своё имя    
        /// </summary>
        /// <param name="PlayerName"></param>
        public NetworkTask(string PlayerName)
        {
            operation = NetworkTaskOperationEnum.ConnectToHost;
            name = PlayerName;
        }

        /// <summary>
        /// Получает id игрока на клиенте от хоста
        /// </summary>
        /// <param name="PlayerId"></param>
        public NetworkTask(int PlayerId)
        {
            operation = NetworkTaskOperationEnum.GetClientPlayerId;
            objId = PlayerId;
        }

        /// <summary>
        /// Универсальный конструктор для нестандартных операций. То, что не нужно(кроме операции) делать null. 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isParam"></param>
        /// <param name="objId"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        public NetworkTask(NetworkTaskOperationEnum operation, string name, int value, bool isParam, int objId, Vector2 position, Vector2 velocity, Type type)
        {
            this.operation = operation;
            this.name = name;
            this.value = value;
            this.isParam = isParam;
            this.objId = objId;
            this.position = position;
            this.velocity = velocity;
            this.type = type;
        }
        public NetworkTask AddConnectedPlayer(int connectedPlayerId, Vector2 playerPosition)
        {
            operation = NetworkTaskOperationEnum.AddConnectedPlayer;
            objId = connectedPlayerId;
            position = playerPosition;
            return this;
        }
        public NetworkTask DeleteObject(int objectId)
        {
            operation = NetworkTaskOperationEnum.DeleteObject;
            objId = objectId;
            return this;
        }
        public NetworkTask KillPlayer(int playerId, string mosterName)
        {
            operation = NetworkTaskOperationEnum.KillPlayer;
            name = mosterName;
            objId = playerId;
            return this;
        }
    }
}
