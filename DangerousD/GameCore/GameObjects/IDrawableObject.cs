using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore.GUI
{
    interface IDrawableObject
    {
        void Initialize(GraphicsDevice graphicsDevice);
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
