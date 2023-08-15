using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class Zombie : CoreEnemy
    {
        public Zombie(Vector2 position) : base(position)
        {
            Width = 24;
            Height = 40;
            GraphicsComponent.StartAnimation("ZombieMoveRight");
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight"}, "ZombieMoveRight");

        public override void Update(GameTime gameTime)
        {
            Move();
            if (monster_health <= 0)
            {
                Death();
            }
            base.Update(gameTime);
        }

        public override void Attack()
        {

        }

        public override void Death()
        {

        }

        public override void Move()
        {
        }
    }
}
