using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Base;

namespace DangerousD.GameCore.GUI;

public abstract class AbstractGui : IDrawableObject
{
    protected UIManager Manager = new();
    protected List<DrawableUIElement> Elements = new();

    public AbstractGui()
    {
    }

    protected abstract void CreateUI();
    private GraphicsDevice graphicsDevice;
    public virtual void Initialize(GraphicsDevice graphicsDevice)
    {
        Manager.Initialize(graphicsDevice);
        this.graphicsDevice = graphicsDevice;
        CreateUI();
    }

    public virtual void LoadContent()
    {
        Manager.LoadContent(AppManager.Instance.Content, "Font");
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