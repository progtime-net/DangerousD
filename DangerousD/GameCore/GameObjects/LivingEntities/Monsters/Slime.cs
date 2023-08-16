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
    public class Slime : CoreEnemy
    {
        public Slime(Vector2 position) : base(position)
        {
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SlimeMoveLeftTop", "SlimeMoveLeftBottom", "SlimeMoveRightTop",
            "SlimeMoveRightBottom", "SlimeReadyJumpRightBottom", "SlimeReadyJumpRightTop", "SlimeReadyJumpLeftBottom", "SlimeReadyJumpLeftTop", "SlimeJumpRightBottom", 
            "SlimeJumpRightTop", "SlimeJumpLeftBottom", "SlimeJumpLeftTop" }, "");

        public override void Attack()
        {

        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {

        }
    }
}
