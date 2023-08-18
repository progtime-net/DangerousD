using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using DangerousD.GameCore.GameObjects.Entities.Items;

namespace DangerousD.GameCore.GameObjects.Entities
{
    public class Door : Entity
    {
        private Rectangle _sourceRectangle;
        Random random = new Random();
        public Door(Vector2 position, Vector2 size, Rectangle sourceRectangle) : base(position)
        {
            _sourceRectangle = sourceRectangle;
            Width = (int)size.X;
            Height = (int)size.Y;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new("doors");

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsComponent.DrawAnimation(Rectangle, spriteBatch, _sourceRectangle);
            //spriteBatch.Draw(debugTexture, new Rectangle(Rectangle.X - GraphicsComponent.CameraPosition.X, Rectangle.Y - GraphicsComponent.CameraPosition.Y, Rectangle.Width, Rectangle.Height), Color.White);
        }
        public override void OnCollision(GameObject gameObject)
        {
            base.OnCollision(gameObject);
            if (this is not TeleportingDoor)
            {
                if (gameObject is Player)
                {
                    Player player = (Player)gameObject;
                    if (player.isUping)
                    {
                        AppManager.Instance.GameManager.Remove(this);
                        //тут спавн лута
                        for (int i = 0; i < random.Next(0,15); i++)
                        {
                            var d = new Diamond(Vector2.Zero);
                            d.SetPosition(Pos + new Vector2(random.Next(-30, 30), Height - d.Height));
                        }
                    }
                }
            }
            
        }
    }
}
