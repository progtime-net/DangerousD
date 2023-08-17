using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public abstract class CoreEnemy : LivingEntity
    {
        protected int monster_health;
        protected int monster_speed;
        protected string name;
        protected bool isAlive = true;

        public CoreEnemy(Vector2 position) : base(position)
        {
            //здесь я не понял
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public abstract void Death();

        public abstract void Attack();

        public abstract void Move(GameTime gameTime);

        public void TakeDamage(int damage)
        {
            monster_health -= damage;
            if (monster_health <= 0)
            {
                Death();
                isAlive = false;
            }
        }
    }
}
