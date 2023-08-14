using DangerousD.GameCore.GameObjects;
using DangerousD.GameCore.Graphics;
using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore
{
    static class GameManager
    {
        static List<LivingEntity> livingEntities;
        static List<Entity> entities;
        static List<MapObject> mapObjects;
        public static AnimationBuilder builder;
        public static MapManager mapManager;
        internal static void Register(GameObject gameObject)
        {
            if (gameObject is LivingEntity)
                livingEntities.Add(gameObject as LivingEntity);
            if (gameObject is Entity)
                entities.Add(gameObject as Entity);
            if (gameObject is MapObject)
                mapObjects.Add(gameObject as MapObject);
        }
        public static void Init()
        {
            livingEntities = new List<LivingEntity>();
            mapObjects = new List<MapObject>();
            entities = new List<Entity>();
            mapManager =new MapManager();
            mapManager.Init();
        }

        public static void Draw(SpriteBatch _spriteBatch)
        {
            foreach (var item in mapObjects)
                item.Draw(_spriteBatch);
            foreach (var item in entities)
                item.Draw(_spriteBatch);
            foreach (var item in livingEntities)
                item.Draw(_spriteBatch);
        }
        public static void Update(GameTime gameTime)
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
