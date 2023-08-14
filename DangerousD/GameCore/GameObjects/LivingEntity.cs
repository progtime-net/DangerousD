using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore.GameObjects
{
    abstract class LivingEntity : Entity
    {
        public LivingEntity(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }

        public LivingEntity(Texture2D texture, Vector2 position, GraphicsComponent animator) : base(texture, position, animator)
        {
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
