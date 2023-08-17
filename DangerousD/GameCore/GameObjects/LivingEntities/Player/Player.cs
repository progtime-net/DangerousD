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
        bool isJump = false;
        public int health;
        public bool isGoRight = false;
        public Vector2 playerVelocity;
        public int rightBorder;
        public int leftBorder;
        public bool isVisible = true;
        public GameObject objectAttack;

        public Player(Vector2 position) : base(position)
        {
            Width = 16;
            Height = 32;

            AppManager.Instance.InputManager.ShootEvent += Shoot;

            AppManager.Instance.InputManager.MovEventJump += AnimationJump;
            AppManager.Instance.InputManager.MovEventDown += MoveDown;

           velocity = new Vector2(0, 0);
            rightBorder = (int)position.X + 100;
            leftBorder = (int)position.X - 100;

        }
        public bool IsAlive { get { return isAlive; } }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack", "DeathFromZombie" }, "ZombieMoveLeft");//TODO: Change to player

        public void Attack()
        {
            if (objectAttack.Rectangle.Intersects(this.Rectangle))
            {
                isVisible = false;
            }
        }
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                //isVisible = false;
            }
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
            /*if(monsterName == "Zombie")
            {
                DeathRectangle deathRectangle = new DeathRectangle(Pos, "DeathFrom" + monsterName);
                deathRectangle.Gr.actionOfAnimationEnd += (a) =>
                {
                    if (a == "DeathFrom" + monsterName)
                    {
                        AppManager.Instance.ChangeGameState(GameState.GameOver);
                    }
                };
            }
            isAlive = false;*/
        }
        public void AnimationJump()
        {
            velocity.Y = -11;
            isJump = true;
            // здесь будет анимация
        }
        public void Shoot()
        {

        }

        public override void Update(GameTime gameTime)
        {
            GraphicsComponent.CameraPosition = (_pos-new Vector2(200, 350)).ToPoint();
            velocity.X = 0.5f;
            if (velocity.Y == 0)
            {
                isJump = false;
            }
            base.Update(gameTime);
            if (id == AppManager.Instance.GameManager.GetPlayer1.id)
            {
                Move(gameTime);
            }
        }

        public void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (AppManager.Instance.InputManager.VectorMovementDirection.X==1)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveRight")//идёт направо
                {
                    GraphicsComponent.StartAnimation("ZombieMoveRight");
                }
                velocity.X = 5;
            }
            else if (AppManager.Instance.InputManager.VectorMovementDirection.X == -1)//идёт налево
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveLeft")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveLeft");
                }
                 velocity.X = -5;
            }
            else if(AppManager.Instance.InputManager.VectorMovementDirection.X == 0)//стоит
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveLeft")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveLeft");
                }
                velocity.X = 0;
            }
            if (AppManager.Instance.multiPlayerStatus != MultiPlayerStatus.SinglePlayer)
            {
                NetworkTask task = new NetworkTask(id, Pos);
                AppManager.Instance.NetworkTasks.Add(task);
            }
        }
        public void MoveDown()
        {
            velocity.Y = -11;
            isJump = true;
        }

    }
}
