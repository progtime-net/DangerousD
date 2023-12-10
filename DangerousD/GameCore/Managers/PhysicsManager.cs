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

            var mapobjcts = mapObjects.Where(mo => mo is StopTile or Platform).ToList();
            CheckCollisionsLE_MO(livingEntities, mapobjcts, delta / 0.033f); //delta / 0.033 is normal to degengine


            CheckCollisionsPlayers_Platform(players, mapobjcts, delta / 0.033f, livingEntities);

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
                if (livingEntities[i] is Player) continue;
                int l1 = livingEntities.Count;
                var dMovement = CheckCollisionsSingleLE_MO(livingEntities[i], mapObjects, delta, livingEntities);
                if (dMovement != null)
                { 
                    livingEntities[i].SetPosition(livingEntities[i].Rectangle.Location.ToVector2() + dMovement.Value.ToVector2());
                }
                if (l1< livingEntities.Count)
                {
                    i--;
                }
            }
        }
        private Point? CheckCollisionsSingleLE_MO(LivingEntity livingEntity,
            List<MapObject> mapObjects, float delta, List<LivingEntity> livingEntities)
        {

            var currentRect = livingEntity.Rectangle;
            var newRect = currentRect;
            bool flagRemovedObject = false;

            #region x collision
            var collidedX = false;
            var tryingRectX = currentRect;
            tryingRectX.Offset((int)(livingEntity.velocity.X * delta), 0);//removed ceiling on X, it caused problems with sliding
            foreach (var mapObject in mapObjects.OfType<StopTile>())
            {
                if (
                    Math.Abs(mapObject.Pos.X - livingEntity.Pos.X) < 550
                    && Math.Abs(mapObject.Pos.Y - livingEntity.Pos.Y) < 550
                    && tryingRectX.Intersects(mapObject.Rectangle)
                )
                {
                    collidedX = true;
                    int prevL = livingEntities.Count;
                    livingEntity.OnCollision(mapObject);
                    if (livingEntities.Count < prevL)
                    {
                        flagRemovedObject = true;
                    }


                    break;
                }
            }

            if (flagRemovedObject)
            {
                return null;
                //continue;
            }
            if (collidedX)
            {
                livingEntity.velocity.X = 0;
            }
            else
            {
                newRect.X = tryingRectX.X;
            }
            #endregion

            #region y collision
            bool firstTimeFlag = false;
            SECONDTRY:
            var collidedY = false;
            var tryingRectY = currentRect;
            tryingRectY.Offset(0, (int)Math.Ceiling(livingEntity.velocity.Y * delta));

            if (livingEntity is Player)
            {
                AppManager.Instance.DebugHUD.Set("position", livingEntity.Pos.ToString());
                AppManager.Instance.DebugHUD.Set("velocity", livingEntity.velocity.ToString());
                AppManager.Instance.DebugHUD.Set("falling", (livingEntity as Player).FallingThroughPlatform.ToString());
                AppManager.Instance.DebugHUD.Set("intersects y", "");
            }
            foreach (var mapObject in mapObjects)
            {
                if ((livingEntity is Player || livingEntity is Bullet) && mapObject is Platform)
                {
                    continue;
                }
                if (tryingRectY.Intersects(mapObject.Rectangle))
                {
                    if (livingEntity is Player)
                        AppManager.Instance.DebugHUD.Set("intersects y", mapObject.GetType().ToString());
                    collidedY = true;
                    int prevL = livingEntities.Count;
                    livingEntity.OnCollision(mapObject);
                    if (livingEntities.Count < prevL)
                    {
                        flagRemovedObject = true;
                    }

                    break;
                }
            }
            if (flagRemovedObject)
            {
                return null;
                //continue;
            }
            livingEntity.isOnGround = collidedY && livingEntity.velocity.Y >= 0;
            if (collidedY)
            {
                livingEntity.velocity.Y = 0.101f;// livingEntities[i].acceleration.Y * delta / 2 * 0.001f; //castile
                if (!firstTimeFlag)
                {
                    firstTimeFlag = true;
                    goto SECONDTRY;
                }
                else
                {

                }
            }
            else
            {
                newRect.Y = tryingRectY.Y;
            }
            #endregion

            return newRect.Location - currentRect.Location;
        }
        private void CheckCollisionsPlayers_Platform(List<Player> players, List<MapObject> mapObjects, float delta, List<LivingEntity> livingEntities
            )
        {
            foreach (var player in players)
            {
                var dMovement2 = CheckCollisionsSingleLE_MO(players[0], mapObjects, delta, livingEntities);
                var dMovement = CheckCollisionsPlayer_Platform(player, mapObjects.OfType<Platform>().ToList(), delta);
                if (dMovement2 != null && dMovement == true)
                {
                    dMovement2= new Point(dMovement2.Value.X, 0);
                }
                if (dMovement2 != null  )
                {
                    player.SetPosition(player.Rectangle.Location.ToVector2() + dMovement2.Value.ToVector2());
                } 
            }

        }
        private bool CheckCollisionsPlayer_Platform(Player player, List<Platform> platforms, float delta)
        {

            bool firstTimeFlag = false;
            if (player.velocity.Y < 0 || player.FallingThroughPlatform)
            {
                return false;
            }
            SECONDTRY_PPlatform:
            var currentRect = player.Rectangle;
            var newRect = currentRect;

            var collidedY = false;
            var tryingRectY = currentRect;
            tryingRectY.Offset(0, (int)Math.Ceiling(player.velocity.Y * delta)); //tried to fix vertical gaps with boxes
            AppManager.Instance.DebugHUD.Set("intersects platform", "false");
            foreach (var platform in platforms)
            {
                AppManager.Instance.DebugHUD.Set("sus", (player.Rectangle.Bottom < platform.Rectangle.Top).ToString());
                AppManager.Instance.DebugHUD.Set("sus2", ( platform.Rectangle.Top).ToString());
                if (tryingRectY.Intersects(platform.Rectangle) && (player.Rectangle.Bottom < platform.Rectangle.Top + 8 && player.Rectangle.Bottom > platform.Rectangle.Top - 0))
                {
                    AppManager.Instance.DebugHUD.Set("intersects platform", "true");
                    collidedY = true;
                    break;
                }
            }
            //player.isOnGround = collidedY && player.velocity.Y >= 0;
            if (collidedY)
            {
                // костыль потому что в CheckCollisionsLE_MO он спускается
                newRect.Y -= (int)Math.Ceiling(player.velocity.Y * delta);
                player.isOnGround = true;
                player.velocity.Y = -0.99f;
                if (!firstTimeFlag)
                {
                    firstTimeFlag = true;
                    goto SECONDTRY_PPlatform;
                }
                return true;
            }
            else
            {
                //newRect.Y = tryingRectY.Y;
            }
            player.SetPosition(newRect.Location.ToVector2());
            return false;// newRect.Location - player.Rectangle.Location;
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