using DangerousD.GameCore.GameObjects.Entities.Items;
using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.Entities
{
    internal class Dmitry : Entity
    {
        private Rectangle _sourceRectangle;
        Random random = new Random();
        protected bool isUppingPrev = true;
        public Dmitry(Vector2 position) : base(position)
        {
            Width = (int)20;
            Height = (int)50;
             
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "dmitry_static" }, "dmitry_static"); 

        public override void Update(GameTime gameTime)
        { 
            base.Update(gameTime);
            delay--;
            if (delay<-1000)
            {
                delay = 10;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsComponent.DrawAnimation(Rectangle, spriteBatch, _sourceRectangle);
            base.Draw(spriteBatch);
            //spriteBatch.Draw(debugTexture, new Rectangle(Rectangle.X - GraphicsComponent.CameraPosition.X, Rectangle.Y - GraphicsComponent.CameraPosition.Y, Rectangle.Width, Rectangle.Height), Color.White);
        }
        int delay = 0;
        public override void OnCollision(GameObject gameObject)
        {
            base.OnCollision(gameObject);

            if (gameObject is Player)
            {
                Player player = (Player)gameObject;
                if (player.isUping && !player.IsRunning && delay < 0)
                    OpenDoor();

            }
        }
        public void OpenDoor()
        {
            delay += 100;
            //AppManager.Instance.GameManager.Remove(this);
            //тут спавн лута
            for (int i = 0; i < random.Next(0, 6); i++)
            {
                var d = new Diamond(Vector2.Zero);
                d.SetPosition(Pos + new Vector2(random.Next(-60, 60), Height - d.Height));
            }
            if (random.NextDouble() < 0.45)
            {
                var a = new Ammo(Vector2.Zero);
                a.SetPosition(Pos + new Vector2(random.Next(-60, 60), Height - a.Height + 4/*to look better*/));
            }
        }
    }
}
