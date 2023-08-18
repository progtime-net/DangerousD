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
        private bool isFlyRight = true;
        private bool isAttacking = false;

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
            monster_speed = 3;
            acceleration = Vector2.Zero;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "BallMoveRight" }, "BallMoveRight");

        public override void Update(GameTime gameTime)
        {
            if(!isAttacking)
            {
                Move(gameTime);
            }

            base.Update(gameTime);
        }
        public override void Attack()
        {
            collision = new Rectangle((int)position.X, (int)position.Y, 40, 40);
            isAttacking = true;

            if(isFlyRight)
            {
                AppManager.Instance.GameManager.players[0].Death(name);
            }
            else if(!isFlyRight)
            {
                AppManager.Instance.GameManager.players[0].Death(name);
            }

        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            velocity.X = 0;
            velocity.Y = 0;

            if(isFlyRight)
            {
                velocity.X += monster_speed;
                velocity.Y += monster_speed;
            }
            else if(!isFlyRight)
            {
                velocity.X -= monster_speed;
                velocity.Y -= monster_speed;
            }
        }
        public override void Target()
        {
            throw new NotImplementedException();
        }

        public override void Attack(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
