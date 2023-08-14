using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects;

internal abstract class LivingEntity : Entity
{
    public LivingEntity(Vector2 position) : base(position)
    {
    }
}