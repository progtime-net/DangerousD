using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore.GameObjects
{
    abstract class Entity : GameObject
    {   
        private Vector2 targetPosition;
        public float speed;

        public Entity(Texture2D texture, Vector2 position) : base(texture, position) {}
        public Entity(Texture2D texture, Vector2 position, GraphicsComponent animator) : base(texture, position, animator) {}

        
        public void SetPosition(Vector2 position) { targetPosition = position; }

        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(Position, targetPosition) > 0.5f)
            {
                Vector2 dir = targetPosition - Position;
                dir.Normalize();
                Position += dir * speed;
            }
        }
    }
}
