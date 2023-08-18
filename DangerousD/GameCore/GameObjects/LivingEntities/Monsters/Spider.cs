using DangerousD.GameCore.GameObjects.MapObjects;
using DangerousD.GameCore.Graphics;
using DangerousD.GameCore.Managers;
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
        private bool isAttack;

        protected SpiderWeb web;
        protected float delay;
        protected int webLength;
        protected bool isDown;
        protected bool isDownUp;
        protected PhysicsManager physicsManager;
        protected Player player;

        public Spider(Vector2 position) : base(position)
        {
            player = AppManager.Instance.GameManager.players[0];
            isDownUp = true;
            isDown = true;
            physicsManager = AppManager.Instance.GameManager.physicsManager;
            name = "Spider";
            Width = 112;
            Height = 24;
            delay = 0;
            web = new SpiderWeb(new Vector2(Pos.X-Width/2,Pos.Y));
            webLength = 0;
            monster_speed = 3;
            acceleration = new Vector2(0, -50);
            isGoRight = true;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SpiderMoveRight", "SpiderMoveLeft", "SpiderOnWeb" }, "SpiderMoveRight");

        public override void Update(GameTime gameTime)
        {
            if (!isAttack)
            {
                Move(gameTime);
            }
            else
            {
                Attack(gameTime);
            }
            Target();

            base.Update(gameTime);
        }
        /// <summary>
        /// НИЧЕГО НЕ ДЕЛАЕТ! НУЖЕН ДЛЯ ПЕРЕОПРЕДЕЛЕНИЯ 
        /// </summary>
        public override void Attack()
        {
        }
        /// <summary>
        /// Атака паука РАБОЧАЯ
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Attack(GameTime gameTime)
        { //48 72
            velocity.X = 0;
            delay += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (isAttack)
            {
                if (delay > 0.5 && webLength <= 4 && isDown)
                {
                    Width = 48;
                    Height = 72;
                    StartCicycleAnimation("SpiderOnWeb");
                    acceleration = Vector2.Zero;
                    webLength++;
                    _pos.Y += 25;
                    web.Height = webLength * 25;
                    web.SetPosition(new Vector2(_pos.X + Width / 2 - web.Width / 2, Pos.Y - 25 * webLength));
                    delay = 0;
                    if (webLength == 4)
                    {
                        isDown = false;
                    }
                }
                else if (delay > 0.5 && webLength != 0 && !isDown)
                {
                    Width = 48;
                    Height = 72;
                    StartCicycleAnimation("SpiderOnWeb");
                    webLength--;
                    _pos.Y -= 25;
                    web.Height = webLength * 25;
                    web.SetPosition(new Vector2(_pos.X + Width / 2 - web.Width / 2, Pos.Y - 25 * webLength));
                    delay = 0;
                    if (webLength == 0)
                    {
                        isDown = true;
                    }
                }
                var entitiesInter = physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y, 200, 600));
                if (entitiesInter.Count > 0)
                {
                    foreach (var entity in entitiesInter)
                    {
                        if (entity.GetType() == typeof(Player))
                        {
                            player.Death(name);
                        }
                    }
                }
            }
            if (webLength == 0)
            {
                isAttack = false;
            }
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
            Width = 112;
            Height = 24;
            foreach (var entity in physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 7, (int)Pos.Y, 126, 10)))
            {
                if (entity.GetType() == typeof(StopTile))
                {
                    isGoRight = !isGoRight;
                }
            }
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "SpiderMoveRight")
                {
                    GraphicsComponent.StartAnimation("SpiderMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (GraphicsComponent.GetCurrentAnimation != "SpiderMoveLeft")
                {
                    GraphicsComponent.StartAnimation("SpiderMoveLeft");
                }
                velocity.X = -monster_speed;
            }
        }

        public void Target()
        {
            if (player.Pos.X >= Pos.X && player.Pos.X <= Pos.X+Width)
            {
                isAttack = true;
            }
        }
    }
}
