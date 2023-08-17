using DangerousD.GameCore.GameObjects;
using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using DangerousD.GameCore.GameObjects.LivingEntities.Monsters;

namespace DangerousD.GameCore
{
    public class GameManager
    {
        public List<GameObject> GetAllGameObjects { get; private set; }

        public List<LivingEntity> livingEntities;
        public List<Entity> entities;
        public List<MapObject> mapObjects;
        public List<MapObject> BackgroundObjects;
        public List<GameObject> others;
        public MapManager mapManager;
        public PhysicsManager physicsManager;
        public List<Player> players;
        public List<GameObject> otherObjects = new();

        public Player GetPlayer1 { get; private set; }
        public GameManager()
        {
            others = new List<GameObject>();
            GetAllGameObjects = new List<GameObject>();
            livingEntities = new List<LivingEntity>();
            mapObjects = new List<MapObject>();
            BackgroundObjects = new List<MapObject>();
            entities = new List<Entity>();
            players = new List<Player>();
            mapManager = new MapManager(1);
            physicsManager = new PhysicsManager();
            
        }

        

        internal void Register(GameObject gameObject)
        {
            GetAllGameObjects.Add(gameObject);
            if (gameObject is Player objPl)
            {
                livingEntities.Add(gameObject as LivingEntity);
                players.Add(objPl);
                GetPlayer1 = players[0];
            }
            else if (gameObject is LivingEntity objLE)
            {
                livingEntities.Add(objLE);
            }
            else if (gameObject is Entity objE)
            {
                entities.Add(objE);
            }
            else if (gameObject is MapObject obj)
            {
                if (obj.IsColliderOn)
                    mapObjects.Add(obj);
                else
                    BackgroundObjects.Add(obj);
            }
            else
            {
                otherObjects.Add(gameObject);
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (var item in BackgroundObjects)
                item.Draw(_spriteBatch);
            foreach (var item in mapObjects)
                item.Draw(_spriteBatch);
            foreach (var item in entities)
                item.Draw(_spriteBatch);
            foreach (var item in livingEntities)
                item.Draw(_spriteBatch);
            foreach (var item in otherObjects)
                item.Draw(_spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in BackgroundObjects)
                item.Update(gameTime);
            foreach (var item in mapObjects)
                item.Update(gameTime);
            foreach (var item in entities)
                item.Update(gameTime);

            for (int i = 0; i < livingEntities.Count; i++)
                livingEntities[i].Update(gameTime);
            foreach (var item in otherObjects)
                item.Update(gameTime);

            physicsManager.UpdateCollisions(entities, livingEntities, mapObjects, gameTime);


        }
    }
}
