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
    public class Zombie : CoreEnemy
    {
        private bool isGoRight = true;
        public Zombie(Vector2 position) : base(position)
        {
            Width = 72;
            Height = 120;
            GraphicsComponent.StartAnimation("ZombieRightAttack");
            monster_speed = 100;
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack"}, "ZombieMoveRight");

        public override void Update(GameTime gameTime)
        {
            //Move(gameTime);
            if (monster_health <= 0)
            {
                isAlive = false;
                Death();
            }
            base.Update(gameTime);
        }

        public override void Attack()
        {
            if (isGoRight)
            {

            }
        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveRight")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveRight");
                    velocity = new Vector2(monster_speed, 0);
                }
            }

            else if (!isGoRight)
            {
                if(GraphicsComponent.GetCurrentAnimation != "ZombieMoveLeft")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveLeft");
                    velocity = new Vector2(-monster_speed, 0);
                }
            }
        }
    }
}
