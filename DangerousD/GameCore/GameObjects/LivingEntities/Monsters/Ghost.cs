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
        public Ghost(Vector2 position) : base(position)
        {
            monster_speed = 1;
            name = "Ghost";
            Width = 48;
            Height = 62;
            GraphicsComponent.StartAnimation("GhostSpawn");

        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "GhostMoveRight", "GhostMoveLeft", "GhostSpawn", "GhostAttack" }, "GhostMoveRight");

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
