﻿using DangerousD.GameCore.Graphics;
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
    public class Zombie : CoreEnemy
    {
        private bool isGoRight = true;
        int leftBorder;
        int rightBorder;
        public Zombie(Vector2 position) : base(position)
        {
            Width = 72;
            Height = 120;
            monster_speed = 10;
            GraphicsComponent.StartAnimation("ZombieLeftAttack");
            name = "Zombie";
            leftBorder = (int)position.X;
            rightBorder = (int)position.X + 200;
        }
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack" }, "ZombieMoveLeft");

        public override void Update(GameTime gameTime)
        {
            if (AppManager.Instance.GameManager.GetPlayer1.Pos.X>Pos.X) 
                isGoRight = true; 
            else
                isGoRight = false;
            Move(gameTime);

            if(Pos.X + 20 <= AppManager.Instance.GameManager.GetPlayer1.Pos.X || Pos.X - 20 >= AppManager.Instance.GameManager.GetPlayer1.Pos.X)
            {
                Attack();
                AppManager.Instance.GameManager.GetPlayer1.Death(name);
            }

            base.Update(gameTime);
        }

        public override void Attack()
        {
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieRightAttack")
                    GraphicsComponent.StartAnimation("ZombieRightAttack"); 
                AppManager.Instance.GameManager.players[0].Death(name);
            }
            else if (!isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieLeftAttack")
                    GraphicsComponent.StartAnimation("ZombieLeftAttack"); 
                AppManager.Instance.GameManager.players[0].Death(name);
            }
        }

        public override void Death()
        {

        }

        public override void Move(GameTime gameTime)
        {
            double delta = gameTime.ElapsedGameTime.TotalSeconds;
            if (isGoRight)
            {
                if (GraphicsComponent.GetCurrentAnimation != "ZombieMoveRight")
                    GraphicsComponent.StartAnimation("ZombieMoveRight");
                velocity.X = monster_speed;
            }

            else if (!isGoRight)
            {
                if(GraphicsComponent.GetCurrentAnimation != "ZombieMoveLeft")
                    GraphicsComponent.StartAnimation("ZombieMoveLeft");
                velocity.X = -monster_speed;
            }
        }

        public void TakeDamage(int damage)
        {
            monster_health -= damage;
            //play take damage animation
        }
    }
}