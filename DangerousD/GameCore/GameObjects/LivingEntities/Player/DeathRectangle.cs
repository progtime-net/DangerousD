using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.PlayerDeath
{
    public class DeathRectangle : GameObject
    {
        public DeathRectangle(Vector2 pos, string DeathType) : base(pos)
        {
            Height = 48;
            Width = 48;
            PlayDeath(DeathType);
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> {"DeathFromZombie"},
            "DeathFromZombie");

        public GraphicsComponent Gr => GraphicsComponent;

        private void PlayDeath(string deathName)
        {
            if (GraphicsComponent.GetCurrentAnimation != "DeathFromZombie")
            {
                GraphicsComponent.StartAnimation("DeathFromZombie");
            }
        }

    }
}
