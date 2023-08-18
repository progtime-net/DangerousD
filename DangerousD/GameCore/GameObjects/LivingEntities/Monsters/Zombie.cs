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
using DangerousD.GameCore.Network;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class Zombie : CoreEnemy
    {

        float leftBorder;
        float rightBorder;
        bool isAttaking = false;
        bool isTarget = false;
        PhysicsManager physicsManager;
        public Zombie(Vector2 position) : base(position)
        {
            Width = 24;
            Height = 40;
            monster_speed = 2;
            name = "Zombie";
            monster_health = 2;
            leftBorder = (int)position.X - 100;
            rightBorder = (int)position.X + 100;
            physicsManager = new PhysicsManager();
            Random random = new Random();
            monster_health = 2;
            if(random.Next(0, 2) == 0)
            {
                isGoRight = true;
            }
            else
            {
                isGoRight = false;
            }

            this.GraphicsComponent.actionOfAnimationEnd += (a) =>
            {
                if (a == "ZombieRightAttack" || a == "ZombieLeftAttack")
                {
                    isAttaking = false;
                }
            };
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack" }, "ZombieMoveLeft");

        public override void Update(GameTime gameTime)
        {
            if (!isAttaking)
            {
                Target();
                Move(gameTime);
            }
            //fixBorder();
            base.Update(gameTime);
        }

        public override void Attack()
        {
            isAttaking = true;
            PlayAttackAnimation();
            AppManager.Instance.GameManager.GetClosestPlayer(Pos).Death(name);
        }
        public void PlayAttackAnimation()
        {
            velocity.X = 0;
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieRightAttack")
                {
                    GraphicsComponent.StartAnimation("ZombieRightAttack");
                }
            }
            else if (!isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieLeftAttack")
                {
                    GraphicsComponent.StartAnimation("ZombieLeftAttack");
                }
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
                StartCicycleAnimation("ZombieMoveRight");
                velocity.X = monster_speed;
            }

            else if (!isGoRight)
            {
                StartCicycleAnimation("ZombieMoveLeft");
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
            if (gameObject.id == AppManager.Instance.GameManager.GetClosestPlayer(Pos).id && AppManager.Instance.GameManager.GetClosestPlayer(Pos).IsAlive)
            {
                if (AppManager.Instance.multiPlayerStatus != MultiPlayerStatus.Client)
                {
                    Attack();
                }
            }
            else if (gameObject is Player)
            {
                if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host)
                {
                    NetworkTask task = new NetworkTask();
                    AppManager.Instance.NetworkTasks.Add(task.KillPlayer(gameObject.id, name));
                }
            }
            base.OnCollision(gameObject);
        }

        public void Target()
        {
            if (AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 50, (int)Pos.Y, Width + 200, Height), typeof(Player)).Count > 0)
            {
                if(isGoRight && this._pos.X <= AppManager.Instance.GameManager.players[0].Pos.X)
                {
                    isTarget = true;
                    leftBorder = Pos.X - 100;
                    rightBorder = Pos.X + AppManager.Instance.GameManager.players[0].Pos.X;
                }

                else if(!isGoRight && this._pos.X >= AppManager.Instance.GameManager.players[0].Pos.X)
                {
                    isTarget = true;
                    rightBorder = Pos.X + 100;
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
        public void SwitchToRight()
        {
            isGoRight = true;
        }

        public void SwitchToLeft()
        {
            isGoRight = false;
        }
        public override void Attack(GameTime gameTime)
        {}

        public override void TakeDamage()
        {
            monster_health--;
            Particle particle = new Particle(Pos);
            if (monster_health <= 0)
            {
                Death();
            }
        }
        }
    }
