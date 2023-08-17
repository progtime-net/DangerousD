﻿using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects.PlayerDeath;
using Microsoft.Xna.Framework.Input;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Player : LivingEntity
    {
        bool isAlive = true;
        public int health;
        public Player(Vector2 position) : base(position)
        {
            Width = 32;
            Height = 64;
<<<<<<< HEAD
            AppManager.Instance.InputManager.MovEventJump += Jump;
            AppManager.Instance.InputManager.ShootEvent += Shoot;

=======
            AppManager.Instance.InputManager.MovEventJump += AnimationJump;
>>>>>>> 833da68a4e42a47ab035a220c049aa9937eb1969
        }
        public bool IsAlive { get { return isAlive; } }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "ZombieMoveRight", "ZombieMoveLeft", "ZombieRightAttack", "ZombieLeftAttack", "DeathFromZombie" }, "ZombieMoveLeft");//TODO: Change to player

        public void Kill()
        {

        }

        public void Death(string monsterName)
        {
            if(monsterName == "Zombie")
            {
                DeathRectangle deathRectangle = new DeathRectangle(Pos, "DeathFrom" + monsterName);
                deathRectangle.Gr.actionOfAnimationEnd += (a) =>
                {
                    if (a == "DeathFrom" + monsterName)
                    {
                        AppManager.Instance.ChangeGameState(GameState.GameOver);
                    }
                };
            }
            isAlive = false;
        }
        public void Jump()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                velocity.Y = -300;
            }
            // здесь будет анимация
        }
        public void Shoot()
        {

        }
        public override void Update(GameTime gameTime)
        {
            GraphicsComponent.CameraPosition = (_pos-new Vector2(200, 350)).ToPoint();
            velocity.X = 0.5f;
            base.Update(gameTime);
        }
    }
}
