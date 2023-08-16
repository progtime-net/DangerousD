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
        public int objId { get; set; }
        public Vector2 position { get; set; }
        public Vector2 velocity { get; set; }
        public Type type { get; set; }


        /// <summary>
        /// Нанести урон сущности
        /// </summary>
        /// <param name="LivingEntityId"></param>
        /// <param name="Damage"></param>
        public NetworkTask(int LivingEntityId, int Damage)
        {
            operation = NetworkTaskOperationEnum.TakeDamage;
            objId = LivingEntityId;
            value = Damage;
        }

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
        /// <param name="EntityVelocity"></param>
        /// <param name="ParentId"></param>
        public NetworkTask(Type EntityType, Vector2 EntityPosition, Vector2 EntityVelocity, int ParentId)
        {
            operation = NetworkTaskOperationEnum.CreateEntity;
            type = EntityType;
            position = EntityPosition;
            velocity = EntityVelocity;
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
    }
}
