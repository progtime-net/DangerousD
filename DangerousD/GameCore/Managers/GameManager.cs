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

        public List<LivingEntity> livingEntities;
        public List<Entity> entities;
        public List<MapObject> mapObjects;
        public MapManager mapManager;
        public PhysicsManager physicsManager;
        public List<Player> players;
        public Player GetPlayer1 { get; private set; }
        public GameManager()
        {
            livingEntities = new List<LivingEntity>();
            mapObjects = new List<MapObject>();
            entities = new List<Entity>();
            players = new List<Player>();
            mapManager = new MapManager(1);
            physicsManager = new PhysicsManager();
        }

        public void Initialize()
        {
            //mapManager.LoadLevel("Level1");
        }

        public void LoadContent()
        {
        }

        internal void Register(GameObject gameObject)
        {
            if (gameObject is LivingEntity)
                livingEntities.Add(gameObject as LivingEntity);
            if (gameObject is Entity)
                entities.Add(gameObject as Entity);
            if (gameObject is MapObject)
                mapObjects.Add(gameObject as MapObject);
            if (gameObject is Player)
            {
                players.Add(gameObject as Player);
                GetPlayer1= players[0];
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
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in mapObjects)
                item.Update(gameTime);
            foreach (var item in entities)
                item.Update(gameTime);
            foreach (var item in livingEntities)
                item.Update(gameTime);

            physicsManager.UpdateCollisions(entities, livingEntities, mapObjects, gameTime);


        }
    }
}