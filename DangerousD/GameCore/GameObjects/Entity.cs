using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore.GameObjects
{
    public abstract class Entity : GameObject
    {
        protected bool hasColidedLastUpdateCall = false;
        public Entity(Vector2 position) : base(position) {}
        public virtual void SetPosition(Vector2 position) {  _pos = position; } 
        
    }
}
