using DangerousD.GameCore.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using DangerousD.GameCore.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore
{
    public abstract class GameObject : IDrawableObject
    {
        public Vector2 Pos { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Rectangle Rectangle => new Rectangle((int)Pos.X, (int)Pos.Y, Width, Height);
        protected GraphicsComponent Animator;
        
        public GameObject(Vector2 pos)
        {
            Pos = pos;
            Width = 500;
            Height = 100;
            Animator = new GraphicsComponent(new() { "playerIdle" });
            GameManager.Register(this);
        }

        public virtual void OnCollision()
        {
        }

        public abstract void Initialize(GraphicsDevice graphicsDevice);
        
        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Animator.DrawAnimation(Rectangle, "playerIdle", spriteBatch);
        }
    }
}
