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

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Player : LivingEntity
    {
        bool isAlive = true;
        public int health;
        public bool isGoRight = false;
        public Vector2 playerVelocity;
        public int rightBorder;
        public int leftBorder;
        public bool isVisible = true;
        public GameObject objectAttack;

        public Player(Vector2 position) : base(position)
        {
            Width = 32;
            Height = 64;
            AppManager.Instance.InputManager.MovEventJump += Jump;
            AppManager.Instance.InputManager.ShootEvent += Shoot;
            playerVelocity = new Vector2(100, 0);
            rightBorder = (int)position.X + 100;
            leftBorder = (int)position.X - 100;
        }
        public bool IsAlive { get { return isAlive; } }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack", "DeathFromZombie" }, "ZombieMoveLeft");//TODO: Change to player
        public void Update(GameTime gameTime)
        {
            if (!isVisible)
            {

            }
            Move(gameTime);
        }
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
                isVisible = false;
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
            if(monsterName == "Zombie")
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
            isAlive = false;
        }
        public void Jump()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                velocity.Y = -300;
            }
            // здесь будет анимация
        }
        public void Shoot()
        {

        }
        public void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveRight")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveRight");
                }
                _pos = playerVelocity * delta;
            }
            else if (!isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveLeft")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveLeft");
                }
                    _pos= -playerVelocity * delta;
            }
            if (_pos.X >= rightBorder)
            {
                isGoRight = false;
            }
            if (_pos.X >= leftBorder)
            {
                isGoRight = true;
            }
        }

    }
}
