using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore.GameObjects
{
    class MapObject : GameObject
    {
        public MapObject(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }
    }
}
