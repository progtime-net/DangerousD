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
        private bool isGoRight = false;

        public HunchmanDagger(Vector2 position) : base(position)
        {
            name = "Hunchman";
            monster_speed = 4;
            Width = 9;
            Height = 6;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new (new List<string> { "HunchmanDaggerRight", "HunchmanDaggerLeft" }, "HunchmanDaggerLeft");

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
            velocity.X = 0;
            var animation = GraphicsComponent.GetCurrentAnimation;

            if (animation == "HunchmanDaggerRight")
            {
                velocity.X = monster_speed;
            }
            else if (animation == "HunchmanDaggerLeft")
            {
                velocity.X = -monster_speed;
            }
        }

        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                AppManager.Instance.GameManager.players[0].Death(name);
            }
            else
            {
                Death();
            }

            base.OnCollision(gameObject);
        }

        public void Target()
        {
            throw new NotImplementedException();
        }
    }
}
