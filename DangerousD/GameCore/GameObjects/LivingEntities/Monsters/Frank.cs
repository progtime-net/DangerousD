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
            Width = 112;
            Height = 160;
            GraphicsComponent.StartAnimation("FrankMoveLeft");
            monster_speed = 1;
            name = "Фрэнк";
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
           /* if (player.Pos.X - _pos.X <= 20 || player.Pos.X - _pos.X <= -20)
            {
                player.Death(name);
            } */

            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "FrankMoveRight")
                {
                    GraphicsComponent.StartAnimation("FrankMoveRight");
                    velocity = new Vector2(monster_speed, 0);
                }
            }
            else if (!isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "FrankMoveLeft")
                {
                    GraphicsComponent.StartAnimation("FrankMoveLeft");
                    velocity = new Vector2(-monster_speed, 0);
                }
            }

            if (_pos.X <= 1)
            {
                isGoRight = true;
            }
            else if (_pos.X >= 500)
            {
                isGoRight = false;
            }
        }
    }
}
