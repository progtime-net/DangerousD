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
    public virtual void Initialize()
    {
<<<<<<< HEAD
        Manager.Initialize(AppManager.Instance.GraphicsDevice);
=======
        Manager.Initialize(AppManager.Instance.GraphicsDevice); 
>>>>>>> ea55e2b4f2b2b9af627579f3c4b82bdf0171d80b
        CreateUI();
    }

    public virtual void LoadContent()
    {
        Manager.LoadContent(AppManager.Instance.Content, "Font2");
    }

    public virtual void Update(GameTime gameTime)
    {
        Manager.Update(gameTime);
    }
        
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Manager.Draw(spriteBatch);
    }
}