using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.MapObjects
{
    internal class GrassBlock : MapObject
    {
        public GrassBlock(Vector2 position) : base(position)
        {
        }

        public override void Initialize(GraphicsDevice graphicsDevice)
        {
            
        }
        public override void LoadContent(ContentManager content)
        {
           // graphicsComponent = new Graphics.GraphicsComponent();
            base.LoadContent(content); 
        }

        public override void Update(GameTime gameTime)
        { 
        }
    }
}
