using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects.MapObjects;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class FrankBalls : CoreEnemy
    {
        private Rectangle collision;
        private bool isFlyRight = true;
        private bool isAttacking = false;
        public Rectangle Collision
        {
            get { return collision; }
        }

        public FrankBalls(Vector2 position) : base(new Vector2(300, 200))
        {
            
            name = "FrankBalls";
            Width = 40;
            Height = 40;
            monster_speed = 2;
            monster_health = 13;
            acceleration = Vector2.Zero;
            velocity = new Vector2(monster_speed, monster_speed);
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "BallMoveRight" }, "BallMoveRight");

        public override void Update(GameTime gameTime)
        {
            collision = new Rectangle((int)_pos.X, (int)_pos.Y, 40, 40);

            if (!isAttacking)
            {
                Move(gameTime);
            }

            if(GraphicsComponent.GetCurrentAnimation == "FrankMoveRight")
            {
                isFlyRight = true;
            }
            else if(GraphicsComponent.GetCurrentAnimation == "FrankMoveLeft")
            {
                isFlyRight = false;
            }

            var foundObj = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)_pos.X-1, (int)_pos.Y-1, Width+2, Height+2), typeof(StopTile));

            if (foundObj != null && foundObj.Count > 0)
            {

                float av = foundObj.Sum(x => x.Rectangle.X)/foundObj.Count;
                float avy = foundObj.Sum(x => x.Rectangle.Y)/foundObj.Count;

                if (avy <= _pos.Y)
                    velocity.Y = monster_speed;
                else if (avy >= _pos.Y)
                    velocity.Y = -monster_speed;
                if (av <= _pos.X)
                    velocity.X = -monster_speed;
                else if (av >= _pos.X)
                    velocity.X = monster_speed;
                monster_health--;
            }

            base.Update(gameTime);
        }

        public override void Attack()
        {
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
            
        }
    }
}
