﻿using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.Entities
{
    public class SilasBall : LivingEntity
    {
        public SilasBall(Vector2 position) : base(position)
        {
            Height = 60;
            Width = 60;
            acceleration = Vector2.Zero;
            
        }
        public SilasBall(Vector2 position, Vector2 velosity) : base(position)
        {
            Height = 60;
            Width = 60;
            acceleration = Vector2.Zero;
            velocity = velosity;
            
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "SilasBallMove" }, "SilasBallMove");
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}