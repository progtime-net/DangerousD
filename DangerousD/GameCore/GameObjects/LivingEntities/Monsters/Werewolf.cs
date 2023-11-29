using DangerousD.GameCore.GameObjects.MapObjects;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects.MapObjects;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class Werewolf : CoreEnemy
    {
        private bool isJump;
        int delay;

        public Werewolf(Vector2 position) : base(position)
        {
            name = "Werewolf";
            monster_speed = 16;
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
         

        public override void Death()
        {
            for (int i = 0; i < 9; i++)
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
                StartCicycleAnimation("WolfMoveRight"); 
                velocity.X = monster_speed;

                if (GraphicsComponent.LastUpdateCallFrame != GraphicsComponent.CurrentFrame)
                {
                    velocity.X = monster_speed;
                }
                else
                {
                    velocity.X = 0;
                }
            }
            else
            {
                StartCicycleAnimation("WolfMoveLeft");
                if (GraphicsComponent.LastUpdateCallFrame != GraphicsComponent.CurrentFrame)
                {
                    velocity.X = -monster_speed;
                }
                else
                {
                    velocity.X = 0;
                }
            }


            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y, 1, 1), typeof(CollisionMapObject));
            if (isGoRight)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y, Width + 20, Height), typeof(CollisionMapObject));
            }
            else
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 20, (int)Pos.Y, Width + 20, Height), typeof(CollisionMapObject));

            }


            foreach (var item in getCols)
            {
                if (item is StopTile)
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
            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 50, 2),typeof(CollisionMapObject));
            if (isGoRight)
            {
                getCols= AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y, Width+100, Height),typeof(Player));
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
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X-100, (int)Pos.Y, 100, Height), typeof(Player));
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
            if (gameObject is Player)
            {
                
                velocity.X = 0;
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    AppManager.Instance.GameManager.players[0].Death(name);
                }
            }
            base.OnCollision(gameObject);
        }
        public override void TakeDamage()
        {
            monster_health--;
            
            Particle particle = new Particle(Pos);
            Particle particle1 = new Particle(Pos);
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
