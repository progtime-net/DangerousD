using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects.PlayerDeath;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Player : LivingEntity
    {
        bool isAlive = true;
        public Player(Vector2 position) : base(position)
        {
            Width = 32;
            Height = 64;
            AppManager.Instance.InputManager.MovEventJump += AnimationJump;
            
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
        public void AnimationJump()
        {
            velocity.Y = -300;
        }
        public override void Update(GameTime gameTime)
        {
            GraphicsComponent.CameraPosition = (_pos-new Vector2(200, 350)).ToPoint();
            velocity.X = 0.5f;
            base.Update(gameTime);
        }
    }
}
