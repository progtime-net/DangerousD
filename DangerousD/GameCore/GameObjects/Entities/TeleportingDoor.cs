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
        public TeleportingDoor(Vector2 position, Vector2 size, Rectangle sourceRectangle, Vector2 target, Action action) : base(position, size, sourceRectangle)
        {
            Target = target;
            this.action = action;
        }
        
        public TeleportingDoor(Vector2 position, Vector2 size, Rectangle sourceRectangle, Vector2 target) : base(position, size, sourceRectangle)
        {
            Target = target;
            action = () => { };
        }
        public override void OnCollision(GameObject gameObject)
        {
            if (IsVisible)
            {
                if (gameObject is Player)
                {
                    Player player = (Player)gameObject;
                    if (player.isUping)
                    {
                        IsVisible = false;
                        
                    }
                }
            }
            else
            {
                if (gameObject is Player)
                {
                    Player player = (Player)gameObject;
                    if (player.isUping)
                    {
                        player.SetPosition(Target);
                        if (action!=null)
                        {
                            action();
                        }
                        
                    }
                }
            }
        }

    }
}
