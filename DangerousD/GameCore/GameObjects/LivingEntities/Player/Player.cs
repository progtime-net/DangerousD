﻿using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects.PlayerDeath;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using DangerousD.GameCore.GameObjects.LivingEntities.Monsters;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Player : LivingEntity
    {
        bool isAlive = true;
        bool isRight = true;
        string stayAnimation;
        bool isJump = false;
        public int health;
        public Vector2 playerVelocity;
        public int rightBorder;
        public int leftBorder;
        public bool isVisible = true;
        private bool isAttacked = false;
        public bool isShooting = false;
        public GameObject objectAttack;
        private int bullets;
        public bool FallingThroughPlatform = false;
        public bool isUping = false;
        public bool isNetworkPlayer;
        private int shootLength = 160;
        public int score = 0;
        Random random = new Random();



        public int Bullets { get { return bullets; } }

        /// <summary>
        /// Don't delete this constructor. Used in MapManager!!!!
        /// </summary>
        /// <param name="position"></param>
        public Player(Vector2 position) : base(position)
        {
           Initialize(position);
        }

        public Player(Vector2 position, bool isNetworkPlayer) : base(position)
        {
            this.isNetworkPlayer = isNetworkPlayer;
            Initialize(position);
        }

        public void Initialize(Vector2 position)
        {
            Width = 16;
            Height = 32;

            if (!isNetworkPlayer)
            {
                AppManager.Instance.InputManager.ShootEvent += Shoot;
                AppManager.Instance.InputManager.MovEventJump += Jump;
                AppManager.Instance.InputManager.MovEventDown += MoveDown;
                velocity = new Vector2(0, 0);
                rightBorder = (int)position.X + 100;
                leftBorder = (int)position.X - 100;
                bullets = 5;

                GraphicsComponent.actionOfAnimationEnd += (a) =>
                {
                    if (a == "playerShootLeft" || a == "playerShootRight")
                    {
                        isShooting = false;
                    }
                    if (a == "playerReload")
                    {
                        bullets++;
                        AppManager.Instance.SoundManager.StartSound("reloading", Pos, Pos, pitch: (float)(random.NextDouble()/2-0.25));
                    }
                    if(a == "playerShootBoomUpRight" || a == "playerShootBoomUpLeft")
                    {
                        isShooting = false;
                    }
                };
            }
        }

        public bool IsAlive { get { return isAlive; } }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "playerMoveLeft", "playerMoveRight", "DeathFromZombie", "playerRightStay", "playerStayLeft",
            "playerJumpRight" , "playerJumpLeft", "playerShootLeft", "playerShootRight", "playerReload", "smokeAfterShoot", "playerShootUpRight", "playerShootUpLeft", "playerShootBoomUpRight",
        "playerShootBoomUpLeft"}, "playerReload");

        public void Attack()
        {
            if (objectAttack.Rectangle.Intersects(this.Rectangle))
            {
                isVisible = false;
            }

        }
        public override void OnCollision(GameObject gameObject)
        {
            base.OnCollision(gameObject);
        }
        public Rectangle GetShootRectangle(bool isRight)
        {
            if (isRight)
                return new Rectangle((int)Pos.X, (int)(Pos.Y) + 10, shootLength + Width, Height / 2);
            else
                return new Rectangle((int)Pos.X - shootLength, (int)(Pos.Y) + 10, shootLength, Height / 2);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                base.Draw(spriteBatch);
            }
            if (AppManager.Instance.InputManager.CollisionsCheat)
            {
                Rectangle attackRect = GetShootRectangle(isRight);
                DrawDebugRectangle(spriteBatch, attackRect, Color.Orange);  
            }
        } 
        public void Death(string monsterName)
        {
            if (AppManager.Instance.InputManager.InvincibilityCheat)
                return;
            isAttacked = true;
            AnimationRectangle deathRectangle = new AnimationRectangle(Pos, "DeathFrom" + monsterName);
            deathRectangle.Gr.actionOfAnimationEnd += (a) =>
            {
                AppManager.Instance.ChangeGameState(GameState.Death);
            };
            isAlive = false;
        }
        public void Jump()
        {
            if (isOnGround)
            {
                velocity.Y = -11;
            }
            // здесь будет анимация
        }
        public void Shoot()
        {
            AppManager.Instance.DebugHUD.Set("shotanimInterval", GraphicsComponent.CurrentFrameInterval.ToString());
            AppManager.Instance.DebugHUD.Set("shotanimFrame", GraphicsComponent.CurrentFrame.ToString());
            if (bullets > 0)
            {
                if (!isAttacked)
                {
                    if (!isShooting ||(isShooting && GraphicsComponent.CurrentFrameInterval <10))
                    {
                        if (isShooting && GraphicsComponent.CurrentFrame == 1&&GraphicsComponent.CurrentFrameInterval < 10)
                        {
                            AppManager.Instance.DebugHUD.Set("shotanimInterval", GraphicsComponent.CurrentFrameInterval.ToString());
                        }
                        AppManager.Instance.SoundManager.StartSound("shotgun_shot", Pos, Pos);
                        isShooting = true;
                        if (!AppManager.Instance.InputManager.InfiniteAmmoCheat)
                            bullets--;
                        if (isRight)
                        {
                            if (!isUping)
                            {
                                GraphicsComponent.StartAnimation("playerShootRight");
                                Bullet bullet = new Bullet(new Vector2(Pos.X + 16, Pos.Y));
                                bullet.ShootRight();
                                SmokeAfterShoot smokeAfterShoot = new SmokeAfterShoot(new Vector2(Pos.X + 30, Pos.Y + 7));
                            }
                            else
                            {
                                GraphicsComponent.StartAnimation("playerShootBoomUpRight");
                                Bullet bullet = new Bullet(new Vector2(Pos.X + 16, Pos.Y));
                                bullet.ShootUpRight();
                                SmokeAfterShoot smokeAfterShoot = new SmokeAfterShoot(new Vector2(Pos.X + 12, Pos.Y - 8));
                            }
                        }
                        else if(!isRight)
                        {
                            if (!isUping)
                            {
                                GraphicsComponent.StartAnimation("playerShootLeft");
                                Bullet bullet = new Bullet(new Vector2(Pos.X, Pos.Y));
                                bullet.ShootLeft();
                                SmokeAfterShoot smokeAfterShoot = new SmokeAfterShoot(new Vector2(Pos.X - 12, Pos.Y + 7));
                            }
                            else
                            {
                                GraphicsComponent.StartAnimation("playerShootBoomUpLeft");
                                Bullet bullet = new Bullet(new Vector2(Pos.X, Pos.Y));
                                bullet.ShootUpLeft();
                                SmokeAfterShoot smokeAfterShoot = new SmokeAfterShoot(new Vector2(Pos.X - 6, Pos.Y - 7));
                            }
                        }
                    }
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (AppManager.Instance.InputManager.ScopeState == ScopeState.Up)
                isUping = true;
            else
                isUping = false;

            if (isOnGround && FallingThroughPlatform)
                FallingThroughPlatform = false;

            GraphicsComponent.SetCameraPosition(Pos);
            if (!isAttacked || AppManager.Instance.InputManager.InvincibilityCheat)
            {
                if (!isShooting)
                {
                    Move(gameTime);
                }
                else
                { 
                   // velocity.X = 0.98f * velocity.X;
                }
            }
            else
            {
                velocity.X = 0;
            }

            base.Update(gameTime);
        }

        float max_speed = 5f;
        float lastUpdSpeed = 0;
        float base_speed = 3f;
        public void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float addSpeed = 0;
            if (AppManager.Instance.InputManager.VectorMovementDirection.X != 0)
            {
                addSpeed = (AppManager.Instance.InputManager.VectorMovementDirection.X) * base_speed;
                lastUpdSpeed = velocity.X * 1;
            }
            else
            {
                lastUpdSpeed = velocity.X * 0.8f;
            }
            //if (Math.Abs(velocity.X) > max_speed * 0.8 || Math.Abs(velocity.X) < max_speed * 0.1)
            //    addSpeed *= 2;

            //if (Math.Abs(velocity.X) > max_speed * 0.9 || Math.Abs(velocity.X) < max_speed * 0.05)
            //    addSpeed *= 5;
            velocity.X = lastUpdSpeed + addSpeed;
            velocity.X = Math.Clamp(velocity.X , -max_speed, max_speed);
            if (Math.Abs(lastUpdSpeed) < 0.001) lastUpdSpeed = 0;

            AppManager.Instance.DebugHUD.Set("input X", AppManager.Instance.InputManager.VectorMovementDirection.X.ToString());
            AppManager.Instance.DebugHUD.Set("lastUpdSpeed", lastUpdSpeed.ToString());

            if (GraphicsComponent.GetCurrentAnimation != "playerShootLeft" && GraphicsComponent.GetCurrentAnimation != "playerShootRight")
            {
                if (AppManager.Instance.InputManager.VectorMovementDirection.X > 0)
                {
                    isRight = true;
                    StartCicycleAnimation("playerMoveRight");
                }
                else if (AppManager.Instance.InputManager.VectorMovementDirection.X < 0)//идёт налево
                {
                    isRight = false;
                    StartCicycleAnimation("playerMoveLeft");
                }
                else if (
                    Math.Abs(AppManager.Instance.InputManager.VectorMovementDirection.X) < max_speed
                    && AppManager.Instance.InputManager.VectorMovementDirection.X == 0
                    )//с
                     //тоит
                {
                    //lastUpdSpeed *= 0.7f;
                    if (isRight)
                    {
                        if (isUping)
                            StartCicycleAnimation("playerShootUpRight");
                        else if (bullets < 5)
                            StartCicycleAnimation("playerReload");
                        else
                            GraphicsComponent.StartAnimation("playerRightStay");
                    }
                    else if (!isRight)
                    {
                        if (isUping)
                            StartCicycleAnimation("playerShootUpLeft");
                        else if (bullets < 5)
                            StartCicycleAnimation("playerReload");
                        else
                            GraphicsComponent.StartAnimation("playerStayLeft");
                    }
                }
            }
        }
        public void MoveDown()
        {
            FallingThroughPlatform = true;
            isOnGround = false;
        }

        
    }
}