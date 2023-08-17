using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
namespace DangerousD.GameCore.GameObjects;

public abstract class MapObject : GameObject
{
    public bool IsColliderOn;
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

    public void Draw(SpriteBatch spriteBatch)
    {
        GraphicsComponent.DrawAnimation(Rectangle, spriteBatch, _sourceRectangle);
    }
}