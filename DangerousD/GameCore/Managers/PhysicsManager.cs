using DangerousD.GameCore.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.Managers
{
    public class PhysicsManager
    {

        public void UpdateCollisions(List<Entity> entities, List<LivingEntity> livingEntities,
            List<MapObject> mapObjects, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (var item in livingEntities)
            {
                item.velocity = item.velocity + item.acceleration * delta;
            }

            CheckCollisions(livingEntities, mapObjects);
            OnCollision(entities, livingEntities);
            OnCollision(livingEntities);

            //entities dont move
            //Living entities dont move
            //mapObjects dont move
            //mapObjects have isCollisions on
            //Check collisions
            //Only Living entities move
            //OnCollision

        }
        public void CheckCollisions(List<LivingEntity> livingEntities,
            List<MapObject> mapObjects)
        {
            LivingEntity currentEntity;
            Rectangle oldRect;
            for (int i = 0; i < livingEntities.Count; i++)
            {
                currentEntity = livingEntities[i];
                oldRect = currentEntity.Rectangle;
                bool isXNormalise = true;
                bool isYNormalise = true;

                oldRect.Offset((int)currentEntity.velocity.X, 0);
                for (int j = 0; j < mapObjects.Count; j++)
                {
                    if (Math.Abs(mapObjects[i].Pos.X - currentEntity.Pos.X) < currentEntity.velocity.X * 2 && Math.Abs(mapObjects[i].Pos.Y - currentEntity.Pos.Y) < 50)
                    {
                        if (oldRect.Intersects(mapObjects[j].Rectangle))
                        {
                            isXNormalise = false;
                            oldRect.Offset(-(int)currentEntity.velocity.X, 0);
                            break;
                        }
                    }
                }
                if (!isXNormalise)
                    currentEntity.velocity.X = 0;


                oldRect.Offset(0, (int)currentEntity.velocity.Y);
                for (int j = 0; j < mapObjects.Count; j++)
                {

                    if (oldRect.Intersects(mapObjects[j].Rectangle))
                    {
                        isYNormalise = false;
                        oldRect.Offset(0, -(int)currentEntity.velocity.Y);
                        break;
                    }
                }
                if (!isYNormalise)
                    currentEntity.velocity.Y = 0;
                currentEntity.SetPosition(new Vector2(oldRect.X, oldRect.Y));
            }

        }
        public void OnCollision(List<Entity> entities, List<LivingEntity> livingEntities)
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
        public void OnCollision(List<LivingEntity> livingEntities)
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