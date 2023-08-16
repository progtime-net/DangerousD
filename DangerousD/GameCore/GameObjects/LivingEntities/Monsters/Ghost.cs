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

        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "GhostMoveRight", "GhostMoveLeft", "GhostSpawn", "GhostAttack" }, "");


        public override void Attack(Player player)
        {

        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime, Player player)
        {

        }
    }
}
