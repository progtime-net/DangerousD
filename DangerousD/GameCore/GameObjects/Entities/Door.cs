using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.Entities
{
    public class Door : Entity
    {
        public Door(Vector2 position) : base(position)
        {
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SilasBallMove" }, "SilasBallMove");
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
           
        }
        public override void OnCollision(GameObject gameObject)
        {
            base.OnCollision(gameObject);
            if (gameObject is Player)
            {
                Player player = (Player)gameObject;
                if (player.isUping)
                {
                    AppManager.Instance.GameManager.Remove(this);
                    //тут спавн лута
                }
            }
        }
    }
}
