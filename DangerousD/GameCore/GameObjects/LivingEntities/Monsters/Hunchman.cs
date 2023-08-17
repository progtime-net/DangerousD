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
    public class Hunchman : CoreEnemy
    {
        private bool isGoRight = true;

        public Hunchman(Vector2 position) : base(position)
        {
            Width = 20;
            Height = 30;
            name = "Hunchman";
            GraphicsComponent.StartAnimation("");
            monster_speed = 3;

        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "HunchmanMoveLeft", "HunchmanMoveRight", "HunchmanAttackLeft", "HunchmanAttackRight" }, "HunchmanMoveRight");

        public override void Attack()
        {

        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            var player = AppManager.Instance.GameManager.players[0];
            if(player.Pos.X > 0)
            {

            }
        }
    }
}
