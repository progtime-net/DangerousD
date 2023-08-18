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
        bool isFall;
        public Particle(Vector2 position) : base(position)
        {
            Width = 14;
            Height = 14;
            Random random = new Random();
            velocity = new Vector2(random.Next(-6, 6), random.Next(-8,4));
            acceleration.Y = 10;
            delay = 100;
            isFall = false;
            isOnGround = false;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "GibsMoveLeftBottom", "GibsMoveLeftTop", "GibsMoveRightBottom", "GibsMoveRightTop", "GibsNotMove" }, "GibsMoveRightTop");
        public override void Update(GameTime gameTime)
        {
            
            delay--;
            if (delay<=80)
            {
                velocity.X=0;
            }
            if(isOnGround)
            {
                GraphicsComponent.StartAnimation("GibsNotMove");
                Width = 16;
                Height = 5;
                isFall=true;
                
;           }
            
            else if(!isFall)
            {
                Width = 14;
                Height= 14;
                if (velocity.Y<0 && velocity.X>0)
                {
                    GraphicsComponent.StartAnimation("GibsMoveRightTop");
                }
                else if (velocity.Y < 0 && velocity.X < 0)
                {
                    GraphicsComponent.StartAnimation("GibsMoveLeftTop");
                }
                else if (velocity.Y > 0 && velocity.X > 0)
                {
                    GraphicsComponent.StartAnimation("GibsMoveRightBottom");
                }
                else if (velocity.Y > 0 && velocity.X < 0)
                {
                    GraphicsComponent.StartAnimation("GibsMoveLeftBottom");
                }
            }
            if(delay<=0)
            {
                AppManager.Instance.GameManager.Remove(this);
            }
            base.Update(gameTime);
        }

    }
}
