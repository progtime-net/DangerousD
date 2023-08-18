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
            Width = 48;
            Height = 62;
            GraphicsComponent.StartAnimation("GhostSpawn");
            acceleration = Vector2.Zero;

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

        }

        public override void Death()
        {

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
            if (Pos.X >= rightBoarder)
            {
                isGoRight = false;
            }
            else if (Pos.X <= leftBoarder)
            {
                isGoRight = true;
            }
            if (true)
            {

            }
        }

        public override void Attack(GameTime gameTime)
        {

        }

        public override void Target()
        {
            throw new NotImplementedException();
        }
    }
}
