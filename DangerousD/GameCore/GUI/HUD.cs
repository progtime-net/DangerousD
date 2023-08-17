using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Elements;

namespace DangerousD.GameCore.GUI
{
	public class HUD : IDrawableObject
	{
        

		public HUD()
		{
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            ;
        }

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LoadContent()
        {
            var content = AppManager.Instance.Content;

        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}

