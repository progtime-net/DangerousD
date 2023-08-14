using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore
{
    static class TextureManager
    {
        public static ContentManager contentManager;
        public static GraphicsDevice graphicsDevice;
        public static SpriteBatch spriteBatch;
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public static Texture2D nullTexture;
        public static void Init(GraphicsDevice _graphicsDevice, ContentManager _contentManager, SpriteBatch _spriteBatch)
        {
            graphicsDevice = _graphicsDevice;
            contentManager = _contentManager;
            spriteBatch = _spriteBatch;
            nullTexture = new Texture2D(graphicsDevice, 1, 1);
            nullTexture.SetData(new Color[] { Color.Purple , Color.Black, Color.Purple, Color.Black });
        }
        public static Texture2D GetTexture(string textureName)
        {
            if (textures.ContainsKey(textureName))
                return textures[textureName];
            try
            {
                Texture2D loadedTexture = contentManager.Load<Texture2D>(textureName);
                textures.Add(textureName, loadedTexture);
            }
            catch
            {
            }
            return nullTexture;

        }

    }
}
