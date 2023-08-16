using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class StopTile : MapObject
{
    public StopTile(Vector2 position) : base(position)
    {
        IsColliderOn = true;
    }

    protected override GraphicsComponent GraphicsComponent { get; }
}