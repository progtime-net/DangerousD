﻿using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects.MapObjects;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class Ghost : CoreEnemy
    {
        private bool isAttack;

        public Ghost(Vector2 position) : base(position)
        {
            isGoRight = true;
            monster_speed = 3;
            name = "Ghost";
            Width = 24;
            Height = 30;
            GraphicsComponent.StartAnimation("GhostSpawn");
            acceleration = new Vector2(0,1);
            monster_health = 1;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "GhostMoveRight", "GhostMoveLeft", "GhostSpawn", "GhostAttack" }, "GhostMoveRight");

        public override void Update(GameTime gameTime)
        {
            if (!isAttack)
            {
                Move(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Attack(GameTime gameTime)
        {
            velocity.X = 0;
            isAttack = true;
            if (GraphicsComponent.GetCurrentAnimation != "GhostAttack")
            {
                GraphicsComponent.StartAnimation("GhostAttack");
            }
            
            AppManager.Instance.GameManager.players[0].Death(name);
        }

        public override void Death()//TODO ghost death
        {

            for (int i = 0; i < 3; i++)
            {
                Particle particle = new Particle(Pos);
            }

            AppManager.Instance.GameManager.Remove(this);
        }
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    Attack(null);

                }
            }
            base.OnCollision(gameObject);
        }

        public override void Move(GameTime gameTime)
        {
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "GhostMoveRight")
                {
                    GraphicsComponent.StartAnimation("GhostMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (GraphicsComponent.GetCurrentAnimation != "GhostMoveLeft")
                {
                    GraphicsComponent.StartAnimation("GhostMoveLeft");
                }
                velocity.X = -monster_speed;
            }
            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 50, 2),typeof(CollisionMapObject));
            if (isGoRight)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, Width + 4, 2),typeof(CollisionMapObject));
            }
            else
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 3, (int)Pos.Y + Height / 2 - 2, Width + 3, 2),typeof(CollisionMapObject));
            }


            foreach (var item in getCols)
            {
                if (item is MapObject)
                {
                    isGoRight = !isGoRight;
                    break;
                }
            }
        }
         
        public void TakeDamage()
        {
            monster_health--;
            if (monster_health <= 0)
            {
                Death();
            }
        }

        public override void Target()
        {
            
        }
    }
}
