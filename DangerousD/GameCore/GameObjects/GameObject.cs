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
        protected GraphicsComponent graphicsComponent;

        public GameObject(Vector2 pos)
        {
            Pos = pos;
            Width = 128;
            Height = 128;
            //Animator = new GraphicsComponent(new() { "playerIdle" });
            GameManager.Register(this);
        }

        public virtual void OnCollision()
        {
        }

        public abstract void Initialize(GraphicsDevice graphicsDevice);

        public virtual void LoadContent(ContentManager content)
        {
            graphicsComponent.LoadContent();
        }

        public virtual void Update(GameTime gameTime) { graphicsComponent.Update(); }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            graphicsComponent.DrawAnimation(Rectangle, spriteBatch);
        }
    }
}
