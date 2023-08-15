using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Enums;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Content;

namespace MonogameLibrary.UI.Base
{
    public class DrawableUIElement
    {
        protected Texture2D texture;
        protected int layerIndex;
        protected UIManager Manager;
        protected string textureName;
        public Rectangle rectangle = new Rectangle(0, 0, 10, 10);
        public Color mainColor = Color.White;

        public DrawableUIElement(UIManager manager, int layerIndex = 0, string textureName = "")
        {
            Manager = manager;
            this.textureName = textureName;
            manager.Register(this, layerIndex);
        }
        public virtual void LoadTexture(ContentManager content)
        {
            if (textureName == "")
            {
                texture = new Texture2D(Manager.GraphicsDevice, 1, 1);
                texture.SetData<Color>(new Color[] { mainColor });
            }
            else
            {
                try
                {
                    texture = content.Load<Texture2D>(textureName);
                }
                catch
                {
                    texture = new Texture2D(Manager.GraphicsDevice, 1, 1);
                    texture.SetData<Color>(new Color[] { mainColor });
                }
            }
        }
        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, rectangle, mainColor);
        }
    }
}
