using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.MapObjects;

public class Tile : MapObject
{
    public override bool IsColliderOn { get; protected set; } = false;
    public Tile(Vector2 position, Vector2 size, Rectangle sourceRectangle) : base(position, size, sourceRectangle)
    { 
    }
}