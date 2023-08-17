using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.GameObjects;

public abstract class LivingEntity : Entity
{
    public bool isOnGround = true;
    public Vector2 velocity;
    public Vector2 acceleration;

    public LivingEntity(Vector2 position) : base(position)
    {
        acceleration = new Vector2(0, 30);
    }
    public override void SetPosition(Vector2 position)
    {
        _pos = position;
        
    } //TODO befrend targetpos and physics engine

    public override void Update(GameTime gameTime)
    {
        //if (Vector2.DistanceSquared(Pos, targetPosition) > 0.25f)
        //{
        //    Vector2 dir = targetPosition - Pos;
        //    dir.Normalize();
        //    _pos += dir * velocity;
        //}
        base.Update(gameTime);
    }

    public virtual void StartCicycleAnimation(string animationName)
    {
        if (GraphicsComponent.GetCurrentAnimation != animationName)
        {
            GraphicsComponent.StartAnimation(animationName);
        }
    }
}