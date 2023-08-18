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
        private bool isDown = false;
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
        public void Jump()
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
                    isJumping = true;
                    velocity = new Vector2(5, -3);
                    if (GraphicsComponent.GetCurrentAnimation != "SlimeJumpLeftBottom")
                    {
                        GraphicsComponent.StartAnimation("SlimeJumpLeftBottom");
                    }
                    getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y - 5, 48, 5));
                    if (getCols.Count > 0)
                    {
                        isJumping = false;
                        isDown = false;
                    }
                }

            }
            else if (!isGoRight && isDown)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SlimeReadyJumpRightTop")
                {
                    GraphicsComponent.StartAnimation("SlimeReadyJumpRightTop");
                }
                delay--;
                if (delay <= 0)
                {
                    velocity = new Vector2(-5, -3);
                    if (GraphicsComponent.GetCurrentAnimation != "SlimeJumpRightTop")
                    {
                        GraphicsComponent.StartAnimation("SlimeJumpRightTop");
                    }
                    getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y - 5, 48, 5));
                    if (getCols.Count > 0)
                    {
                        isJumping = false;
                        isDown = false;
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
                    isJumping = true;
                    velocity = new Vector2(5, 3);
                    if (GraphicsComponent.GetCurrentAnimation != "SlimeJumpLeftTop")
                    {
                        GraphicsComponent.StartAnimation("SlimeJumpLeftTop");
                    }
                    getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height, 48, 5));
                    if (getCols.Count > 0)
                    {
                        isJumping = false;
                        isDown = true;
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
                    velocity = new Vector2(-5, 3);
                    if (GraphicsComponent.GetCurrentAnimation != "SlimeJumpRightTop")
                    {
                        GraphicsComponent.StartAnimation("SlimeJumpRightTop");
                    }
                    getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height, 48, 5));
                    if (getCols.Count > 0)
                    {
                        isJumping = false;
                        isDown = true;
                    }
                }


            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(debugTexture, new Rectangle((int)Pos.X, (int)Pos.Y - 5, 48, 5), Color.White);
            spriteBatch.Draw(debugTexture, new Rectangle((int)Pos.X, (int)Pos.Y + Height, 48, 5), Color.White);
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

            if (Pos.X >= rightBorder)
            {
                isGoRight = false;
            }

            else if (Pos.X <= leftBorder)
            {
                isGoRight = true;
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
            //if (!isAttaking){ Move(gameTime); }


            base.Update(gameTime);
        }

        public void Target()
        {

        }

        public void Attack(GameTime gameTime)
        {

        }
    }
}
