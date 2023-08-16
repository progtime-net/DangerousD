using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class Tile : MapObject
{
    public Tile(Vector2 position, Rectangle sourceRectangle) : base(position, sourceRectangle)
    {
        IsColliderOn = false;
    }
}