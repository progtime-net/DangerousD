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
    public class Slime : CoreEnemy
    {

        private bool isGoRight = true;
        private bool isDown = true;

        int leftBorder;
        int rightBorder;
        bool isAttaking = false;
        int delay;
        bool isJumping = false;
        public Slime(Vector2 position) : base(position)
        {
            Width = 48;
            Height = 16;
            name = "Slime";
            monster_speed = 3;
            monster_health = 2;
            leftBorder = 100;
            rightBorder = 400;
            //acceleration = Vector2.Zero;
            delay = 30;
            
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SlimeMoveLeftTop", "SlimeMoveLeftBottom", "SlimeMoveRightTop",
            "SlimeMoveRightBottom", "SlimeReadyJumpRightBottom", "SlimeReadyJumpRightTop", "SlimeReadyJumpLeftBottom", "SlimeReadyJumpLeftTop", "SlimeJumpRightBottom",
            "SlimeJumpRightTop", "SlimeJumpLeftBottom", "SlimeJumpLeftTop" }, "SlimeMoveRightTop");

        public override void Attack()
        {
            
        }
        public void Jump(GameTime gameTime)
        {
            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle(0, 0, 100, 100));
            velocity.X = 0;
            Height = 32;

            if (isGoRight && isDown)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeReadyJumpLeftBottom")
                {
                    GraphicsComponent.StartAnimation("SlimeReadyJumpLeftBottom");
                }
                delay--;
                if (delay <= 0)
                {
                    
                    velocity = new Vector2(5, -4);
                    acceleration.Y = 0;
                    
                    if (GraphicsComponent.GetCurrentAnimation != "SlimeJumpLeftBottom")
                    {
                        GraphicsComponent.StartAnimation("SlimeJumpLeftBottom");
                    }
                    getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y - 5, 48, 5));
                    if (getCols.Count > 0 )
                    {
                        isJumping = false;
                        isDown = false;
                        isAttaking = false;
                    }
                }

            }
            else if (!isGoRight && isDown)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeReadyJumpRightBottom")
                {
                    GraphicsComponent.StartAnimation("SlimeReadyJumpRightBottom");
                }
                delay--;
                if (delay <= 0)
                {
                    
                    velocity = new Vector2(-5, -4);
                    acceleration.Y = 0;
                    if (GraphicsComponent.GetCurrentAnimation != "SlimeJumpRightBottom")
                    {
                        GraphicsComponent.StartAnimation("SlimeJumpRightBottom");
                    }
                    getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y - 5, 48, 5));
                    if (getCols.Count > 0)
                    {
                        isJumping = false;
                        isDown = false;
                        isAttaking = false;
                    }
                }
            }
            else if (isGoRight && !isDown)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeReadyJumpLeftTop")
                {
                    GraphicsComponent.StartAnimation("SlimeReadyJumpLeftTop");

                }
                delay--;
                if (delay <= 0)
                {
                   
                    velocity = new Vector2(5, 4);
                    acceleration.Y = 0;
                    
                    if (GraphicsComponent.GetCurrentAnimation != "SlimeJumpLeftTop")
                    {
                        GraphicsComponent.StartAnimation("SlimeJumpLeftTop");
                    }
                    getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X+1, (int)Pos.Y + Height, 46, 5));
                    
                    if (getCols.Count > 0 )
                    {
                        isJumping = false;
                        isDown = true;
                        isAttaking = false;
                        acceleration.Y = 10;
                        Move(gameTime);
                    }
                }

            }
            else if (!isGoRight && !isDown)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeReadyJumpRightTop")
                {
                    GraphicsComponent.StartAnimation("SlimeReadyJumpRightTop");
                }
                delay--;
                if (delay <= 0)
                {
                    velocity = new Vector2(-5, 4);
                    acceleration.Y = 0;
                    
                    if (GraphicsComponent.GetCurrentAnimation != "SlimeJumpRightTop")
                    {
                        GraphicsComponent.StartAnimation("SlimeJumpRightTop");
                    }
                    getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X+1, (int)Pos.Y + Height, 46, 5));
                    if (getCols.Count > 0 )
                    {
                        isJumping = false;
                        isDown = true;
                        isAttaking = false;
                        acceleration.Y = 10;
                        Move(gameTime);
                        
                    }
                }


            }
            

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            
            
            base.Draw(spriteBatch);
        }
        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            delay = 30;
            Height = 16;
            if (isGoRight && isDown)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeMoveRightBottom")
                {
                    GraphicsComponent.StartAnimation("SlimeMoveRightBottom");
                }
                velocity.X = monster_speed;

            }

            else if (!isGoRight && isDown)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeMoveLeftBottom")
                {
                    GraphicsComponent.StartAnimation("SlimeMoveLeftBottom");
                }
                velocity.X = -monster_speed;

            }
            else if (!isDown && isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeMoveRightTop")
                {
                    GraphicsComponent.StartAnimation("SlimeMoveRightTop");

                }
                velocity.X = monster_speed;


            }
            else if (!isDown && !isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeMoveLeftTop")
                {
                    GraphicsComponent.StartAnimation("SlimeMoveLeftTop");
                }
                velocity.X = -monster_speed;

            }
            var getCols= AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 50, 2)); ;
            if (isGoRight)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 51, 2));
            }
            else
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X-3, (int)Pos.Y + Height / 2 - 2, 51, 2));
            }
            
            
            foreach(var item in getCols)
            {
               if(item is MapObject)
               {
                   isGoRight = !isGoRight;
                   break;
               }
            }
        }
        public override void Update(GameTime gameTime)
        {

            if (isDown)
            {
                
                if (acceleration.Y < 0)
                {
                    acceleration.Y = -acceleration.Y;
                }
            }
            else
            {
                
                if (acceleration.Y > 0)
                {
                    acceleration.Y = -acceleration.Y;
                }
            }
            AppManager.Instance.DebugHUD.Set(name, isAttaking.ToString());
            if(!isJumping)
            {
                if (isDown)
                {
                    Jump(gameTime);
                }
                else if(IsInAim())
                {
                    Jump(gameTime);
                    isAttaking = true;
                }
                else if(!isAttaking)
                {
                    Move(gameTime);
                    
                }
                else { Jump(gameTime); }
            }
            
            

            base.Update(gameTime);
        }

        public override void Target()
        {

        }
        public bool IsInAim()
        {
            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height, 48, 5));
            

            if (isGoRight && !isDown)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X + Width, (int)Pos.Y + Height, 200, 500), false);
                if (getCols.Count > 0)
                {
                    
                    return true;
                }
            }
            else if (!isGoRight && !isDown)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 200, (int)Pos.Y + Height, 200, 500), false);
                if (getCols.Count > 0)
                {
                    
                    return true;
                }
            }
            /*/else if (isGoRight && isDown)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X +Width, (int)Pos.Y -500, 200, 500), false);
                if (getCols.Count > 0)
                {
                    isAttaking = true;
                    return true;
                }
            }
            else if (!isGoRight && isDown)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 200, (int)Pos.Y - 500, 200, 500), false);
                if (getCols.Count > 0)
                {
                    isAttaking = true;
                    return true;
                }
            }/*/

            return false;
            
        }
        public override void Attack(GameTime gameTime)
        {

        }
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    AppManager.Instance.GameManager.players[0].Death(name);
                }
            }
            base.OnCollision(gameObject);
        }
    }
}
