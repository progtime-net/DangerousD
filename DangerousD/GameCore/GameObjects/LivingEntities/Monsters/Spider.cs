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
    public class Spider : CoreEnemy
    {
        protected SpiderWeb web;
        protected float delay;
        protected int webLength;
        protected bool isDown;
        protected bool isDownUp;
        public Spider(Vector2 position) : base(position)
        {
            isDownUp = true;
            isDown = true;
            web = new SpiderWeb(Pos);
            name = "Spider";
            Width = 112;
            Height = 24;
            delay = 0;
            webLength = 0;
            monster_speed = 1;
            acceleration = Vector2.Zero;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SpiderMoveRight", "SpiderMoveLeft", "SpiderOnWeb" }, "SpiderMoveRight");

        public override void Update(GameTime gameTime)
        {
            if (isDownUp)
            {
                Width = 48;
                Height = 72;
                delay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (delay > 0.5 && webLength <= 4 && isDown)
                {
                    StartCicycleAnimation("SpiderOnWeb");
                    webLength++;
                    _pos.Y += 25;
                    web.Height = webLength * 25;
                    web.SetPosition(new Vector2(_pos.X + Width / 2 - web.Width / 2 + 2, Pos.Y - 25 * webLength));
                    delay = 0;
                    if (webLength == 4)
                    {
                        isDown = false;
                    }
                }
                else if (delay > 0.5 && webLength != 0 && !isDown)
                {
                    StartCicycleAnimation("SpiderOnWeb");
                    webLength--;
                    _pos.Y -= 25;
                    web.Height = webLength * 25;
                    web.SetPosition(new Vector2(_pos.X + Width / 2 - web.Width / 2 + 2, Pos.Y - 25 * webLength));
                    delay = 0;
                    if (webLength == 0)
                    {
                        isDown = true;
                    }
                }
                else
                {
                    Width = 112;
                    Height = 24;
                }
            }

            base.Update(gameTime);
        }
        public override void Attack()
        { //48 72
           
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (GraphicsComponent.GetCurrentAnimation == "SpiderOnWeb")
            {     
                GraphicsComponent.DrawAnimation(new Rectangle((int)Pos.X, (int)Pos.Y, 48, 72), spriteBatch);
                
            }
            else
            {
                base.Draw(spriteBatch);
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
