using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Enums;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace MonogameLibrary.UI.Base
{
    public class MonoDrawableUI
    {
        protected Texture2D texture;
        protected int layerIndex;
        public Rectangle rectangle = new Rectangle(0, 0, 10, 10);
        public Color mainColor = Color.White;
        public MonoDrawableUI(MonoClassManagerUI MyUIManager = null, int layerIndex = 0)
        {
            MyUIManager.Register(this, layerIndex);
        }
        public void LoadTexture(string textureName)
        {
            if (textureName == "")
            {
                texture = new Texture2D(MonoClassManagerUI.MainGraphicsDevice, 1, 1);
                texture.SetData<Color>(new Color[] { Color.White });
            }
            else
            {
                try
                {
                    texture = MonoClassManagerUI.MainContent.Load<Texture2D>(textureName);
                }
                catch
                {
                    texture = new Texture2D(MonoClassManagerUI.MainGraphicsDevice, 1, 1);
                    texture.SetData<Color>(new Color[] { Color.White });
                }
            }
        }
        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, rectangle, mainColor);
        }
    }
}
