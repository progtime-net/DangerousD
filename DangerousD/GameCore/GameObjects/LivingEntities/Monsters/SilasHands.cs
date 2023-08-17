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
    public class SilasHands : CoreEnemy
    {
        public SilasHands(Vector2 position) : base(position)
        {
            name = "SilasHand";
            Width = 48;
            Height = 48;
            monster_health = 2;
            monster_speed = 2;
            acceleration = Vector2.Zero;
            
        }

        protected override GraphicsComponent GraphicsComponent { get; }=new GraphicsComponent(new List<string>() { "SilasHandMove" }, "SilasHandMove");

        public override void Attack()
        {

        }

        public override void Attack(GameTime gameTime)
        {

        }

        public override void Death()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Move(gameTime);
            if ((Pos.X + 20 <= AppManager.Instance.GameManager.GetPlayer1.Pos.X || Pos.X - 20 >= AppManager.Instance.GameManager.GetPlayer1.Pos.X)&&(Pos.Y + 20 <= AppManager.Instance.GameManager.GetPlayer1.Pos.Y || Pos.Y - 20 >= AppManager.Instance.GameManager.GetPlayer1.Pos.Y))
            {
                
                AppManager.Instance.GameManager.GetPlayer1.Death(name);
            }
            GraphicsComponent.Update();
        }
    }
}
