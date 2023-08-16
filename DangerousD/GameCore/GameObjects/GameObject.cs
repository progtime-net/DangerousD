﻿using DangerousD.GameCore.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using DangerousD.GameCore.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using DangerousD.GameCore.GameObjects.LivingEntities;

namespace DangerousD.GameCore
{
    public abstract class GameObject : IDrawableObject
    {
        protected Vector2 _pos;
        public Vector2 Pos => _pos;
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Rectangle Rectangle => new Rectangle((int)Pos.X, (int)Pos.Y, Width, Height);
        public Vector2 velocity;
        public Vector2 acceleration;
        protected abstract GraphicsComponent GraphicsComponent { get; }
        public GameObject(Vector2 pos)
        {
            _pos = pos;
            Width = 500;
            Height = 101;
            //Animator = new GraphicsComponent(new() { "playerIdle" });
            LoadContent();
            AppManager.Instance.GameManager.Register(this);
        }

        public virtual void OnCollision()
        {
        }

        public virtual void Initialize(GraphicsDevice graphicsDevice)
        {
        }

        public void LoadContent()
        {
            GraphicsComponent.LoadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            GraphicsComponent.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            GraphicsComponent.DrawAnimation(Rectangle, spriteBatch);
        }
    }
}