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
        /*protected Vector2[] positions = { new Vector2(0, 242), new Vector2(0, 332), new Vector2(582, 332), new Vector2(0, 332), 
            new Vector2(0, 444), new Vector2(582, 444), new Vector2(0, 444), new Vector2(0, 242), new Vector2(582, 242), 
            new Vector2(0, 242), new Vector2(0, 149), new Vector2(582, 149), new Vector2(0,149)};*/
        protected Vector2[] positions = { new Vector2(0,246), new Vector2(0, 344), new Vector2(550,344), new Vector2(520, 246)};
        protected int i;
    public FlameSkull(Vector2 position) : base(position)
        {   // v3 -> v2 -> s2 -> v2 -> v1 -> s1 -> v1 -> v3 -> s3 -> v3 -> v4 -> s4 -> v4 
            //0 149 verv 4 [7]
            //582 108 stairs 4 [6]
            //0 242 verv 3 [5]
            //582 220 stairs 3 [4]
            //0 332 verv 2 [3] 
            //582 332 stairs 2 [2]
            //0 444 verv 1 [1] 
            //582 444 stairs 1 [0]
            startPosition = new Vector2(500, 242);
            _pos = startPosition;
            i = 0;
            Width = 31;
            Height = 20;
            monster_speed = 2;
            name = "Skull";
            acceleration = Vector2.Zero;
            startPosition = new Vector2();
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "FlameSkullMoveRight" , "FlameSkullMoveLeft"}, "FlameSkullMoveRight");

        public override void Update(GameTime gameTime)
        {
            AppManager.Instance.DebugHUD.Set("number i: ", i.ToString());
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
            if (i == positions.Length)
            {
                i = 0;
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
