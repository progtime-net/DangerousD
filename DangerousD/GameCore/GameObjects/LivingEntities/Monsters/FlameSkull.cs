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
        private bool isAttack;
        protected Vector2 startPosition;
        protected Vector2[] positions = { new Vector2(-25, 242), new Vector2(-25, 332), new Vector2(582, 332), new Vector2(-25, 332), 
            new Vector2(-25, 444), new Vector2(581, 444), new Vector2(-25, 444), new Vector2(-25, 242), new Vector2(-25, 242), 
            new Vector2(-25, 242), new Vector2(-25, 149), new Vector2(-25, 149) };
        protected int i;

    public FlameSkull(Vector2 position) : base(position)
        {
            //581 149 stairs 4 [7]
            //-25 149 verv 4 [6]
            //-25 242 spawn 3 [5]
            //-25 242 verv 3 [4]
            //-25 332 verv 2 [3] 
            //582 332 stairs 2 [2]
            //-25 444 verv 1 [1] 
            //581 444 stairs 1 [0]
            i = 0;
            Width = 62;
            Height = 40;
            monster_speed = 1;
            name = "Skull";
            acceleration = Vector2.Zero;
            startPosition = new Vector2();
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "FlameSkullMoveRight" , "FlameSkullMoveLeft"}, "FlameSkullMoveRight");

        public override void Update(GameTime gameTime)
        {
            if (!isAttack)
            {
                Move(gameTime);
            }

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
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "FlameSkullMoveRight")
                {
                    GraphicsComponent.StartAnimation("FlameSkullMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (GraphicsComponent.GetCurrentAnimation != "FlameSkullMoveLeft")
                {
                    GraphicsComponent.StartAnimation("FlameSkullMoveLeft");
                }
                velocity.X = -monster_speed;
            }

            if (i%2 == 0)
            {
                if (Pos.X > positions[i].X)
                {
                    isGoRight = false;
                }
                else if (Pos.X < positions[i].X)
                {
                    isGoRight = true;
                }
                else if (Pos.X == positions[i].X)
                {
                    i++;
                    velocity.X = 0;
                }
            }
            else
            {
                if (Pos.Y > positions[i].Y)
                {
                    _pos.Y -= monster_speed;
                }
                else if (Pos.Y < positions[i].Y)
                {
                    _pos.Y += monster_speed;
                }
                else if (Pos.Y == positions[i].Y)
                {
                    i++;
                }
            }
        }

        public override void Attack(GameTime gameTime)
        {

        }

        public override void Target()
        {

        }
    }
}
