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
        private bool isGoRight;
        private bool isAttack;

        float leftBorder;
        float rightBorder;
        bool isAttaking = false;
        bool isTarget = false;
        PhysicsManager physicsManager;
        public Zombie(Vector2 position) : base(position)
        {
            Width = 24;
            Height = 40;
            monster_speed = 3;
            name = "Zombie";
            monster_health = 2;
            leftBorder = (int)position.X - 100;
            rightBorder = (int)position.X + 100;
            physicsManager = new PhysicsManager();
            Random random = new Random();
            if(random.Next(0, 2) == 0)
            {
                isGoRight = true;
            }
            else
            {
                isGoRight = false;
            }
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack" }, "ZombieMoveLeft");

        public override void Update(GameTime gameTime)
        {
            if (!isAttaking)
            {
                Target();
                Move(gameTime);
            }
            fixBorder();
            base.Update(gameTime);
        }

        public override void Attack()
        {
            velocity.X = 0;
            isAttaking = true;
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieRightAttack")
                {
                    GraphicsComponent.StartAnimation("ZombieRightAttack");
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
            for (int i = 0; i < 3; i++)
            {
                Particle particle = new Particle(Pos);
            }
            
            AppManager.Instance.GameManager.Remove(this);
            
        }

        public override void Move(GameTime gameTime)
        {
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
            if (AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 50, (int)Pos.Y, Width + 100, Height), typeof(Player)).Count > 0)
            {
                if (isGoRight && this._pos.X <= AppManager.Instance.GameManager.players[0].Pos.X)
                {
                    isTarget = true;
                    leftBorder = Pos.X - 10;
                    rightBorder = Pos.X + AppManager.Instance.GameManager.players[0].Pos.X;
                }

                else if (!isGoRight && this._pos.X >= AppManager.Instance.GameManager.players[0].Pos.X)
                {
                    isTarget = true;
                    rightBorder = Pos.X + 10;
                    leftBorder = AppManager.Instance.GameManager.players[0].Pos.X;
                }
            }
        }
        public void fixBorder()
        {
            if(leftBorder <= 0)
            {
                leftBorder = 0;
            }
            if(rightBorder >= 800)
            {
                rightBorder = 760;
            }
        }

        public  void Attack(GameTime gameTime)
        {}

        public void TakeDamage()
        {
            monster_health--;
            GraphicsComponent.StartAnimation("ZombieRightAttack");
            Particle particle = new Particle(Pos);
            if (monster_health <= 0)
            {
                Death();
            }
        }
    }
}
