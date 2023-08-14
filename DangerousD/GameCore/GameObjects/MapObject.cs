using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects;

internal abstract class MapObject : GameObject
{
    public MapObject(Vector2 position) : base(position)
    {
    }
}