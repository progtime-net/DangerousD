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
        private Rectangle collision;
        private Vector2 position;
        private int healthBall;

        public Rectangle Collision
        {
            get { return collision; }
        }

        public FrankBalls(Vector2 position) : base(position)
        {
            this.position = position;
            name = "FrankBalls";
            Width = 40;
            Height = 40;
            monster_speed = 1;
            acceleration = Vector2.Zero;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "BallMoveRight" }, "BallMoveRight");

        public override void Attack()
        {
            collision = new Rectangle((int)position.X, (int)position.Y, 40, 40);
            
        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            
        }
    }
}
