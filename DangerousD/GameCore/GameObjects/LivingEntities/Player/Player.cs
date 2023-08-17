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
        bool isRight;
        string stayAnimation;
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

            AppManager.Instance.InputManager.ShootEvent += Shoot;

            AppManager.Instance.InputManager.MovEventJump += AnimationJump;
            AppManager.Instance.InputManager.MovEventDown += MoveDown;

           playerVelocity = new Vector2(100, 0);
            rightBorder = (int)position.X + 100;
            leftBorder = (int)position.X - 100;

        }

        private void AnimationJump()
        {
            velocity.Y = -300;
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
            velocity.Y = -300;
            //сюда анимацию и доделать
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                velocity.Y = -300;
            }
            // здесь будет анимация
        }
        public void Shoot()
        {

        }

        public override void Update(GameTime gameTime)
        {
            GraphicsComponent.CameraPosition = (_pos-new Vector2(200, 350)).ToPoint();
            velocity.X = 0.5f;
            if (!isVisible)
            {

            }
            Move(gameTime);
            base.Update(gameTime);
        }

        public void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "playerMoveRight")
                {
                    GraphicsComponent.StartAnimation("playerMoveRight");
                }
                _pos = playerVelocity * delta;
            }
            else if (!isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "playerMoveLeft")
                {
                    GraphicsComponent.StartAnimation("playerMoveLeft");
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
        public void MoveDown()
        {

        }

    }
}
