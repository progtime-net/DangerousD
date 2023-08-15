using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.Graphics;

namespace DangerousD.GameCore.GameObjects.MapObjects
{
    internal class GrassBlock : MapObject
    {
        protected override GraphicsComponent GraphicsComponent { get; } = new("wall");

        public GrassBlock(Vector2 position) : base(position)
        {
            Width = 32;
            Height = 32;
        }
    }
}
