using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Network;
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
        if (AppManager.Instance.multiPlayerStatus != MultiPlayerStatus.SinglePlayer)
        {
            NetworkTask task = new NetworkTask(id, _pos);
            if (this is Player || AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host)
            {
                AppManager.Instance.NetworkTasks.Add(task);
            }
        }
        
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="animationName"></param>
    public virtual void StartCicycleAnimation(string animationName)
    {
        if (GraphicsComponent.GetCurrentAnimation != animationName)
        {
            GraphicsComponent.StartAnimation(animationName);
        }
    }
}