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
    internal class Trigger : Entity
    {
        public Action<GameObject> OnCollisionAction;
        string trigger_Name;
        public Trigger(Rectangle rectangle, string trigger_Name) : base(new Vector2(rectangle.X, rectangle.Y))
        {
            _pos = new Vector2(rectangle.X, rectangle.Y);
            Width = rectangle.Width;
            Height = rectangle.Height;
            this.trigger_Name = trigger_Name;
        }

        protected override GraphicsComponent GraphicsComponent => new GraphicsComponent(new List<string>() { "SilasBallMove" }, "SilasBallMove");
        public override void OnCollision(GameObject gameObject)
        {
            OnCollisionAction?.Invoke(gameObject);
        }
        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
