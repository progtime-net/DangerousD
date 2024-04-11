using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects.Entities.Items;

public class    Coin : Entity
{
    public Coin(Vector2 position) : base(position)
    {
    }

    protected override GraphicsComponent GraphicsComponent { get; }
}