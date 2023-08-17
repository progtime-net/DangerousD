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
    internal class Frank : CoreEnemy
    {
        private Vector2 position;
        private bool isGoRight = false;

        public Vector2 Position
        {
            get { return position; }
        }

        public Frank(Vector2 position) : base(new Vector2(300, 200))
        {
            //position = new Vector2(300, 200);
            Width = 112;
            Height = 160;
            GraphicsComponent.StartAnimation("FrankMoveLeft");
            monster_speed = 3;
            name = "Frank";
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "FrankMoveRight", "FrankMoveLeft" }, "FrankMoveRight");

        public override void Attack()
        {

        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            var player = AppManager.Instance.GameManager.players[0];
            if (player.Pos.X - _pos.X <= 20 || player.Pos.X - _pos.X <= -20)
            {
                player.Death(name);
            } 
           
            if (isGoRight)
            {  
                if (GraphicsComponent.GetCurrentAnimation != "FrankMoveRight")
                {
                    GraphicsComponent.StartAnimation("FrankMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (GraphicsComponent.GetCurrentAnimation != "FrankMoveLeft")
                {
                    GraphicsComponent.StartAnimation("FrankMoveLeft");
                }
                velocity.X = -monster_speed;
            }
        }
    }
}
