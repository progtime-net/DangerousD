using DangerousD.GameCore.GameObjects.Entities;
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
    public class SilasMaster : CoreEnemy
    {
        private int attackTime = 60;
        private int moveTime = 360;
        private int currentTime = 0;
        int leftBorder;
        int rightBorder;
        List<SilasHands> hands = new List<SilasHands>();
        List<SilasBall> balls = new List<SilasBall>();
        public SilasMaster(Vector2 position) : base(position)
        {
            name = "SilasMaster";
            Width = 48;
            Height = 53;
            monster_health = 15;
            monster_speed = 2;
            acceleration = Vector2.Zero;
            leftBorder = (int)position.X - 60;
            rightBorder = (int)position.X + 120;
            acceleration = Vector2.Zero;
            hands.Add(new SilasHands(new Vector2(Pos.X+0,Pos.Y+53)));
            hands.Add(new SilasHands(new Vector2(Pos.X + 24, Pos.Y + 53)));
            
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new GraphicsComponent(new List<string>() { "SilasMove", "SilasAttack" }, "SilasMove");
        public override void Attack()
        {

            if (currentTime == 0)
            {
                GraphicsComponent.StartAnimation("SilasAttack");

            }
            else if (currentTime == attackTime / 2)
            {
                SpawnAttackBall();
            }
            else if (currentTime >= attackTime)
            {

                GraphicsComponent.StartAnimation("SilasMove");
                currentTime = 0;
            }
            currentTime++;
        }

        private void SpawnAttackBall()
        {
            balls.Clear();
            for (int i = 0; i < 4; i++)
            {
                SilasBall silasball = new SilasBall(new Vector2(Pos.X + i * 12, Pos.Y + 53), new Vector2((i - 2) * 2, 2));

                balls.Add(silasball);
            }


        }

        public override void Attack(GameTime gameTime)
        {

        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            if (currentTime == 0)
            {
                GraphicsComponent.StartAnimation("SilasMove");
            }
            else if (currentTime >= moveTime)
            {
                GraphicsComponent.StartAnimation("SilasAttack");
                currentTime = 0;
            }
            currentTime++;
            if (isGoRight)
            {
                velocity.X = monster_speed;
            }
            else if (!isGoRight)
            {
                velocity.X = -monster_speed;
            }

            if (Pos.X >= rightBorder)
            {
                isGoRight = false;
            }
            else if (Pos.X <= leftBorder)
            {
                isGoRight = true;
            }
        }

        public override void Target()
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (hands.Count<2)
            {
                hands.Add(new SilasHands(new Vector2(Pos.X + 60, Pos.Y + 120)));
            }
            if (GraphicsComponent.CurrentAnimation.Id == "SilasMove")
            {
                Move(gameTime);
            }
            else
            {
                velocity = Vector2.Zero;
                Attack();
            }
        }
    }
}
