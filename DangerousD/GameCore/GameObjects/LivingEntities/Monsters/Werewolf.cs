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
        public Werewolf(Vector2 position) : base(position)
        {
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "WolfMoveRight", "WolfMoveLeft", "WolfJumpRight", "WolfJumpLeft" }, "");

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
