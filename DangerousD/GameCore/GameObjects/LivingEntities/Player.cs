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
        }

        protected override GraphicsComponent GraphicsComponent => throw new NotImplementedException();

        public void Kill()
        {

        }

        public void Death(string monsterName)
        {
            //анимация по имени монстра
        }
    }
}
