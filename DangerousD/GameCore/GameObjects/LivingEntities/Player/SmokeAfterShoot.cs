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
    public class SmokeAfterShoot : GameObject
    {
        public SmokeAfterShoot(Vector2 pos) : base(pos)
        {
            Height = 6;
            Width = 6;
            PlaySmoke();
            this.GraphicsComponent.actionOfAnimationEnd += (a) =>
            {
                if (a == "smokeAfterShoot")
                {
                    AppManager.Instance.GameManager.Remove(this);
                }
            };
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "smokeAfterShoot" },
            "smokeAfterShoot");

        public GraphicsComponent Gr => GraphicsComponent;

        private void PlaySmoke()
        {
            if (GraphicsComponent.GetCurrentAnimation != "smokeAfterShoot")
            {
                GraphicsComponent.StartAnimation("smokeAfterShoot");
            }
        }

    }
}