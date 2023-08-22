using DangerousD.GameCore.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.GameObjects.MapObjects;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.Managers
{
    public class PhysicsManager
    {

        public void UpdateCollisions(List<Entity> entities, List<LivingEntity> livingEntities,
            List<MapObject> mapObjects, List<Player> players, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var item in livingEntities)
            {
                item.velocity = item.velocity + item.acceleration * delta;
            }

            CheckCollisionsLE_MO(livingEntities, mapObjects.Where(mo => mo is StopTile).ToList());
            CheckCollisionsPlayer_Platform(players, mapObjects.OfType<Platform>().ToList());
            CheckCollisionsE_LE(entities, livingEntities);
            CheckCollisionsLE_LE(livingEntities);

            //entities dont move
            //Living entities dont move
            //mapObjects dont move
            //mapObjects have isCollisions on
            //Check collisions
            //Only Living entities move
            //OnCollision

        }
        private void CheckCollisionsLE_MO(List<LivingEntity> livingEntities,
            List<MapObject> mapObjects)
        {
            foreach (var currentEntity in livingEntities.ToList())
            {
                var currentRect = currentEntity.Rectangle;
                var newRect = currentRect;

                #region x collision
                var collidedX = false;
                var tryingRectX = currentRect;
                tryingRectX.Offset((int)Math.Ceiling(currentEntity.velocity.X), 0);
                foreach (var mapObject in mapObjects)
                {
                    if (
                        Math.Abs(mapObject.Pos.X - currentEntity.Pos.X) < 550 
                        && Math.Abs(mapObject.Pos.Y - currentEntity.Pos.Y) < 550
                        && tryingRectX.Intersects(mapObject.Rectangle)
                    )
                    {
                        collidedX = true;
                        break;
                    }
                }
                if (collidedX)
                {
                    currentEntity.velocity.X = 0;
                }
                else
                {
                    newRect.X = tryingRectX.X;
                }
                #endregion
                
                #region y collision
                var collidedY = false;
                var tryingRectY = currentRect;
                tryingRectY.Offset(0, (int)Math.Ceiling(currentEntity.velocity.Y));
                if (currentEntity is Player)
                {
                    AppManager.Instance.DebugHUD.Set("velocity", currentEntity.velocity.ToString());
                    AppManager.Instance.DebugHUD.Set("falling", (currentEntity as Player).FallingThroughPlatform.ToString());
                    AppManager.Instance.DebugHUD.Set("intersects y", "");
                }
                foreach (var mapObject in mapObjects)
                {
                    if (tryingRectY.Intersects(mapObject.Rectangle))
                    {
                        if (currentEntity is Player) AppManager.Instance.DebugHUD.Set("intersects y", mapObject.GetType().ToString());
                        collidedY = true;
                        break;
                    }
                }
                currentEntity.isOnGround = collidedY && currentEntity.velocity.Y > 0;
                if (collidedY)
                {
                    currentEntity.velocity.Y = 0;
                }
                else
                {
                    newRect.Y = tryingRectY.Y;
                }
                #endregion

                currentEntity.SetPosition(new Vector2(newRect.X, newRect.Y));
            }

        }
        private void CheckCollisionsPlayer_Platform(List<Player> players, List<Platform> platforms)
        {
            foreach (var player in players)
            {
                if (player.velocity.Y <= 0 || player.FallingThroughPlatform)
                {
                    continue;
                }
                var currentRect = player.Rectangle;
                var newRect = currentRect;
                
                var collidedY = false;
                var tryingRectY = currentRect;
                tryingRectY.Offset(0, (int)Math.Ceiling(player.velocity.Y));
                AppManager.Instance.DebugHUD.Set("intersects platform", "false");
                foreach (var platform in platforms)
                {
                    if (tryingRectY.Intersects(platform.Rectangle))
                    {
                        AppManager.Instance.DebugHUD.Set("intersects platform", "true");
                        collidedY = true;
                        break;
                    }
                }
                if (collidedY)
                {
                    // костыль потому что в CheckCollisionsLE_MO он спускается
                    newRect.Y -= (int)Math.Ceiling(player.velocity.Y);
                    player.isOnGround = true;
                    player.velocity.Y = 0;
                }

                player.SetPosition(new Vector2(newRect.X, newRect.Y));
            }

        }
        private void CheckCollisionsE_LE(List<Entity> entities, List<LivingEntity> livingEntities)
        {
            for (int i = 0; i < entities.Count; i++)
            {


                for (int j = 0; j < livingEntities.Count; j++)
                {

                
                    if (livingEntities[j].Rectangle.Intersects(entities[i].Rectangle))
                    {
                        livingEntities[j].OnCollision(entities[i]);
                        entities[i].OnCollision(livingEntities[j]);
                    }
                }
            }
        }
        private void CheckCollisionsLE_LE(List<LivingEntity> livingEntities)
        {
            for (int i = 0; i < livingEntities.Count; i++)
            {
                for (int j = i + 1; j < livingEntities.Count; j++)
                {
                    if (livingEntities[i].Rectangle.Intersects(livingEntities[j].Rectangle))
                    {
                        livingEntities[i].OnCollision(livingEntities[j]);
                        livingEntities[j].OnCollision(livingEntities[i]);
                    }
                }
            }
        }


        public GameObject RayCast(LivingEntity entity1, LivingEntity entity2)
        {

            Rectangle rectangle;
            Vector2 distance = entity1.Pos - entity2.Pos;
            rectangle = new Rectangle((int)entity1.Pos.X, (int)entity1.Pos.Y - 5, entity2.Width, entity2.Height);
            GameObject gameObject = null;
            double length = distance.Length();

            for (int i = 0; i < length; i++)
            {
                rectangle.X = (int)(entity2.Pos.X + (i / length) * distance.X);
                rectangle.Y = (int)(entity2.Pos.Y + (i / length) * distance.Y);
                if (i == length - 1)
                {
                    return null;
                }
                for (int j = 0; j < AppManager.Instance.GameManager.entities.Count; j++)
                {
                    if (AppManager.Instance.GameManager.entities[j].Rectangle.Intersects(rectangle))
                    {
                        gameObject = AppManager.Instance.GameManager.entities[j];
                    }
                }
                for (int r = 0; r < AppManager.Instance.GameManager.mapObjects.Count; r++)
                {
                    if (AppManager.Instance.GameManager.mapObjects[r].Rectangle.Intersects(rectangle))
                    {
                        gameObject = AppManager.Instance.GameManager.mapObjects[r];
                    }
                }
                for (int w = 0; w < AppManager.Instance.GameManager.livingEntities.Count; w++)
                {
                    if (AppManager.Instance.GameManager.livingEntities[w].Rectangle.Intersects(rectangle))
                    {
                        gameObject = AppManager.Instance.GameManager.livingEntities[w];
                    }
                }
            }

            if (gameObject == entity1)
            {
                return null;
            }
            return gameObject;
        }
        
        public GameObject RayCast(LivingEntity entity1, Vector2 targetCast)
        {
            Rectangle rectangle;
            Vector2 direction = entity1.Pos - targetCast;
            rectangle = new Rectangle((int)targetCast.X, (int)targetCast.Y, 1, 1);
            GameObject gameObject = null;
            double k = direction.Length();

            for (int i = 0; i < k; i++)
            {
                rectangle.X = (int)(targetCast.X + (i / k) * direction.X);
                rectangle.Y = (int)(targetCast.Y + (i / k) * direction.X);
                for (int j = 0; j < AppManager.Instance.GameManager.entities.Count; j++)
                {
                    if (AppManager.Instance.GameManager.entities[j].Rectangle.Intersects(rectangle))
                    {
                        gameObject =  AppManager.Instance.GameManager.entities[j];
                    }
                }
                for (int r = 0; r < AppManager.Instance.GameManager.mapObjects.Count; r++)
                {
                    if (AppManager.Instance.GameManager.mapObjects[r].Rectangle.Intersects(rectangle))
                    {
                       gameObject = AppManager.Instance.GameManager.mapObjects[r];
                    }
                }
                for (int w = 0; w < AppManager.Instance.GameManager.livingEntities.Count; w++)
                {
                    if (AppManager.Instance.GameManager.livingEntities[w].Rectangle.Intersects(rectangle))
                    {
                        gameObject = AppManager.Instance.GameManager.livingEntities[w];
                    }
                }
            }
            if (gameObject == entity1)
            {
                return null;
            }
            return gameObject;
        }
        public List<GameObject> CheckRectangle(Rectangle rectangle, Type type)
        {
            var gameObjects = AppManager.Instance.GameManager.GetAllGameObjects;
            List<GameObject> intersected = new List<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].GetType() == type)
                {
                    if (gameObjects[i].Rectangle.Intersects(rectangle))
                    {
                        intersected.Add(gameObjects[i]);
                    }
                }
            }
            return intersected;
        }
        public List<GameObject> CheckRectangle(Rectangle rectangle, bool player)
        {
            var gameObjects = AppManager.Instance.GameManager.GetPlayer1;
            List<GameObject> intersected = new List<GameObject>();
            
                
                
            if (gameObjects.Rectangle.Intersects(rectangle))
            {
                intersected.Add(gameObjects);
            }
                
            
            return intersected;
        }
        public List<GameObject> CheckRectangle(Rectangle rectangle)
        {
            var gameObjects = AppManager.Instance.GameManager.mapObjects;
            List<GameObject> intersected = new List<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                
                    if (gameObjects[i].Rectangle.Intersects(rectangle) && gameObjects[i].IsColliderOn)
                    {
                        intersected.Add(gameObjects[i]);
                    }
                
            }
            return intersected;
        }
    }
}