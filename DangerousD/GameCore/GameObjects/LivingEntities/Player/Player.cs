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
using DangerousD.GameCore.Network;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Player : LivingEntity
    {
        bool isAlive = true;
        bool isRight;
        string stayAnimation;
        bool isJump = false;
        public int health;
        public bool isGoRight = false;
        public Vector2 playerVelocity;
        public int rightBorder;
        public int leftBorder;
        public bool isVisible = true;
        private bool isAttacked = false;
        public bool isInvincible;
        public GameObject objectAttack;

        public Player(Vector2 position, bool isInvincible = false) : base(position)
        {
            Width = 16;
            Height = 32;

            AppManager.Instance.InputManager.ShootEvent += Shoot;

            AppManager.Instance.InputManager.MovEventJump += Jump;
            AppManager.Instance.InputManager.MovEventDown += MoveDown;

           velocity = new Vector2(0, 0);
            rightBorder = (int)position.X + 100;
            leftBorder = (int)position.X - 100;
            this.isInvincible = isInvincible;

        }

        public bool IsAlive { get { return isAlive; } }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "playerMoveLeft", "playerMoveRight", "DeathFromZombie", "playerRightStay", "playerStayLeft",
            "playerJumpRight" , "playerJumpLeft"}, "playerStayLeft");

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
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                base.Draw(spriteBatch);
            }
        }
        public void Death(string monsterName)
        {
            isAttacked = true;
            if(monsterName == "Zombie")
            {
                DeathRectangle deathRectangle = new DeathRectangle(Pos, "DeathFrom" + monsterName);
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

        }

        public override void Update(GameTime gameTime)
        {
            GraphicsComponent.CameraPosition = (_pos-new Vector2(200, 350)).ToPoint();
            if (!isAttacked || isInvincible)
            {
                Move(gameTime);
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
            if (AppManager.Instance.InputManager.VectorMovementDirection.X > 0)
            {
                if (GraphicsComponent.GetCurrentAnimation != "playerMoveRight")//идёт направо
                {
                    GraphicsComponent.StartAnimation("playerMoveRight");
                }
            }
            else if (AppManager.Instance.InputManager.VectorMovementDirection.X < 0)//идёт налево
            {
                if (GraphicsComponent.GetCurrentAnimation != "playerMoveLeft")
                {
                    GraphicsComponent.StartAnimation("playerMoveLeft");
                }
            }
            else if(AppManager.Instance.InputManager.VectorMovementDirection.X == 0)//стоит
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveLeft")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveLeft");
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
            // ПОЧЕМУ
            velocity.Y = -11;
        }

    }
}
