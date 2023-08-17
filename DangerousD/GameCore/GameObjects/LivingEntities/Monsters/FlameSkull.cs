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
    public class FlameSkull : CoreEnemy
    {
        public FlameSkull(Vector2 position) : base(position)
        {
            Width = 62;
            Height = 40;
            monster_speed = 3;
            name = "Skull";
            acceleration = Vector2.Zero;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "FlameSkullMoveRight" , "FlameSkullMoveLeft"}, "FlameSkullMoveRight");

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
                if (GraphicsComponent.GetCurrentAnimation != "FlameSkullMoveRight")
                {
                    GraphicsComponent.StartAnimation("FlameSkullMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (GraphicsComponent.GetCurrentAnimation != "FlameSkullMoveLeft")
                {
                    GraphicsComponent.StartAnimation("FlameSkullMoveLeft");
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
            throw new NotImplementedException();
        }
    }
}
