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
    public class 
        SilasHands : CoreEnemy
    {
        public SilasHands(Vector2 position) : base(position)
        {
            name = "SilasHand";
            Width = 16;
            Height = 16;
            monster_health = 2;
            monster_speed = 1;
            acceleration = Vector2.Zero;
            
        }

        protected override GraphicsComponent GraphicsComponent { get; }=new GraphicsComponent(new List<string>() { "SilasHandMove" }, "SilasHandMove");

        public override void Attack(GameTime gameTime)
        {
            AppManager.Instance.GameManager.GetPlayer1.Death(name);
        }
         
        public override void Death()
        {
            for (int i = 0; i < 3; i++)
            {
                Particle particle = new Particle(Pos);
            }

            AppManager.Instance.GameManager.Remove(this);

        }

        public override void Move(GameTime gameTime)
        {
            if (Pos.Y> AppManager.Instance.GameManager.GetPlayer1.Pos.Y)
            {
                velocity.Y = -monster_speed;

            }
            else
            {
                velocity.Y = monster_speed;
            }
            if (Pos.X> AppManager.Instance.GameManager.GetPlayer1.Pos.X)
            {
                velocity.X = -monster_speed;
            }
            else
            {
                velocity.X = monster_speed;
            }
        }

        public override void Target()
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(gameTime);
            
            GraphicsComponent.Update();
        }
        public void TakeDamage()
        {
            monster_health--;
            GraphicsComponent.StartAnimation("SilasHandMove");
            Particle particle = new Particle(Pos);
            if (monster_health <= 0)
            {
                Death();
            }
        }
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    Attack(null);

                }
            }
            base.OnCollision(gameObject);
        }
    }
}
