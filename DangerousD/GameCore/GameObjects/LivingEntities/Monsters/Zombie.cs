using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.Managers;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class Zombie : CoreEnemy
    {
        private bool isGoRight = true;
        float leftBorder;
        float rightBorder;
        bool isAttaking = false;
        bool isTarget = false;
        PhysicsManager physicsManager;
        public Zombie(Vector2 position) : base(position)
        {
            Width = 48;
            Height = 80;
            monster_speed = 3;
            name = "Zombie";
            leftBorder = (int)position.X - 60;
            rightBorder = (int)position.X + 120;
            physicsManager = new PhysicsManager();
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack" }, "ZombieMoveLeft");

        public override void Update(GameTime gameTime)
        {
            if (!isAttaking)
            {
                Target();
                Move(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Attack()
        {
            velocity.X = 0;
            isAttaking = true;
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveRight")
                {
                    GraphicsComponent.StartAnimation("ZombieAttackRight");
                }
                AppManager.Instance.GameManager.players[0].Death(name);
            }
            else if (!isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieLeftAttack")
                {
                    GraphicsComponent.StartAnimation("ZombieLeftAttack");
                }
                AppManager.Instance.GameManager.players[0].Death(name);
            }
        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveRight")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveRight");
                }
                velocity.X = monster_speed;
            }

            else if (!isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveLeft")
                {
                    GraphicsComponent.StartAnimation("ZombieMoveLeft");
                }
                velocity.X = -monster_speed;
            }

            if(Pos.X >= rightBorder)
            {
                isGoRight = false;
            }

            else if(Pos.X <= leftBorder)
            {
                isGoRight = true;
            }
        }
        public override void OnCollision(GameObject gameObject)
        {
            if(gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    Attack();
                }
            }
            base.OnCollision(gameObject);
        }

        public void Target()
        {
            if(physicsManager.RayCast(this, AppManager.Instance.GameManager.players[0]) == null)
            {
                if(isGoRight && this._pos.X <= AppManager.Instance.GameManager.players[0].Pos.X)
                {
                    isTarget = true;
                    leftBorder = Pos.X - 10;
                    rightBorder = Pos.X + AppManager.Instance.GameManager.players[0].Pos.X;
                }

                else if(!isGoRight && this._pos.X >= AppManager.Instance.GameManager.players[0].Pos.X)
                {
                    isTarget = true;
                    rightBorder = Pos.X + 10;
                    leftBorder = AppManager.Instance.GameManager.players[0].Pos.X; 
                }
            }
        }
    }
}
