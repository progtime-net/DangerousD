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
            for (int i = 0; i < livingEntities.Count; i++)
            {
                for (int j = 0; j < mapObjects.Count; j++)
                {
                    if (livingEntities[i].Rectangle.Intersects(mapObjects[j].Rectangle))
                    {
                        if (livingEntities[i].Rectangle.Right > mapObjects[j].Rectangle.Left)
                        {

                            livingEntities[i].velocity.X = 0;

                            livingEntities[i].SetPosition(new Vector2(livingEntities[i].Pos.X - (livingEntities[i].Rectangle.Right - mapObjects[j].Rectangle.Left),
                                livingEntities[i].Pos.Y));
                        }
                        else if (livingEntities[i].Rectangle.Left < mapObjects[j].Rectangle.Right)
                        {
                            livingEntities[i].velocity.X = 0;
                            livingEntities[i].SetPosition(new Vector2(livingEntities[i].Pos.X + mapObjects[j].Rectangle.Right - livingEntities[i].Rectangle.Left,
                                livingEntities[i].Pos.Y));
                        }
                        else if (livingEntities[i].Rectangle.Bottom > mapObjects[j].Rectangle.Top)
                        {
                            livingEntities[i].velocity.Y = 0;
                            livingEntities[i].SetPosition(new Vector2(livingEntities[i].Pos.X,
                                livingEntities[i].Pos.Y - (livingEntities[i].Rectangle.Bottom - mapObjects[j].Rectangle.Top)));
                        }
                        else if (livingEntities[i].Rectangle.Top < mapObjects[j].Rectangle.Bottom)
                        {
                            livingEntities[i].velocity.Y = 0;
                            livingEntities[i].SetPosition(new Vector2(livingEntities[i].Pos.X,
                                livingEntities[i].Pos.Y + (mapObjects[j].Rectangle.Bottom - livingEntities[i].Rectangle.Top)));
                        }
                    }
                }
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
                        livingEntities[j].OnCollision();
                        entities[i].OnCollision();
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
                        livingEntities[i].OnCollision();
                        livingEntities[j].OnCollision();
                    }
                }
            }
        }


        public GameObject RayCast(LivingEntity entity1, LivingEntity entity2)
        {

            Rectangle rectangle;
            Vector2 distance = entity1.Pos - entity2.Pos;
            rectangle = new Rectangle((int)entity1.Pos.X, (int)entity1.Pos.Y, entity2.Width, entity2.Height);
            GameObject gameObject = null;
            double length = distance.Length();

            for (int i = 0; i < length; i++)
            {
                rectangle.X = (int)(entity2.Pos.X + (i / length) * distance.X);
                rectangle.Y = (int)(entity2.Pos.Y + (i / length) * distance.Y);

                //if (rectangle.Intersects(GameManager.Rectangle))
                //{
                //    return game
                //}
            }
            return gameObject;
        }
        public GameObject RayCast(LivingEntity entity1, Vector2 targetCast)
        {
            Rectangle rectangle;
            Vector2 direction = entity1.Pos - targetCast;
            rectangle = new Rectangle((int)entity1.Pos.X, (int)entity1.Pos.Y, 1, 1);
            GameObject gameObject = null;
            double k = direction.Length();

            for (int i = 0; i < k; i++)
            {
                rectangle.X = (int)(entity1.Pos.X + (i / k) * direction.X);
                rectangle.Y = (int)(entity1.Pos.Y + (i / k) * direction.X);
                if (gameObject != null)
                {
                    break;
                    return gameObject;
                }

            }


            return null;
        }
    }
}