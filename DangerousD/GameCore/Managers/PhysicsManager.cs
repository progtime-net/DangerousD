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
                item.velocity = item.velocity + item.acceleration * delta/2; 
            }
            
            CheckCollisionsLE_MO(livingEntities, mapObjects.Where(mo => mo is StopTile or Platform).ToList(), delta / 0.033f); //delta / 0.033 is normal to degengine
            CheckCollisionsPlayer_Platform(players, mapObjects.OfType<Platform>().ToList(), delta / 0.033f);
            
            CheckCollisionsE_LE(entities, livingEntities);
            CheckCollisionsLE_LE(livingEntities);

            foreach (var item in livingEntities)
            {
                item.velocity = item.velocity + item.acceleration * delta / 2; //right way to add accelaration
            }
            //entities dont move
            //Living entities dont move
            //mapObjects dont move
            //mapObjects have isCollisions on
            //Check collisions
            //Only Living entities move
            //OnCollision

        }
        private void CheckCollisionsLE_MO(List<LivingEntity> livingEntities,
            List<MapObject> mapObjects, float delta)
        {
            for (int i = 0; i < livingEntities.Count; i++)
            {
                var currentRect = livingEntities[i].Rectangle;
                var newRect = currentRect;
                bool flagRemovedObject = false;
                
                #region x collision
                var collidedX = false;
                var tryingRectX = currentRect;
                tryingRectX.Offset((int)(livingEntities[i].velocity.X * delta), 0);//removed ceiling on X, it caused problems with sliding
                foreach (var mapObject in mapObjects.OfType<StopTile>())
                {
                    if (
                        Math.Abs(mapObject.Pos.X - livingEntities[i].Pos.X) < 550 
                        && Math.Abs(mapObject.Pos.Y - livingEntities[i].Pos.Y) < 550
                        && tryingRectX.Intersects(mapObject.Rectangle)
                    )
                    {
                        collidedX = true;
                        int prevL = livingEntities.Count;
                        livingEntities[i].OnCollision(mapObject);
                        if (livingEntities.Count<prevL)
                        {
                            i -= (prevL - livingEntities.Count);
                            flagRemovedObject = true;
                        }
                        
                        
                        break;
                    }
                }

                if (flagRemovedObject)
                {
                    continue;
                }
                if (collidedX)
                {
                    livingEntities[i].velocity.X = 0;
                }
                else
                {
                    newRect.X = tryingRectX.X;
                }
                #endregion
                
                #region y collision
                var collidedY = false;
                var tryingRectY = currentRect;
                
                tryingRectY.Offset(0, (int)Math.Ceiling(livingEntities[i].velocity.Y * delta));
                
                if (livingEntities[i] is Player)
                {
                    AppManager.Instance.DebugHUD.Set("velocity", livingEntities[i].velocity.ToString());
                    AppManager.Instance.DebugHUD.Set("falling", (livingEntities[i] as Player).FallingThroughPlatform.ToString());
                    AppManager.Instance.DebugHUD.Set("intersects y", "");
                }

                var isOnGround = false;
                foreach (var mapObject in mapObjects)
                {
                    if ((livingEntities[i] is Player||livingEntities[i] is Bullet)&& mapObject is Platform)
                    {
                        continue;
                    }
                    if (tryingRectY.Intersects(mapObject.Rectangle))
                    {
                        if (livingEntities[i] is Player) AppManager.Instance.DebugHUD.Set("intersects y", mapObject.GetType().ToString());
                        collidedY = true;

                        isOnGround = livingEntities[i].canCrawl || (tryingRectY.Top < mapObject.Rectangle.Bottom && tryingRectY.Top < mapObject.Rectangle.Top);
                        Console.WriteLine(tryingRectY.Bottom);
                        Console.WriteLine(mapObject.Rectangle.Top);
                        
                        
                        
                        int prevL = livingEntities.Count;
                        livingEntities[i].OnCollision(mapObject);
                        if (livingEntities.Count<prevL)
                        {
                            i -= (prevL - livingEntities.Count);
                            flagRemovedObject = true;
                        }

                        tryingRectY.Offset(0, -(int)Math.Ceiling(livingEntities[i].velocity.Y * delta));
                        if (mapObject.Rectangle.Top - tryingRectY.Bottom>0)
                        {
                            tryingRectY.Offset(0, mapObject.Rectangle.Top - tryingRectY.Bottom);

                            if (livingEntities[i] is Player)
                            {

                            }
                        }
                        if (tryingRectY.Intersects(mapObject.Rectangle))
                            break;
                    }
                }
                if (flagRemovedObject)
                {
                    continue;
                }
                if (livingEntities[i] is Player)
                {
                    Console.WriteLine(isOnGround);
                }
                livingEntities[i].isOnGround = isOnGround;
                if (collidedY)
                {
                    livingEntities[i].velocity.Y = 0;
                    if (livingEntities[i] is Player)
                    {
                        newRect.Y = tryingRectY.Y;
                    }
                }
                else
                {
                    newRect.Y = tryingRectY.Y;
                }
                #endregion
                
                livingEntities[i].SetPosition(newRect.Location.ToVector2());
            }
        }
        private void CheckCollisionsPlayer_Platform(List<Player> players, List<Platform> platforms, float delta)
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
                tryingRectY.Offset(0, (int)Math.Floor(player.velocity.Y)); //tried to fix vertical gaps with boxes
                AppManager.Instance.DebugHUD.Set("intersects platform", "false");
                foreach (var platform in platforms)
                {
                    AppManager.Instance.DebugHUD.Set("sus", (player.Rectangle.Bottom < platform.Rectangle.Top).ToString());
                    if (tryingRectY.Intersects(platform.Rectangle) && player.Rectangle.Bottom < platform.Rectangle.Top + 5)
                    {
                        AppManager.Instance.DebugHUD.Set("intersects platform", "true");
                        collidedY = true;



                        tryingRectY.Offset(0, -(int)Math.Ceiling(player.velocity.Y * delta));
                        if (platform.Rectangle.Top - tryingRectY.Bottom > 0)
                        {
                            tryingRectY.Offset(0, platform.Rectangle.Top - tryingRectY.Bottom); 
                        }
                        if (tryingRectY.Intersects(platform.Rectangle))
                            break;

                    }
                }
                if (collidedY)
                {
                    player.isOnGround = true;
                    // костыль потому что в CheckCollisionsLE_MO он спускается
                    newRect.Y = tryingRectY.Y;
                    player.velocity.Y = 0;
                }

                player.SetPosition(newRect.Location.ToVector2());
            }

        }
        private void CheckCollisionsE_LE(List<Entity> entities, List<LivingEntity> livingEntities)
        {
            for (int i = 0; i < entities.Count; i++)
            {


                for (int j = 0; j < livingEntities.Count && i < entities.Count; j++)
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

        private void CheckCollisionsLE_Platforms(List<LivingEntity> livingEntities, List<Platform> platforms)
        {
            foreach (var livingEntity in livingEntities)
            {
                var currentRect = livingEntity.Rectangle;
                var newRect = currentRect;
                var collidedY = false;
                var tryingRectY = currentRect;
                tryingRectY.Offset(0, (int)Math.Ceiling(livingEntity.velocity.Y));
                foreach (var platform in platforms)
                {
                    if (tryingRectY.Intersects(platform.Rectangle))
                    {
                        collidedY = true;
                        livingEntity.OnCollision(platform);

                        break;
                    }
                }
                //livingEntity.isOnGround = collidedY && livingEntity.velocity.Y > 0;
                if (collidedY)
                {
                    livingEntity.velocity.Y = 0;
                }
                else
                {
                    newRect.Y = tryingRectY.Y;
                }
                livingEntity.SetPosition(new Vector2(newRect.X, newRect.Y));
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
                Type objectType = gameObjects[i].GetType();
                while (objectType!=type&&objectType!=typeof(object))
                {
                    objectType = objectType.BaseType;
                }
                if ( objectType== type)
                {
                    if (gameObjects[i].Rectangle.Intersects(rectangle))
                    {
                        intersected.Add(gameObjects[i]);
                    }
                }
            }
            return intersected;
        }
        
        
    }
}