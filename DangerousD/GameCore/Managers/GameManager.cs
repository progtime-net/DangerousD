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
        public List<GameObject> GetAllGameObjects { get; private set; }

        public List<LivingEntity> livingEntities;
        public List<Entity> entities;
        public List<MapObject> mapObjects;
        public MapManager mapManager;
        public PhysicsManager physicsManager;
        public List<Player> players;
        public List<GameObject> otherObjects = new();
        public Player GetPlayer1 { get; private set; }
        public GameManager()
        {
            GetAllGameObjects = new List<GameObject>();
            livingEntities = new List<LivingEntity>();
            mapObjects = new List<MapObject>();
            entities = new List<Entity>();
            players = new List<Player>();
            mapManager = new MapManager();
            physicsManager = new PhysicsManager();
            mapManager.Init();
        }

        internal void Register(GameObject gameObject)
        {
            

            GetAllGameObjects.Add(gameObject);
            if (gameObject is Player)
            {
                livingEntities.Add(gameObject as LivingEntity);
                players.Add(gameObject as Player);
                GetPlayer1 = players[0];
            }
            else if (gameObject is LivingEntity)
            {
                livingEntities.Add(gameObject as LivingEntity);
            }
            else if (gameObject is Entity)
            {
                entities.Add(gameObject as Entity);
            }
            else if (gameObject is MapObject)
            {
                mapObjects.Add(gameObject as MapObject);
            }
            else
            {
                otherObjects.Add(gameObject);
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
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
            foreach (var item in mapObjects)
                item.Update(gameTime);
            foreach (var item in entities)
                item.Update(gameTime);
            foreach (var item in livingEntities)
                item.Update(gameTime);
            foreach (var item in otherObjects)
                item.Update(gameTime);

            physicsManager.UpdateCollisions(entities, livingEntities, mapObjects, gameTime);


        }
    }
}