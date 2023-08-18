using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Particle : LivingEntity
    {
        int delay;
        public Particle(Vector2 position) : base(position)
        {
            Width = 14;
            Height = 14;
            Random random = new Random();
            velocity = new Vector2(random.Next(3, 15), random.Next(3,30));
            acceleration.Y = 10;
            delay = 100;
            
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "GibsMoveLeftBottom", "GibsMoveLeftTop", "GibsMoveRightBottom", "GibsMoveRightTop" }, "GibsMoveRightTop");
        public override void Update(GameTime gameTime)
        {
            delay--;
            if (velocity.X > 0)
            {
                velocity.X--;
            }
            if(velocity.Y<=0)
            {
                GraphicsComponent.StartAnimation("GipsNoMove");
;           }
            if(delay<=0)
            {
                AppManager.Instance.GameManager.Remove(this);
            }
            base.Update(gameTime);
        }

    }
}
