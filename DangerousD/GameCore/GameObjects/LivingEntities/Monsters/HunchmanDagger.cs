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

        public HunchmanDagger(Vector2 position, bool isGoRight) : base(position)
        {
            this.isGoRight = isGoRight;
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
            AppManager.Instance.GameManager.Remove(this);
        }

        public override void Move(GameTime gameTime)
        {
            velocity.X = 0;
            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle(0, 0, 0, 0));
            if (isGoRight)
            {
                StartCicycleAnimation("HunchmanDaggerRight");
                velocity.X = monster_speed;
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y, Width+5, Height));
                if(getCols.Count>0)
                {
                    Death();
                }
            }

            else if (!isGoRight)
            {
                StartCicycleAnimation("HunchmanDaggerLeft");
                velocity.X = -monster_speed;
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X-5, (int)Pos.Y, Width + 5, Height));
                if (getCols.Count > 0)
                {
                    Death();
                }
            }
            

        }

        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                AppManager.Instance.GameManager.players[0].Death(name);
            }

            base.OnCollision(gameObject);
        }

        public override void Target()
        {

        }
    }
}
