using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class SpiderWeb : CoreEnemy
    {
        public SpiderWeb(Vector2 position) : base(position)
        {
            name = "Web";
            Width = 16;
            Height = 0;
            acceleration = Vector2.Zero;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SpiderWeb" }, "SpiderWeb");

        public override void Attack()
        {

        }

        public override void Attack(GameTime gameTime)
        {

        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {

        }

        public override void Target()
        {

        }
    }
}
