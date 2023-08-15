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
        private bool isGoRight = false;
        public Frank(Vector2 position) : base(position)
        {
            Width = 56;
            Height = 80;
            GraphicsComponent.StartAnimation("FrankMoveLeft");
            monster_speed = 50;
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "FrankMoveRight", "FrankMoveLeft" }, "FrankMoveRight");

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
            }
            else if (!isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "FrankMoveLeft")
                {
                    GraphicsComponent.StartAnimation("FrankMoveLeft");
                }
            }
        }
    }
}
