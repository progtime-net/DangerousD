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
    public class Hunchman : CoreEnemy
    {
        public Hunchman(Vector2 position) : base(position)
        {
            Width = 72;
            Height = 72;
            monster_speed = 5;
            name = "HunchMan";
            velocity = new Vector2(monster_speed, 0);
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> 
            { "HunchmanMoveLeft", "HunchmanMoveRight", "HunchmanAttackLeft", "HunchmanAttackRight" }, "HunchmanMoveRight");

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
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
            if (velocity.X > 0)
            {
                if (GraphicsComponent.GetCurrentAnimation != "HunchmanMoveRight")
                    GraphicsComponent.StartAnimation("HunchmanMoveRight");
            }
            else if (velocity.X < 0)
            {
                if (GraphicsComponent.GetCurrentAnimation != "HunchmanMoveLeft")
                    GraphicsComponent.StartAnimation("HunchmanMoveRight");
            }
        }

        public override void OnCollision()
        {
            monster_speed *= -1;
        }
    }
}
