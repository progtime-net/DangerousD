using DangerousD.GameCore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Player : LivingEntity
    {
        protected override GraphicsComponent GraphicsComponent => throw new NotImplementedException();

        public void Kill()
        {

        }
    }
}
