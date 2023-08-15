using DangerousD.GameCore.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.Managers
{
    internal class PhysicsManager
    {
        public void UpdateCollisions(List<Entity> entities, List<LivingEntity> livingEntities,
            List<MapObject> mapObjects)
        {
           
            
            CheckCollisions(livingEntities,mapObjects);
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
                            livingEntities[i].SetPosition(new Vector2(livingEntities[i].Pos.X - (livingEntities[i].Rectangle.Right - mapObjects[j].Rectangle.Left),
                                livingEntities[i].Pos.Y));

                        }
                        else if (livingEntities[i].Rectangle.Left < mapObjects[j].Rectangle.Right)
                        {
                            livingEntities[i].SetPosition(new Vector2(livingEntities[i].Pos.X + mapObjects[j].Rectangle.Right - livingEntities[i].Rectangle.Left,
                                livingEntities[i].Pos.Y));
                        }
                        else if (livingEntities[i].Rectangle.Bottom > mapObjects[j].Rectangle.Top)
                        {
                            livingEntities[i].SetPosition(new Vector2(livingEntities[i].Pos.X,
                                livingEntities[i].Pos.Y - (livingEntities[i].Rectangle.Bottom - mapObjects[j].Rectangle.Top)));
                        }
                        else if (livingEntities[i].Rectangle.Top < mapObjects[j].Rectangle.Bottom)
                        {
                            livingEntities[i].SetPosition(new Vector2(livingEntities[i].Pos.X,
                                livingEntities[i].Pos.Y + (mapObjects[j].Rectangle.Bottom - livingEntities[i].Rectangle.Top)));
                        }
                    }
                }
            }
           
        }
        public void OnCollision(List<Entity>entities,List<LivingEntity> livingEntities )
        {
            for (int i = 0; i <entities.Count ; i++)
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
                for (int j = i+1; j < livingEntities.Count; j++)
                {
                    if (livingEntities[i].Rectangle.Intersects(livingEntities[j].Rectangle))
                    {
                        livingEntities[i].OnCollision();
                        livingEntities[j].OnCollision();
                    }
                }
            }
        }
        
    }
}
