using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace DangerousD.GameCore.GameObjects;

public abstract class MapObject : GameObject
{
    public bool IsColliderOn;
    private Rectangle _sourceRectangle;
    protected override GraphicsComponent GraphicsComponent { get; } = new("map");
    public MapObject(Vector2 position, Rectangle sourceRectangle) : base(position)
    {
        _sourceRectangle = sourceRectangle;
    }

    public override void Initialize()
    {
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        GraphicsComponent.DrawAnimation(Rectangle, spriteBatch, _sourceRectangle);
    }
}