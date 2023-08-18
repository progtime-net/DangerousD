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
    public class HunchmanDagger : CoreEnemy
    {
        public HunchmanDagger(Vector2 position) : base(position)
        {
            name = "Hunchman";
            monster_speed = 1;
            Width = 9;
            Height = 6;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new (new List<string> { "HunchmanDaggerRight", "HunchmanDaggerLeft" }, "HunchmanDaggerLeft");

        public override void Attack()
        {

        }

        public void Attack(GameTime gameTime)
        {

        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {

        }

        public void Target()
        {
            throw new NotImplementedException();
        }
    }
}
