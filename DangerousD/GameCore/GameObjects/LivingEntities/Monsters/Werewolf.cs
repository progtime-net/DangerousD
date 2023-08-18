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
    public class Werewolf : CoreEnemy
    {
        private bool isJump;
        int delay;

        public Werewolf(Vector2 position) : base(position)
        {
            name = "Wolf";
            monster_speed = 3;
            Width = 39;
            Height = 48;
            delay = 10;
            monster_health = 3;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "WolfMoveRight", "WolfMoveLeft", "WolfJumpRight", "WolfJumpLeft" }, "WolfMoveRight");

        public override void Update(GameTime gameTime)
        {
            if(!isJump )
            {
                Jump();
            }
            if(isOnGround)
            {
                Move(gameTime);
                
            }
            

            base.Update(gameTime);
        }

        public override void Attack()
        {

        }

        public override void Death()
        {
            for (int i = 0; i < 5; i++)
            {
                Particle particle = new Particle(Pos);
            }

            AppManager.Instance.GameManager.Remove(this);
        }

        public override void Move(GameTime gameTime)
        {
            isJump = false;
            
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "WolfMoveRight")
                {
                    GraphicsComponent.StartAnimation("WolfMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (GraphicsComponent.GetCurrentAnimation != "WolfMoveLeft")
                {
                    GraphicsComponent.StartAnimation("WolfMoveLeft");
                }
                velocity.X = -monster_speed;
            }
            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 50, 2));
            if (isGoRight)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, Width+4, 2));
            }
            else
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 3, (int)Pos.Y + Height / 2 - 2, Width +3, 2));
            }


            foreach (var item in getCols)
            {
                if (item is MapObject)
                {
                    isGoRight = !isGoRight;
                    break;
                }
            }
        }

        public override void Attack(GameTime gameTime)
        {
        }

        public void Jump()
        {
            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 50, 2));
            if (isGoRight)
            {
                getCols= AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y, Width+100, Height),false);
                if(getCols.Count > 0)
                {
                    isJump = true;
                    if (GraphicsComponent.GetCurrentAnimation != "WolfJumpRight")
                    {
                        GraphicsComponent.StartAnimation("WolfJumpRight");
                    }
                    velocity.Y = -7;
                    velocity.X = 6;
                }
                
            }
            else
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X-100, (int)Pos.Y, 100, Height), false);
                if (getCols.Count > 0)
                {
                    isJump = true;
                    if (GraphicsComponent.GetCurrentAnimation != "WolfJumpLeft")
                    {
                        GraphicsComponent.StartAnimation("WolfJumpLeft");
                    }
                    velocity.Y = -7;
                    velocity.X = -6;
                }
                
            }
            
        }
        public override void OnCollision(GameObject gameObject)
        {
            /*/if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    AppManager.Instance.GameManager.players[0].Death(name);
                }
            }
            base.OnCollision(gameObject);/*/
        }
        public void TakeDamage()
        {
            monster_health--;
            
            Particle particle = new Particle(Pos);
            if (monster_health <= 0)
            {
                Death();
            }
        }

        public override void Target()
        {
        }
    }
}
