using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DangerousD.GameCore.GUI
{
    interface IGui
    {
        void Initialize(GraphicsDevice graphicsDevice);
        void LoadContent(ContentManager content);
        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}
