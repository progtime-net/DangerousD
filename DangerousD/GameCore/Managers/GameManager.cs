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
using System.Linq;
using DangerousD.GameCore.GUI;
using DangerousD.GameCore.Network;

namespace DangerousD.GameCore
{
    public class GameManager
    {
        public List<GameObject> GetAllGameObjects { get; private set; }
        private int currentEntityId = 0;
        public List<LivingEntity> livingEntities;
        public List<LivingEntity> livingEntitiesWithoutPlayers;
        public List<Entity> entities;
        public List<MapObject> mapObjects;
        public List<MapObject> BackgroundObjects;
        public List<GameObject> others;
        public MapManager mapManager;
        public PhysicsManager physicsManager;
        public List<Player> players;
        public List<GameObject> otherObjects = new();
        public Vector4 CameraBorder;
        public Player GetPlayer1 { get; private set; }
        private int _lastUpdate = 0;
        private int _currTime = 0;

        public GameManager()
        {
            others = new List<GameObject>();
            GetAllGameObjects = new List<GameObject>();
            livingEntities = new List<LivingEntity>();
            livingEntitiesWithoutPlayers = new List<LivingEntity>();
            mapObjects = new List<MapObject>();
            BackgroundObjects = new List<MapObject>();
            entities = new List<Entity>();
            players = new List<Player>();
            mapManager = new MapManager(1);
            physicsManager = new PhysicsManager();
            CameraBorder = Vector4.Zero;

        }

        internal void Register(GameObject gameObject)
        {
            GetAllGameObjects.Add(gameObject);
            if (gameObject is Entity)
            {
                gameObject.id = currentEntityId;
                currentEntityId++;
            }
            if (gameObject is Player objPl)
            {
                livingEntities.Add(gameObject as LivingEntity);
                players.Add(objPl);
                if (GetPlayer1 is null)
                {
                    GetPlayer1 = players[players.Count - 1];
                }
            }
            else if (gameObject is LivingEntity objLE)
            {
                livingEntitiesWithoutPlayers.Add(objLE);
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
        
        public void Remove(GameObject gameObject)
        {
            GetAllGameObjects.Remove(gameObject);
            if (gameObject is Player objPl)
            {
                livingEntities.Remove(gameObject as LivingEntity);
                players.Remove(objPl);
            }
            else if (gameObject is LivingEntity objLE)
            {
                livingEntities.Remove(objLE);
            }
            else if (gameObject is Entity objE)
            {
                entities.Remove(objE);
            }
            else if (gameObject is MapObject obj)
            {
                if (obj.IsColliderOn)
                    mapObjects.Remove(obj);
                else
                    BackgroundObjects.Remove(obj);
            }
            else
            {
                otherObjects.Remove(gameObject);
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
            _currTime += gameTime.ElapsedGameTime.Milliseconds;
            if (AppManager.Instance.NetworkTasks.Count > 0)
            {
                if (_currTime - _lastUpdate > 50)
                {
                    AppManager.Instance.DebugHUD.Log("sending");
                    AppManager.Instance.NetworkManager.SendMsg(AppManager.Instance.NetworkTasks.ToList());
                    AppManager.Instance.NetworkTasks.Clear();
                    _lastUpdate = _currTime;
                }
            }
            foreach (var item in BackgroundObjects)
                item.Update(gameTime);
            foreach (var item in mapObjects)
                item.Update(gameTime);
            foreach (var item in entities)
                item.Update(gameTime);
            if (AppManager.Instance.multiPlayerStatus != MultiPlayerStatus.Client)
            {
                for (int i = 0; i < livingEntitiesWithoutPlayers.Count; i++)
                {
                    livingEntitiesWithoutPlayers[i].Update(gameTime);
                }
            }
            else
            {
                for (int i = 0; i < livingEntitiesWithoutPlayers.Count; i++)
                {
                    livingEntitiesWithoutPlayers[i].PlayAnimation();
                }
            }
            foreach (Player player in players)
            {
                if (player.id != GetPlayer1.id)
                {
                    player.PlayAnimation();
                }
            }
            GetPlayer1.Update(gameTime);
            for(int i = 0; i < otherObjects.Count; i++)
            {
                otherObjects[i].Update(gameTime);
            }

            physicsManager.UpdateCollisions(entities, livingEntities, mapObjects, players, gameTime);
        }
        public void FindBorders()
        {
            foreach (var item in GetAllGameObjects)
            {
                if (item.Pos.X<CameraBorder.X)
                {
                    CameraBorder.X = item.Pos.X;
                }
                if (item.Pos.X > CameraBorder.Y)
                {
                    CameraBorder.Y = item.Pos.X;
                }
                if (item.Pos.Y < CameraBorder.Z)
                {
                    CameraBorder.Z = item.Pos.Y;
                }
                if (item.Pos.Y > CameraBorder.W)
                {
                    CameraBorder.W = item.Pos.Y;
                }
            }
        }

        public Player GetClosestPlayer(Vector2 position)
        {
            return players.OrderBy(x => (x.Pos - position).Length()).First();
        }
    }
}