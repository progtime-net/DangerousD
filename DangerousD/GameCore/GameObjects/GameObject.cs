using DangerousD.GameCore.Graphics;
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
        public int id;
        public bool isChildEntity = false;
        public bool isIdFromHost = false;
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Rectangle => new Rectangle((int)Pos.X, (int)Pos.Y, Width, Height);
        protected abstract GraphicsComponent GraphicsComponent { get; }
        public GameObject(Vector2 pos)
        {
            Initialize();
            _pos = pos;
            Width = 500;
            Height = 101;
            //Animator = new GraphicsComponent(new() { "playerIdle" });
            
            LoadContent();
            AppManager.Instance.GameManager.Register(this);
            GraphicsComponent.parentId = id;
        }

        public virtual void OnCollision(GameObject gameObject)
        {
        }

        protected int obsticles = 0;
        public virtual void OnCollisionWithObsticle(GameObject gameObject)
        {
            obsticles++;//SD
        }

        public virtual void Initialize()
        {
        }

        public void PlayAnimation()
        {
            GraphicsComponent.LoadContent();
        }
        public void LoadContent()
        {
            PlayAnimation();
        }

        public virtual void Update(GameTime gameTime)
        {
            obsticles = 0;
            GraphicsComponent.Update();
        }

        public static Texture2D debugTexture;
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            GraphicsComponent.DrawAnimation(Rectangle, spriteBatch);
            //debug
            if (AppManager.Instance.InputManager.CollisionsCheat)
            {
                spriteBatch.Draw(debugTexture,
                    new Rectangle(Rectangle.X - GraphicsComponent.CameraPosition.X,
                        Rectangle.Y - GraphicsComponent.CameraPosition.Y, Rectangle.Width, Rectangle.Height),
                    Color.White);
            }

        }
        public GraphicsComponent GetGraphicsComponent()
        {
            return this.GraphicsComponent;
        }
    }
}