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
    public class SilasMaster : CoreEnemy
    {
        private int attackTime = 60;
        private int moveTime = 360;
        private int currentTime = 0;
        public SilasMaster(Vector2 position) : base(position)
        {
            name = "SilasMaster";
            Width = 144;
            Height = 160;
            monster_health = 15;
            monster_speed = 4;
            acceleration = Vector2.Zero;
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new GraphicsComponent(new List<string>() { "SilasMove", "SilasAttack" }, "SilasMove");
        public override void Attack()
        {
            if (currentTime==0)
            {
                GraphicsComponent.StartAnimation("SilasAttack");
            }
            else if (currentTime >= attackTime)
            {
                GraphicsComponent.StartAnimation("SilasMove");
                currentTime = 0;
            }
            currentTime++;
        }

        public override void Death()
        {
            throw new NotImplementedException();
        }

        public override void Move(GameTime gameTime)
        {
            if (currentTime == 0)
            {
                GraphicsComponent.StartAnimation("SilasMove");
            }
            else if (currentTime >= moveTime)
            {
                GraphicsComponent.StartAnimation("SilasAttack");
                currentTime = 0;
            }
            currentTime++;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (GraphicsComponent.CurrentAnimation.Id=="SilasMove")
            {
                Move(gameTime);
            }
            else
            {
                Attack();
            }
        }
    }
}
