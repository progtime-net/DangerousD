using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities.Monsters
{
    class Knife : Entity
    {
        public Knife(Vector2 position) : base(position)
        {
        }

        protected override GraphicsComponent GraphicsComponent => throw new NotImplementedException();
    }
}
