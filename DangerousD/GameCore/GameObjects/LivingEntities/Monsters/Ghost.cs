using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void Attack()
        {
            velocity.X = 0;
            isAttack = true;
            if (GraphicsComponent.GetCurrentAnimation != "GhostAttack")
            {
                GraphicsComponent.StartAnimation("GhostAttack");
            }
            
            AppManager.Instance.GameManager.players[0].Death(name);
        }

        public override void Death()
        {

        }
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    Attack();

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
            var getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 50, 2));
            if (isGoRight)
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, Width + 4, 2));
            }
            else
            {
                getCols = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 3, (int)Pos.Y + Height / 2 - 2, Width + 3, 2));
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

        public override void Attack(GameTime gameTime)
        {

        }
        public void TakeDamage()
        {
            monster_health--;

<<<<<<< HEAD
        public override void Target()
=======
            
            if (monster_health <= 0)
            {
                Death();
            }
        }
        public void Target()
>>>>>>> черешня
        {
            throw new NotImplementedException();
        }
    }
}
