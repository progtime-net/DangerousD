using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.Entities.Items;

public class Diamond : Entity
{
    public Diamond(Vector2 position) : base(position)
    {
    }

    protected override GraphicsComponent GraphicsComponent { get; }
}