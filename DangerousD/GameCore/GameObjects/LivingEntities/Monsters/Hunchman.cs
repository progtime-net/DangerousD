using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.Managers;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    public class Hunchman : CoreEnemy
    {
        private bool isGoRight = false;
        private bool isAttaking = false;
        private bool isTarget = false;
        private bool isVisible = true;
        float leftBoarder;
        float rightBoarder;

        PhysicsManager physicsManager;
        public Hunchman(Vector2 position) : base(position)
        {
            Width = 20;
            Height = 30;
            leftBoarder = (int)position.X - 100;
            rightBoarder = (int)position.X + 100;
            name = "Hunchman";
            GraphicsComponent.StartAnimation("HunchmanMoveLeft");
            monster_speed = 3;
            physicsManager = new PhysicsManager();
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string>
            { "HunchmanMoveLeft", "HunchmanMoveRight", "HunchmanDaggerLeft", "HunchmanDaggerRight" }, "HunchmanMoveLeft");


        public override void Update(GameTime gameTime)
        {
            if (!isAttaking)
            {
                Target();
                Move(gameTime);
            }
            base.Update(gameTime);

        }

        public override void Attack()
        {
            var animation = GraphicsComponent.GetCurrentAnimation;
            isAttaking = true;
            if (isGoRight)
            {
                if (animation != "HunchmanDaggerRight")
                {
                    GraphicsComponent.StartAnimation("HunchmanDaggerRight");
                }
            }
            else
            {
                if (animation != "HunchmanDaggerLeft")
                {
                    GraphicsComponent.StartAnimation("HunchmanDaggerLeft");
                }
            }
        }

        public override void Attack(GameTime gameTime)
        {}

        public override void Death()
        {
            for (int i = 0; i < 3; i++)
            {
                Particle particle = new Particle(Pos);
            }

            if (monster_health <= 0)
            {
                isVisible = false;
            }
        }

        public override void Move(GameTime gameTime)
        {
            velocity.X = 0;
            var animation = GraphicsComponent.GetCurrentAnimation;
            if (isGoRight)
            {
                if (animation != "HunchmanMoveRight")
                {
                    GraphicsComponent.StartAnimation("HunchmanMoveRight");
                }
                velocity.X = monster_speed;
            }
            else
            {
                if (animation != "HunchmanMoveLeft")
                {
                    GraphicsComponent.StartAnimation("HunchmanMoveLeft");
                }
                velocity.X = -monster_speed;
            }

            if (_pos.X >= rightBoarder)
            {
                isGoRight = false;
            }
            else if (_pos.X <= leftBoarder)
            {
                isGoRight = true;
            }
        }

        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject is Player)
            {
                if (AppManager.Instance.GameManager.players[0].IsAlive)
                {
                    Attack();
                }
            }

            base.OnCollision(gameObject);
        }

        public void Target()
        {
            var player = AppManager.Instance.GameManager.players[0];

            if (physicsManager.RayCast(this, player) == null)
            {
                if(this._pos.X <= player.Pos.X)
                {
                    isTarget = true;
                    leftBoarder = Pos.X - 10;
                    rightBoarder = player.Pos.X;
                }
                else if(this._pos.X >= player.Pos.X)
                {
                    isTarget = true;
                    rightBoarder = Pos.X + 10;
                    leftBoarder = player.Pos.X;
                }
            }
        }
    }
}