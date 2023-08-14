using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore
{
    public abstract class GameObject
    {
        protected Texture2D Texture;
        public Vector2 Position;
        protected GraphicsComponent Animator;
        
        public GameObject(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            GameManager.Register(this);
        }

        public GameObject(Texture2D texture, Vector2 position, GraphicsComponent animator)
        {
            Texture = texture;
            Position = position;
            Animator = animator;
            GameManager.Register(this);
        }

        public virtual void OnCollision()
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
