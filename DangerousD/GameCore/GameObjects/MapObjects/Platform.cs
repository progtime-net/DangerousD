using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class Platform : MapObject
{
    public Platform(Vector2 position, string texture) : base(position)
    {
        IsColliderOn = true;
    }

    protected override GraphicsComponent GraphicsComponent { get; }
}