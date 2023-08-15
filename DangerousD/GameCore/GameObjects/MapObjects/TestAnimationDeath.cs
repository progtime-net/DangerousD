using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.MapObjects
{
    internal class TestAnimationDeath : Entity
    {
        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "death1", "deathbear" },"death1");

        public TestAnimationDeath(Vector2 position) : base(position)
        {
            Width =512;
            Height = 512;

            GraphicsComponent.StartAnimation("deathbear");

        } 
    }
}
