using DangerousD.GameCore.GameObjects.LivingEntities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.Entities
{
    internal class TeleportingDoor : Door
    {
        public Vector2 Target;
        public bool IsVisible = true;
        public Action action;
        public TeleportingDoor(Vector2 position, Vector2 size, Rectangle sourceRectangle, Action action) : base(position, size, sourceRectangle)
        {
            this.action = action;
        }
        
        public TeleportingDoor(Vector2 position, Vector2 size, Rectangle sourceRectangle, Vector2 target) : base(position, size, sourceRectangle)
        {
            Target = target;
            
        }
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                Player player = (Player)gameObject;
                if (player.isUping && !isUppingPrev)
                {
                    if (action!=null)
                    {
                        action();
                    }
                    else
                    {
                        player.SetPosition(new Vector2(Target.X, Target.Y - player.Height - 5));
                    }
                }
            }
            base.OnCollision(gameObject);
        }

    }
}
