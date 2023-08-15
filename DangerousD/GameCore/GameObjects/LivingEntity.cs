using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects;

public abstract class LivingEntity : Entity
{
    private Vector2 targetPosition;
    public Vector2 velocity;
    public Vector2 acceleration;
    public LivingEntity(Vector2 position) : base(position)
    {
        acceleration = new Vector2(0, 10);
    }
    public void SetPosition(Vector2 position) { targetPosition = position; }

    public override void Update(GameTime gameTime)
    {
        if (Vector2.Distance(Pos, targetPosition) > 0.5f)
        {
            Vector2 dir = targetPosition - Pos;
            dir.Normalize();
            Pos += dir * velocity;
        }
        base.Update(gameTime);
    }
}