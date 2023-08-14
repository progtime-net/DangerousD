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
            Width = 32;
            Height = 32;
        }

        public override void Initialize(GraphicsDevice graphicsDevice)
        {
        }
        public override void LoadContent(ContentManager content)
        {
            graphicsComponent = new Graphics.GraphicsComponent(content.Load<Texture2D>("wall"));
            base.LoadContent(content); 
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
