using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Player : LivingEntity
    {
        public Player(Vector2 position) : base(position)
        {
            Width = 32;
            Height = 64;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack" }, "ZombieMoveLeft");//TODO: Change to player

        public void Kill()
        {

        }

        public void Death(string monsterName)
        {
            //анимация по имени монстра
        }
    }
}
