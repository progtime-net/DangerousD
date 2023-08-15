using DangerousD.GameCore.GameObjects;
using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore
{
    public class GameManager
    {

        public List<LivingEntity> livingEntities;
        public List<Entity> entities;
        public List<MapObject> mapObjects;
        public MapManager mapManager;


        public GameManager()
        {
            livingEntities = new List<LivingEntity>();
            mapObjects = new List<MapObject>();
            entities = new List<Entity>();
            mapManager = new MapManager();
            mapManager.Init();
        }

        internal void Register(GameObject gameObject)
        {
            if (gameObject is LivingEntity)
                livingEntities.Add(gameObject as LivingEntity);
            if (gameObject is Entity)
                entities.Add(gameObject as Entity);
            if (gameObject is MapObject)
                mapObjects.Add(gameObject as MapObject);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (var item in mapObjects)
                item.Draw(_spriteBatch);
            foreach (var item in entities)
                item.Draw(_spriteBatch);
            foreach (var item in livingEntities)
                item.Draw(_spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in mapObjects)
                item.Update(gameTime);
            foreach (var item in entities)
                item.Update(gameTime);
            foreach (var item in livingEntities)
                item.Update(gameTime);
        }
    }
}