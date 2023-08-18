using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class Werewolf : CoreEnemy
    {
        private bool isAttack;

        public Werewolf(Vector2 position) : base(position)
        {
            name = "Wolf";
            monster_speed = 4;
            Width = 39;
            Height = 48;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "WolfMoveRight", "WolfMoveLeft", "WolfJumpRight", "WolfJumpLeft" }, "WolfMoveRight");

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
                if (GraphicsComponent.GetCurrentAnimation != "WolfMoveRight")
                {
                    GraphicsComponent.StartAnimation("WolfMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (GraphicsComponent.GetCurrentAnimation != "WolfMoveLeft")
                {
                    GraphicsComponent.StartAnimation("WolfMoveLeft");
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
        }

        public override void Attack(GameTime gameTime)
        {
        }

        public override void Target()
        {

        }
    }
}
