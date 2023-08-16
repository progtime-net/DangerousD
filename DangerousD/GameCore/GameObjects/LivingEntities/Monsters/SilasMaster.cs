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
    public class SilasMaster : CoreEnemy
    {
        public SilasMaster(Vector2 position) : base(position)
        {

        }
        protected override GraphicsComponent GraphicsComponent => throw new NotImplementedException();

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public override void Death()
        {
            throw new NotImplementedException();
        }

        public override void Move(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
