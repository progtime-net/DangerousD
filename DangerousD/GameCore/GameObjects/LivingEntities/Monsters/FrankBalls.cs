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
    public class FrankBalls : CoreEnemy
    {
        public FrankBalls(Vector2 position) : base(position)
        {
            name = "FrankBalls";
            Width = 40;
            Height = 40;
            monster_speed = 1;
            acceleration = Vector2.Zero;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "BallMoveRight" }, "BallMoveRight");

        public override void Update(GameTime gameTime)
        {
            
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

        }

        public override void Attack(GameTime gameTime)
        {

        }
    }
}
