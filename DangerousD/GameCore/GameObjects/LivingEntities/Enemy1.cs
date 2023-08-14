using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Enemy1 : LivingEntity
    {
        public Enemy1(Vector2 position) : base(position)
        {
        }

        public override void Initialize(GraphicsDevice graphicsDevice)
        {
            graphicsComponent = new GraphicsComponent(new List<string>() { "IDLE", "WALK" }, "IDLE");
        }
         

        public override void Update(GameTime gameTime)
        {
            if (graphicsComponent.GetCurrentAnimation!="WALK")
                graphicsComponent.StartAnimation("WALK");

            base.Update(gameTime);
        }
    }
}
