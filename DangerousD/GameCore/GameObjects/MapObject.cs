using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
namespace DangerousD.GameCore.GameObjects;

public abstract class MapObject : GameObject
{
    public virtual bool IsColliderOn { get; protected set; } = true;
    private Rectangle _sourceRectangle;
    protected override GraphicsComponent GraphicsComponent { get; } = new("tiles");
    public MapObject(Vector2 position, Vector2 size, Rectangle sourceRectangle) : base(position)
    {
        _sourceRectangle = sourceRectangle;
        Width = (int)size.X;
        Height = (int)size.Y;
    }

    public override void Initialize()
    {
        
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        GraphicsComponent.DrawAnimation(Rectangle, spriteBatch, _sourceRectangle);
        //spriteBatch.Draw(debugTexture, new Rectangle(Rectangle.X - GraphicsComponent.CameraPosition.X, Rectangle.Y - GraphicsComponent.CameraPosition.Y, Rectangle.Width, Rectangle.Height), Color.White);

    }
}