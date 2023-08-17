using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class Hunchman : CoreEnemy
    {
        GameManager gameManager;
        bool isAttacking;
        public Hunchman(Vector2 position) : base(position)
        {
            Width = 48;
            Height = 48;
            monster_speed = -2;
            monster_health = 1;
            name = "HunchMan";
            velocity = new Vector2(monster_speed, 0);
            gameManager = AppManager.Instance.GameManager;
            isAttacking = false;
            isAlive = true;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> 
            { "HunchmanMoveLeft", "HunchmanMoveRight", "HunchmanAttackLeft", "HunchmanAttackRight" }, "HunchmanMoveLeft");

        public override void Update(GameTime gameTime)
        {
            gameManager = AppManager.Instance.GameManager;
           
            if (!isAttacking)
            {
                Attack();
                Move(gameTime);
                
            }
            Death();
            base.Update(gameTime);

        }

        public override void Attack()
        {
            GameObject gameObject;
            foreach (var player in gameManager.players)
            {
                if (player.Pos.Y + player.Height >= Pos.Y && player.Pos.Y <= Pos.Y + Height)
                {
                    gameObject = gameManager.physicsManager.RayCast(this, player);
                    if (gameObject is null)
                    {
                        isAttacking = true;
                        if (velocity.X > 0)
                        {
                            if (GraphicsComponent.GetCurrentAnimation != "HunchmanAttackRight")
                            {
                                GraphicsComponent.StartAnimation("HunchmanAttackRight");
                            }
                        }
                        else if (velocity.X < 0)
                        {
                            if (GraphicsComponent.GetCurrentAnimation != "HunchmanAttackLeft")
                            {
                                GraphicsComponent.StartAnimation("HunchmanAttackLeft");
                            }
                        }
                    }
                }
            }
        }

        public override void Death()
        {
            if (monster_health <= 0)
            {

            }
        }

        public override void Move(GameTime gameTime)
        {
            if (gameManager.physicsManager.RayCast(this, new Vector2(Pos.X + Width + 10, Pos.Y + Height)) is not null)
            {
                monster_speed *= -1;
            }
            
            velocity.X = monster_speed;

            if (velocity.X > 0)
            {
                if (GraphicsComponent.GetCurrentAnimation != "HunchmanMoveRight")
                {
                    GraphicsComponent.StartAnimation("HunchmanMoveRight");
                }

            }

            else if (velocity.X < 0)
            {
                if (GraphicsComponent.GetCurrentAnimation != "HunchmanMoveLeft")
                {
                    GraphicsComponent.StartAnimation("HunchmanMoveLeft");
                }
            }

        }

        public override void OnCollision(GameObject gameObject)
        { 
            monster_speed *= -1;
            _pos.X += 5 * monster_speed;
            Debug.WriteLine("Collision");
        }
    }
}
