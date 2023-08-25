using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using DangerousD.GameCore.Network;
using DangerousD.GameCore.GameObjects.MapObjects;

namespace DangerousD.GameCore.GameObjects.LivingEntities
{
    public class Bullet : LivingEntity
        {
            public Bullet(Vector2 position) : base(position)
            {
                Height = 5;
                Width = 5;
            }
            int time = 0;   
            protected override GraphicsComponent GraphicsComponent { get; } = new(new List<string> { "playerMoveLeft" }, "playerMoveLeft");
            Vector2 direction;
            public Vector2 maindirection;
            public void ShootUpRight()
            {
                direction = new Vector2(1, -1);
                acceleration = Vector2.Zero;
                velocity = new Vector2(10, 10) * direction;
                maindirection = velocity;
                if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Client)
                {
                    NetworkTask task = new NetworkTask(typeof(Bullet), Pos, id, velocity);
                    AppManager.Instance.NetworkTasks.Add(task);
                    AppManager.Instance.GameManager.Remove(this);
                }
            }
            public void ShootRight()
            {
                direction = new Vector2(1, 0);
                acceleration = Vector2.Zero;
                velocity = new Vector2(10, 10) * direction;
                maindirection = velocity;
                if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Client)
                {
                    NetworkTask task = new NetworkTask(typeof(Bullet), Pos, id, velocity);
                    AppManager.Instance.NetworkTasks.Add(task);
                    AppManager.Instance.GameManager.Remove(this);
                }
            }
            public void ShootLeft()
            {
                direction = new Vector2(-1, 0);
                acceleration = Vector2.Zero;
                velocity = new Vector2(10, 10) * direction;
                maindirection = velocity;
                if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Client)
                {
                    NetworkTask task = new NetworkTask(typeof(Bullet), Pos, id, velocity);
                    AppManager.Instance.NetworkTasks.Add(task);
                    AppManager.Instance.GameManager.Remove(this);
                }
            }
            public void ShootUpLeft()
            {
                direction = new Vector2(-1, -1);
                acceleration = Vector2.Zero;
                velocity = new Vector2(10, 10) * direction;
                maindirection = velocity;
                if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Client)
                {
                    NetworkTask task = new NetworkTask(typeof(Bullet), Pos, id, velocity);
                    AppManager.Instance.NetworkTasks.Add(task);
                    AppManager.Instance.GameManager.Remove(this);
                }
            }
            public override void OnCollision(GameObject gameObject)
            {
                if (gameObject is not Player)
                {
                    if (gameObject is CoreEnemy)
                    {
                        CoreEnemy enemy = (CoreEnemy)gameObject;
                        enemy.TakeDamage();
                        AppManager.Instance.GameManager.Remove(this);
                       
                    }

                    if (gameObject is StopTile)
                    {
                        AppManager.Instance.GameManager.Remove(this);
                        
                    }
                    base.OnCollision(gameObject);
                }
            }
            public override void Update(GameTime gameTime)
            {
                base.Update(gameTime);
                
            }
        }
}