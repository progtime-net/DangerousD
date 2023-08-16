using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore.GameObjects
{
    public abstract class Entity : GameObject
    {   
        

        public Entity(Vector2 position) : base(position) {}

        
        public void SetPosition(Vector2 position) { targetPosition = position; }

        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(Pos, targetPosition) > 0.5f)
            {
                Vector2 dir = targetPosition - Pos;
                dir.Normalize();
                _pos += dir * speed;
            }
            base.Update(gameTime);
        }
    }
}
