using DangerousD.GameCore.Graphics;
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
using DangerousD.GameCore.Network;
using DangerousD.GameCore.GameObjects.MapObjects;

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
        private bool isShooting = false;
        public GameObject objectAttack;
        private int bullets;
        public bool FallingThroughPlatform = false;
        public bool isUping = false;
        public bool isNetworkPlayer;
        private int shootLength = 160;




        public int Bullets { get { return bullets; } }

        public Player(Vector2 position, bool isNetworkPlayer = false) : base(position)

        {
            this.isNetworkPlayer = isNetworkPlayer;
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

                this.GraphicsComponent.actionOfAnimationEnd += (a) =>
                {
                    if (a == "playerShootLeft" || a == "playerShootRight")
                    {
                        isShooting = false;
                    }
                    if (a == "playerReload")
                    {
                        bullets++;
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
                return new Rectangle((int)Pos.X-shootLength, (int)(Pos.Y) + 10, shootLength, Height / 2);
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
                spriteBatch.Draw(debugTexture,
                                     new Rectangle((attackRect.X - GraphicsComponent.CameraPosition.X) * GraphicsComponent.scaling,
                                     (attackRect.Y - GraphicsComponent.CameraPosition.Y) * GraphicsComponent.scaling,
                                     attackRect.Width * GraphicsComponent.scaling,
                                     attackRect.Height * GraphicsComponent.scaling), Color.White);

            }
        }
        public void Death(string monsterName)
        {
            if (AppManager.Instance.InputManager.InvincibilityCheat)
            {
                return;
            }
            isAttacked = true;
            if(monsterName == "Zombie")
            {
                AnimationRectangle deathRectangle = new AnimationRectangle(Pos, "DeathFrom" + monsterName);
                deathRectangle.Gr.actionOfAnimationEnd += (a) =>
                {
                    if (a == "DeathFrom" + monsterName)
                    {
                        AppManager.Instance.ChangeGameState(GameState.Death);
                    }
                };
            }
            else if(monsterName == "Spider")
            {
                AnimationRectangle deathRectangle = new AnimationRectangle(Pos, "DeathFrom" + monsterName);
                deathRectangle.Gr.actionOfAnimationEnd += (a) =>
                {
                    if (a == "DeathFrom" + monsterName)
                    {
                        AppManager.Instance.ChangeGameState(GameState.Death);
                    }
                };
            }
            else if (monsterName == "SilasHand")
            {
                AnimationRectangle deathRectangle = new AnimationRectangle(Pos, "DeathFrom" + monsterName);
                deathRectangle.Gr.actionOfAnimationEnd += (a) =>
                {
                    if (a == "DeathFrom" + monsterName)
                    {
                        AppManager.Instance.ChangeGameState(GameState.Death);
                    }
                };
            }
            else if (monsterName == "SilasBall")
            {
                AnimationRectangle deathRectangle = new AnimationRectangle(Pos, "DeathFrom" + monsterName);
                deathRectangle.Gr.actionOfAnimationEnd += (a) =>
                {
                    if (a == "DeathFrom" + monsterName)
                    {
                        AppManager.Instance.ChangeGameState(GameState.Death);
                    }
                };
            }
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
            if (bullets > 0)
            {
                if (!isAttacked)
                {
                    if (!isShooting)
                    {
                        isShooting = true;
                        bullets--;
                        if (isRight)
                        {
                            if (!isUping)
                            {
                                StartCicycleAnimation("playerShootRight");
                                var targets = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)(Pos.Y - 10f), shootLength + 24, 10), typeof(Zombie)).OrderBy(x => (x.Pos - Pos).LengthSquared());
                                if (targets.Count() > 0)
                                {
                                    Zombie targetZombie = (Zombie)targets.First();
                                    targetZombie.TakeDamage();
                                }
                                targets = AppManager.Instance.GameManager.physicsManager.CheckRectangle(GetShootRectangle(isRight), typeof(SilasHands)).OrderBy(x => (x.Pos - Pos).LengthSquared());
                                if (targets.Count() > 0)
                                {
                                    SilasHands targetHand = (SilasHands)targets.First();
                                    targetHand.TakeDamage();
                                }
                                SmokeAfterShoot smokeAfterShoot = new SmokeAfterShoot(new Vector2(Pos.X + 30, Pos.Y + 7));
                            }
                            else
                            {
                                StartCicycleAnimation("playerShootBoomUpRight");
                                Bullet bullet = new Bullet(new Vector2(Pos.X + 16, Pos.Y));
                                bullet.ShootUpRight();
                                SmokeAfterShoot smokeAfterShoot = new SmokeAfterShoot(new Vector2(Pos.X + 12, Pos.Y - 8));
                            }
                        }
                        else if(!isRight)
                        {
                            if (!isUping)
                            {
                                StartCicycleAnimation("playerShootLeft");
                                var targets = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)(Pos.Y - 10f), shootLength + 24, 10), typeof(Zombie)).OrderBy(x => (x.Pos - Pos).LengthSquared());
                                if (targets.Count() > 0)
                                {
                                    Zombie targetZombie = (Zombie)targets.First();
                                    targetZombie.TakeDamage();
                                }
                                targets = AppManager.Instance.GameManager.physicsManager.CheckRectangle(GetShootRectangle(isRight), typeof(SilasHands));
                                if (targets.Count() > 0)
                                {
                                    SilasHands targetHand = (SilasHands)targets.First();
                                    targetHand.TakeDamage();   
                                }
                                SmokeAfterShoot smokeAfterShoot = new SmokeAfterShoot(new Vector2(Pos.X - 12, Pos.Y + 7));
                            }
                            else
                            {
                                StartCicycleAnimation("playerShootBoomUpLeft");
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
            {
                isUping = true;
            }
            else
            {
                isUping = false;
            }
            if (isOnGround && FallingThroughPlatform)
            {
                FallingThroughPlatform = false;
            }
            GraphicsComponent.SetCameraPosition(Pos);
            if (!isAttacked  || AppManager.Instance.InputManager.InvincibilityCheat)
            {
                if (!isShooting)
                {
                    Move(gameTime);
                }
                else
                {
                    velocity.X = 0;
                }
            }
            else
            {
                velocity.X = 0;
            }

            base.Update(gameTime);
        }

        public void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity.X = 5 * AppManager.Instance.InputManager.VectorMovementDirection.X;
            if (GraphicsComponent.GetCurrentAnimation != "playerShootLeft" && GraphicsComponent.GetCurrentAnimation != "playerShootRight")
            {
                if (AppManager.Instance.InputManager.VectorMovementDirection.X > 0)
                {
                    isRight = true;
                    if (GraphicsComponent.GetCurrentAnimation != "playerMoveRight")//идёт направо
                    {
                        GraphicsComponent.StartAnimation("playerMoveRight");
                    }
                }
                else if (AppManager.Instance.InputManager.VectorMovementDirection.X < 0)//идёт налево
                {
                    isRight = false;
                    if (GraphicsComponent.GetCurrentAnimation != "playerMoveLeft")
                    {
                        GraphicsComponent.StartAnimation("playerMoveLeft");
                    }
                }
                else if (AppManager.Instance.InputManager.VectorMovementDirection.X == 0)//стоит
                {
                    if (isRight)
                    {
                        if (isUping)
                        {
                            if (GraphicsComponent.GetCurrentAnimation != "playerShootUpRight")
                            {
                                GraphicsComponent.StartAnimation("playerShootUpRight");
                            }
                        }
                        else if (bullets < 5)
                        {
                            if (GraphicsComponent.GetCurrentAnimation != "playerReload")
                            {
                                GraphicsComponent.StartAnimation("playerReload");
                            }
                        }
                        else
                        {
                            GraphicsComponent.StartAnimation("playerRightStay");
                        }
                    }
                    else if (!isRight)
                    {
                        if (isUping)
                        {
                            if (GraphicsComponent.GetCurrentAnimation != "playerShootUpLeft")
                            {
                                GraphicsComponent.StartAnimation("playerShootUpLeft");
                            }
                        }
                        else if (bullets < 5)
                        {
                            if (GraphicsComponent.GetCurrentAnimation != "playerReload")
                            {
                                GraphicsComponent.StartAnimation("playerReload");
                            }
                        }
                        else
                        {
                            GraphicsComponent.StartAnimation("playerStayLeft");
                        }
                    }
                }
            }
            if (AppManager.Instance.multiPlayerStatus != MultiPlayerStatus.SinglePlayer)
            {
                NetworkTask task = new NetworkTask(id, Pos);
                AppManager.Instance.NetworkTasks.Add(task);
            }
        }
        public void MoveDown()
        {
            FallingThroughPlatform = true;
            isOnGround = false;
        }

        public class Bullet : LivingEntity
        {
            public Bullet(Vector2 position) : base(position)
            {
                Height = 5;
                Width = 5;
            }
            protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "playerMoveLeft"}, "playerMoveLeft");
            Vector2 direction;
            Vector2 maindirection;
            public void ShootUpRight()
            {
                direction = new Vector2(1, -1);
                acceleration = Vector2.Zero;
                velocity = new Vector2(10, 10) * direction;
                maindirection = velocity;
            }
            public void ShootUpLeft()
            {
                direction = new Vector2(-1, -1);
                acceleration = Vector2.Zero;
                velocity = new Vector2(10, 10) * direction;
                                maindirection = velocity;
            }
            public override void OnCollision(GameObject gameObject)
            {
                if (gameObject is not Player)
                {
                    if (gameObject is CoreEnemy)
                    {
                        CoreEnemy enemy = (CoreEnemy)gameObject;
                        enemy.TakeDamage();
                        AppManager.Instance.GameManager.Remove(this);
                    }
                    base.OnCollision(gameObject);
                }
            }
            public override void Update(GameTime gameTime)
            {
                if (maindirection!=velocity)
                {
                    AppManager.Instance.GameManager.Remove(this);
                }
                base.Update(gameTime);
            }
        }
    }
}
