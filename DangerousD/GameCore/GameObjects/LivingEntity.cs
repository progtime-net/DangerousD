using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects;

public abstract class LivingEntity : Entity
{
    public LivingEntity(Vector2 position) : base(position)
    {
    }
}