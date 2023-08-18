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
        private bool isFlyUp = true;
        private bool isAttacking = false;
        private int hp;

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
            velocity = new Vector2(3,-3);
            acceleration = Vector2.Zero;
            hp = 10;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "BallMoveRight" }, "BallMoveRight");

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            
            Death();
            
            base.Update(gameTime);
        }
        public override void Attack()
        {

        }
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    AppManager.Instance.GameManager.players[0].Death(name);
                }
            }
            base.OnCollision(gameObject);
        }

        public override void Death()
        {
            if (hp <= 0)
            {
                AppManager.Instance.GameManager.Remove(this);
            }
        }

        public override void Move(GameTime gameTime)
        {
            
            var getColsHor = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 50, 2));
            var getColsVer= AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y + Height / 2 - 2, 50, 2)); ;
            if (isFlyRight)
            {
                getColsHor = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y , 42, 40));
                if(getColsHor.Count > 0)
                {
                    isFlyRight = false;
                    velocity.X = -velocity.X;
                    hp--;
                }
            }
            else
            {
                getColsHor = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X-2, (int)Pos.Y, 42, 40));
                if (getColsHor.Count > 0)
                {
                    isFlyRight = true;
                    velocity.X = -velocity.X;
                    hp--;
                }
            }
            if (isFlyUp)
            {
                
                getColsVer = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X , (int)Pos.Y-3, 40, 43));
                if (getColsVer.Count > 0)
                {
                    isFlyUp = false;
                    velocity.Y = -velocity.Y;
                    hp--;
                }
            }
            else
            {
                getColsVer = AppManager.Instance.GameManager.physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y, 40, 43));
                if (getColsVer.Count > 0)
                {
                    isFlyUp = true;
                    velocity.Y = -velocity.Y;
                    hp--;
                }
            }

           
        }

        public void Target()
        {
            throw new NotImplementedException();
        }

        public override void Attack(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
