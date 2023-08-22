using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class CollisionMapObject:MapObject
{
    public CollisionMapObject(Vector2 position, Vector2 size, Rectangle sourceRectangle) : base(position, size, sourceRectangle)
    {
    }
}