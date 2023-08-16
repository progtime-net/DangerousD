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
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "FlameSkullMoveRight" , "FlameSkullMoveLeft"}, "FlameSkullMoveRight");

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
