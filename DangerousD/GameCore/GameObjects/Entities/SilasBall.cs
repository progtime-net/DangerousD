using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.Entities
{
    public class SilasBall : LivingEntity
    {
        
        private Vector2 v;
        public SilasBall(Vector2 position) : base(position)
        {
            Height = 24;
            Width = 24;
            acceleration = Vector2.Zero;
            
        }
        public SilasBall(Vector2 position, Vector2 velosity) : base(position)
        {
            Height = 24;
            Width = 24;
            acceleration = Vector2.Zero;
            velocity = velosity;
            v = velosity;
            
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SilasBallMove" }, "SilasBallMove");
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (AppManager.Instance.GameManager.physicsManager.CheckRectangle( new Rectangle(Rectangle.X-2,Rectangle.Y-2,Rectangle.Width+8,Rectangle.Height+8)).Count>0)
            {
                AppManager.Instance.GameManager.Remove(this);
            }
            velocity = v;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            
            base.Draw(spriteBatch);
            
            
        }
    }
}
