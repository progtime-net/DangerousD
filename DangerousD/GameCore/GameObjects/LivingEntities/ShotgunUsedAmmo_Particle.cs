using DangerousD.GameCore.Graphics;
using DangerousD.GameCore.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DangerousD.GameCore.GameObjects.LivingEntities.Player;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class ShotgunUsedAmmo_Particle : LivingEntity
    {
        int delay; 
        public ShotgunUsedAmmo_Particle(Vector2 position, Vector2 shotDirection, Vector2 playerVelocity) : base(position)
        {
            Width = 4;
            Height = 2;
            Random random = new Random();
            velocity =  - shotDirection * (3+(float)random.NextDouble()) + new Vector2(0, -1 - random.Next(0,5)) + playerVelocity;//oposite direction of the shot+ upwards force + relativity to player
            acceleration.Y = 20;
            delay = 100;
            isOnGround = false;

            if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host)
            {
                //NetworkTask task = new NetworkTask(typeof(Particle), Pos, id, velocity); //recheck
                //AppManager.Instance.NetworkTasks.Add(task);
            }
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "UsedAmmo" }, "UsedAmmo");
        public override void Update(GameTime gameTime)
        {

            delay--;




            if (isOnGround)
            {
                velocity.X *= 0.8f; //decrease speed when touching ground
                GraphicsComponent.StartAnimation("UsedAmmo");
                velocity.Y = -Math.Abs(velocity.X * 1); //bouncing effect
            }




            if (velocity.X > 0) //TODO - left and right variations
            {
                GraphicsComponent.StartAnimation("UsedAmmo");
            }
            else
                GraphicsComponent.StartAnimation("UsedAmmo");





            if (delay <= 0)
            {
                //AppManager.Instance.GameManager.Remove(this); //auto self delete
            }
            base.Update(gameTime);
        }

    }
}
