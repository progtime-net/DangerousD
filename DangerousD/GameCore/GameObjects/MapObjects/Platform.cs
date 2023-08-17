using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class Platform : MapObject
{
    public override bool IsColliderOn { get; protected set; } = true;
    public Platform(Vector2 position, Vector2 size, Rectangle sourceRectangle) : base(position, size, sourceRectangle)
    {
    }

}