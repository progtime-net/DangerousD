using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class Tile : MapObject
{
    public Tile(Vector2 position) : base(position)
    {
        IsColliderOn = false;
    }

    protected override GraphicsComponent GraphicsComponent { get; }
}