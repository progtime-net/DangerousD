using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects.MapObjects;

namespace DangerousD.GameCore.GameObjects.Entities
{
    public class SilasBall : LivingEntity
    {
        
        private Vector2 v;
        public string name;
        public SilasBall(Vector2 position) : base(position)
        {
            Height = 24;
            Width = 24;
            acceleration = Vector2.Zero;
            name = "SilasHand";


        }
        public SilasBall(Vector2 position, Vector2 velosity) : base(position)
        {
            Height = 24;
            Width = 24;
            acceleration = Vector2.Zero;
            velocity = velosity;
            v = velosity;
            name = "SilasHand";

        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SilasBallMove" }, "SilasBallMove");
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (AppManager.Instance.GameManager.physicsManager.CheckRectangle( new Rectangle(Rectangle.X-2,Rectangle.Y-2,Rectangle.Width+8,Rectangle.Height+8),typeof(CollisionMapObject)).Count>0)
            {
                AppManager.Instance.GameManager.Remove(this);
            }
            velocity = v;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            
            base.Draw(spriteBatch);
            
            
        }
        public void Attack()
        {
            AppManager.Instance.GameManager.GetPlayer1.Death(name);
        }
        public override void OnCollision(GameObject gameObject)
        {
            base.OnCollision(gameObject);
            if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    Attack();

                }
            }
        }
    }
}
