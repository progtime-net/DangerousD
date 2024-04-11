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
        protected int widthS;//del
        protected bool isDown;
        protected bool isDownUp;
        protected PhysicsManager physicsManager;
        protected Player player;
        protected Vector2 oldPosition;
        int spiderSectionLength = 16; //длина сегмента паутины
        int movingWidth = 28;
        int movingHeight = 6;
        bool canCrawl = true;
        public Spider(Vector2 position) : base(position)
        {
            player = AppManager.Instance.GameManager.players[0];
            isDownUp = true;
            isDown = true;
            physicsManager = AppManager.Instance.GameManager.physicsManager;
            name = "Spider";
            Width = movingWidth;
            Height = movingHeight; 
            web = new SpiderWeb(new Vector2(Pos.X+Width/2,Pos.Y));
            delay = 0;
            webLength = 0;
            monster_speed = 3;
            acceleration = new Vector2(0, -50);
            isGoRight = true;
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SpiderMoveRight", "SpiderMoveLeft", "SpiderOnWeb" }, "SpiderMoveRight");

        public override void Update(GameTime gameTime)
        {
            if (isAttack == false)
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
        /// Атака паука РАБОЧАЯ
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Attack(GameTime gameTime)
        {
            if (isAttack)
            {
                velocity.X = 0;
                delay += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (delay > 0.25 && webLength <= 4 && isDown)
                {
                    Width = 12;
                    Height = 18;
                    StartCicycleAnimation("SpiderOnWeb");
                    acceleration = Vector2.Zero;
                    webLength++;
                    _pos.Y += spiderSectionLength;
                    web.Height = webLength * spiderSectionLength; //подгон
                    web.SetPosition(new Vector2(_pos.X + Width / 2 - web.Width / 2, Pos.Y - spiderSectionLength * webLength));
                    delay = 0;
                    if (webLength == 4)
                    {
                        isDown = false;
                    }
                }
                else if (delay > 0.5 && webLength != 0 && !isDown)
                {
                    Width = 12;
                    Height = 18;
                    StartCicycleAnimation("SpiderOnWeb");
                    webLength--;
                    _pos.Y -= spiderSectionLength;
                    web.Height = webLength * spiderSectionLength;
                    web.SetPosition(new Vector2(_pos.X + Width / 2 - web.Width / 2, Pos.Y - spiderSectionLength * webLength));
                    delay = 0;
                    if (webLength == 0)
                    {
                        isDown = true;
                    }
                }
                if (webLength == 0)
                {
                    isAttack = false;
                }
                var entities = physicsManager.CheckRectangle(new Rectangle((int)Pos.X, (int)Pos.Y, Width, Height + 200),typeof(CollisionMapObject));
                foreach (var entity in entities)
                {
                    if (webLength == 4 && entity is Player)
                    {
                        AppManager.Instance.GameManager.players[0].Death(name);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (AppManager.Instance.InputManager.CollisionsCheat)
                DrawDebugRectangle(spriteBatch, new Rectangle((int)Pos.X + Width, (int)Pos.Y, 30, 10), color: Color.Blue);
            if (AppManager.Instance.InputManager.CollisionsCheat)
                DrawDebugRectangle(spriteBatch, new Rectangle((int)Pos.X - 30, (int)Pos.Y, 30, 10), color: Color.Blue);
            base.Draw(spriteBatch); 
        }

        public override void Death()
        {

            for (int i = 0; i < 1; i++)
            {
                Particle particle = new Particle(Pos);
            }

            AppManager.Instance.GameManager.Remove(this);
            AppManager.Instance.GameManager.Remove(web);
        }

        public override void Move(GameTime gameTime)
        {
            Width = movingWidth;
            Height = movingHeight;
            if (isGoRight)
            {
                if (physicsManager.CheckRectangle(new Rectangle((int)Pos.X + Width, (int)Pos.Y, 30, 10), typeof(CollisionMapObject)).Where(entity =>
                entity.GetType() == typeof(StopTile)).Count() > 0)
                {
                    isGoRight = false;
                }
            }
            else if (!isGoRight)
            {
                if (physicsManager.CheckRectangle(new Rectangle((int)Pos.X - 30, (int)Pos.Y, 30, 10), typeof(CollisionMapObject)).Where(entity =>
                entity.GetType() == typeof(StopTile)).Count() > 0)
                {
                    isGoRight = true;
                }
            }
            if (isGoRight)
            {
                StartCicycleAnimation("SpiderMoveRight"); 
                velocity.X = monster_speed;
            }
            else
            {
                StartCicycleAnimation("SpiderMoveLeft"); 
                velocity.X = -monster_speed;
            }
        }

        public override void Target()
        {
            if (player.Pos.X + player.Width *2/3.0 >= Pos.X && player.Pos.X + player.Width * 1 / 3.0 <= Pos.X+Width)
            {
                isAttack = true;
            }
        }

        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    AppManager.Instance.GameManager.players[0].Death(name);
                }
            }
            base.OnCollision(gameObject);
        }
    }
}
