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
    internal class Frank : CoreEnemy
    {
        public Frank(Vector2 position) : base(position)
        {
            isGoRight = false;
            Width = 112;
            Height = 160;
            leftBoarder = 50;
            rightBoarder = 300;
            GraphicsComponent.StartAnimation("FrankMoveLeft");
            monster_speed = 2;
            name = "Frank";
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "FrankMoveRight", "FrankMoveLeft" }, "FrankMoveLeft");

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
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
                if (GraphicsComponent.GetCurrentAnimation != "FrankMoveRight")
                {
                    GraphicsComponent.StartAnimation("FrankMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (GraphicsComponent.GetCurrentAnimation != "FrankMoveLeft")
                {
                    GraphicsComponent.StartAnimation("FrankMoveLeft");
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
    }
}
