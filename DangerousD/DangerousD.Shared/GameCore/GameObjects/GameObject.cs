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

        public virtual void Initialize()
        {
        }

        public void PlayAnimation()
        {
            GraphicsComponent.Update();
        }
        public void LoadContent()
        {
            GraphicsComponent.LoadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            PlayAnimation();
        }

        public static Texture2D debugTexture;
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            GraphicsComponent.DrawAnimation(Rectangle, spriteBatch);
            //debug
            if (AppManager.Instance.InputManager.CollisionsCheat)
                DrawDebugRectangle(spriteBatch, Rectangle);

        } 
        public void DrawDebugRectangle(SpriteBatch spriteBatch, Rectangle _rectangle, Nullable<Color> color = null)
        {
            if (color is null) color = new Color(1, 0, 0, 0.25f);
            if (color.Value.A == 255) color = new Color(color.Value, 0.25f) ;
            spriteBatch.Draw(debugTexture,
                                 new Rectangle((_rectangle.X - GraphicsComponent.CameraPosition.X) * GraphicsComponent.scaling, 
                                 (_rectangle.Y - GraphicsComponent.CameraPosition.Y) * GraphicsComponent.scaling,
                                 _rectangle.Width * GraphicsComponent.scaling,
                                 _rectangle.Height * GraphicsComponent.scaling), color.Value);
        }
        public GraphicsComponent GetGraphicsComponent()
        {
            return this.GraphicsComponent;
        }
    }
}