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
        protected Vector2[] positions = { new Vector2(0,246), new Vector2(0, 344), new Vector2(550,344), new Vector2(550, 246)};
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
            monster_speed = 10;
            name = "FlameSkull";
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

        public override void Attack(GameTime gameTime)
        {

        }

        public override void Death()
        {

        }

        double time_since_target_changed = 0;
        public override void Move(GameTime gameTime)
        {
            time_since_target_changed += gameTime.ElapsedGameTime.TotalSeconds;
            if (isGoRight)
            {
                StartCicycleAnimation("FlameSkullMoveRight"); 
                velocity.X = monster_speed;
            }
            else
            {
                StartCicycleAnimation("FlameSkullMoveLeft"); 
                velocity.X = -monster_speed;
            }

            if (positions[GetOreviousTarget()].X > positions[i].X)
            {
                isGoRight = false;
            }
            if (positions[GetOreviousTarget()].X < positions[i].X)
            {
                isGoRight = true;
            }

            double dt = 10 * monster_speed * time_since_target_changed / (positions[GetOreviousTarget()] - positions[i]).Length();
            float ease = (float)easeOutElastic(dt);
            _pos = positions[GetOreviousTarget()] * (1 - ease) + (positions[i]) * (ease);
            if (dt >= 1)
            {
                i = (i+1)% positions.Length;
                time_since_target_changed = 0;
            } 
        }
        public int GetOreviousTarget()
        {
            if (i == 0)
                return positions.Length - 1;
            else
                return i - 1;
        }
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                velocity.Y = 0;
                velocity.X = 0;
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    AppManager.Instance.GameManager.players[0].Death(name);
                }
            }
            base.OnCollision(gameObject);
        }
         
        public override void Target()
        {

        }
        public double easeOutElastic(double x)
        {

            double c1 = 1.70158;
            double c2 = c1 * 1.525;
            return x < 0.5
               ? (Math.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
               : (Math.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;


            //return Math.Sin((x * Math.PI) / 2);
            const double c4 = (2 * Math.PI) / 10.5;
            int cx = 5;
            if (x > 0.5)
                return 1 - Math.Pow(2, -cx * x) * Math.Sin((x * cx - 0.75) * c4);
            else
                return Math.Pow(2, -cx * x) * Math.Sin((x * cx - 0.75) * c4) + 1;
        }
    }
}
