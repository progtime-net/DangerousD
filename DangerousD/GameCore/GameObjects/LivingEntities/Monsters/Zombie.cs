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
        int leftBorder;
        int rightBorder;
        public Zombie(Vector2 position) : base(position)
        {
            Width = 72;
            Height = 120;
            monster_speed = 20;
            GraphicsComponent.StartAnimation("ZombieLeftAttack");
            name = "Zombie";
            leftBorder = (int)position.X;
            rightBorder = (int)position.X + 200;
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack" }, "ZombieMoveLeft");

        public override void Update(GameTime gameTime, Player player)
        {
            Move(gameTime);

            if(Pos.X + 20 <= player.Pos.X || Pos.X - 20 >= player.Pos.X)
            {
                Attack();
                player.Death(name);
            }

            base.Update(gameTime);
        }

        public override void Attack()
        {
            if (isGoRight)
            {
                GraphicsComponent.StopAnimation();
                GraphicsComponent.StartAnimation("ZombieRightAttack");
                AppManager.Instance.GameManager.Player.Death(name);
            }
            else if (!isGoRight)
            {
                GraphicsComponent.StopAnimation();
                GraphicsComponent.StartAnimation("ZombieLeftAttack");
                AppManager.Instance.GameManager.Player.Death(name);
            }
        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds;
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

        public void TakeDamage(int damage)
        {
            monster_health -= damage;
            //play take damage animation
        }
    }
}
