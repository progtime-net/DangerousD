using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Base;

namespace DangerousD.GameCore.GUI;

public abstract class AbstractGui : IGameObject
{
    protected UIManager Manager = new();
    protected List<DrawableUIElement> Elements = new();

    public AbstractGui()
    {
    }

    protected abstract void CreateUI();

    public virtual void Initialize(GraphicsDevice graphicsDevice)
    {
        Manager.Initialize("", graphicsDevice);
        CreateUI();
    }

    public virtual void LoadContent(ContentManager content)
    {
        Manager.LoadContent(content);
    }

    public virtual void Update(GameTime gameTime)
    {
        Manager.Update();
    }
        
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Manager.Draw(spriteBatch);
    }
}