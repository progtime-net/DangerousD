using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class StopTile : MapObject
{
    public StopTile(Vector2 position, Rectangle sourceRectangle) : base(position, sourceRectangle)
    {
        IsColliderOn = true;
    }

    protected override GraphicsComponent GraphicsComponent { get; }
}