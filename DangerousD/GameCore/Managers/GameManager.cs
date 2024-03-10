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
using DangerousD.GameCore.GameObjects.Entities;
using System.IO; 
using Newtonsoft.Json;

namespace DangerousD.GameCore
{
    public class GameManager
    {
        public List<GameObject> GetAllGameObjects { get; private set; } = new();
        private int currentEntityId = 1;
        public List<LivingEntity> livingEntities = new();
        public List<LivingEntity> livingEntitiesWithoutPlayers = new();
        public List<Entity> entities = new();
        public List<MapObject> mapObjects = new();
        public List<MapObject> BackgroundObjects = new();
        public List<GameObject> others = new();
        public MapManager mapManager = new(1);
        public PhysicsManager physicsManager = new();
        public List<Player> players = new();
        public List<GameObject> otherObjects = new();
        public Vector4 CameraBorder = Vector4.Zero;

        public Player GetPlayer1 { get; private set; }
        private int _lastUpdate = 0;
        private int _currTime = 0;

        public GameManager(List<Player> players, Player player1)
        {
            this.players = players;
            GetPlayer1 = player1;
            everyRunDataTotal.LoadEveryRunDataFromMemory();
        }

        public GameManager() { }

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
            if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host || gameObject is Door)
            {
                NetworkTask task = new NetworkTask();
                AppManager.Instance.NetworkTasks.Add(task.DeleteObject(gameObject.id));
            }
            GetAllGameObjects.Remove(gameObject);
            if (gameObject is Player objPl)
            {
                livingEntities.Remove(gameObject as LivingEntity);
                players.Remove(objPl);
            }
            else if (gameObject is LivingEntity objLE)
            {
                livingEntities.Remove(objLE);
                livingEntitiesWithoutPlayers.Remove(objLE);
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
            AppManager.Instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            AppManager.Instance.spriteEffect.CurrentTechnique = AppManager.Instance.spriteEffect.Techniques["Dark"];
            if (GetPlayer1.isShooting && Math.Abs(GetPlayer1.velocity.X) > 2)
                _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, effect: AppManager.Instance.spriteEffect);
            else
                _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            foreach (var item in BackgroundObjects)
                item.Draw(_spriteBatch);
            foreach (var item in mapObjects)
                item.Draw(_spriteBatch);

            _spriteBatch.End();

            AppManager.Instance.spriteEffect.CurrentTechnique = AppManager.Instance.spriteEffect.Techniques["Red"];
            if (GetPlayer1.isShooting && Math.Abs(GetPlayer1.velocity.X) > 2)
                _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, effect: AppManager.Instance.spriteEffect);
            else
                _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            foreach (var item in entities)
                item.Draw(_spriteBatch);
            foreach (var item in livingEntities)
                item.Draw(_spriteBatch);
            foreach (var item in otherObjects)
                item.Draw(_spriteBatch);

            //draw yellow player
            _spriteBatch.End();


            AppManager.Instance.spriteEffect.CurrentTechnique = AppManager.Instance.spriteEffect.Techniques["Dark"];
            if (GetPlayer1.isShooting && Math.Abs(GetPlayer1.velocity.X) > 2)
            {
                AppManager.Instance.spriteEffect.CurrentTechnique = AppManager.Instance.spriteEffect.Techniques["Yellow"];
                _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, effect: AppManager.Instance.spriteEffect);
                GetPlayer1.Draw(_spriteBatch);
                _spriteBatch.End();
            }
        }

        public void Update(GameTime gameTime)
        {

            #region Network Update
            _currTime += gameTime.ElapsedGameTime.Milliseconds;
            if (_currTime - _lastUpdate > 50 && AppManager.Instance.multiPlayerStatus != MultiPlayerStatus.SinglePlayer)
            {
                foreach (Entity entity in livingEntities)
                {
                    if (entity is Player || AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host)
                    {
                        NetworkTask task = new NetworkTask(entity.id, entity.Pos);
                        AppManager.Instance.NetworkTasks.Add(task);
                        AppManager.Instance.NetworkTasks.Add(new NetworkTask(entity.id, entity.GetGraphicsComponent().GetCurrentAnimation, Vector2.Zero));
                    }
                }
                AppManager.Instance.DebugHUD.Log("sending");
                AppManager.Instance.NetworkManager.SendMsg(AppManager.Instance.NetworkTasks.ToList());
                AppManager.Instance.NetworkTasks.Clear();
                _lastUpdate = _currTime;
            }
            #endregion


            //gameTime = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime); //normal
            gameTime = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime * 1);
            foreach (var item in BackgroundObjects)
                item.Update(gameTime);

            foreach (var item in mapObjects)
                item.Update(gameTime);

            for (int i = 0; i < entities.Count; i++)
                entities[i].Update(gameTime);

            #region living entities update
            if (AppManager.Instance.multiPlayerStatus != MultiPlayerStatus.Client)
                for (int i = 0; i < livingEntitiesWithoutPlayers.Count; i++)
                    livingEntitiesWithoutPlayers[i].Update(gameTime);
            else
                for (int i = 0; i < livingEntitiesWithoutPlayers.Count; i++)
                    livingEntitiesWithoutPlayers[i].PlayAnimation();
            #endregion

            foreach (Player player in players)
                if (player.id != GetPlayer1.id)
                    player.PlayAnimation();

            GetPlayer1.Update(gameTime);

            for (int i = 0; i < otherObjects.Count; i++)
                otherObjects[i].Update(gameTime);

            physicsManager.UpdateCollisions(entities, livingEntities, mapObjects, players, gameTime);
        }
        public void FindBorders()
        {
            foreach (var item in GetAllGameObjects)
            {
                if (item.Pos.X < CameraBorder.X)
                    CameraBorder.X = item.Pos.X;
                if (item.Pos.X > CameraBorder.Y)
                    CameraBorder.Y = item.Pos.X;
                if (item.Pos.Y < CameraBorder.Z)
                    CameraBorder.Z = item.Pos.Y;
                if (item.Pos.Y > CameraBorder.W)
                    CameraBorder.W = item.Pos.Y;
            }
        }

        #region Timer
        private double timeGameStarted;
        private EveryRunDataTotal everyRunDataTotal = new EveryRunDataTotal();
        public double GetTimeOfPlaythrough { get { return AppManager.Instance.gameTime.TotalGameTime.TotalSeconds - timeGameStarted; } }
        public EveryRunDataTotal EveryRunDataTotal { get { return everyRunDataTotal; } }
        public void ChangedStateGame(GameTime gameTime)
        {
            timeGameStarted = gameTime.TotalGameTime.TotalSeconds;
            mapManager.LoadLevel(AppManager.Instance.currentMap);
            FindBorders();
        }

        public void LoadEveryRunData(EveryRunDataTotal everyRunDataTotal) //for loading from old games and between levels
        {
            EveryRunDataTotal.LoadEveryRunData(everyRunDataTotal);
        }
        #endregion
    }
    [Serializable]
    public class EveryRunDataTotal //All stats are here
    {
        [JsonProperty("TimeData")]
        public EveryRunDataElement time = new EveryRunDataElement() { BetterBigModifier = false };
        [JsonProperty("ScoreData")]
        public EveryRunDataElement score = new EveryRunDataElement();
        public EveryRunDataElement Time { get { return time; } }
        public EveryRunDataElement Score { get { return score; } }  
        public void FixateLevelParametrs(string level, bool hasDied = false)
        {
            time.FixateLevelParametr(level, AppManager.Instance.GameManager.GetTimeOfPlaythrough, setBest: !hasDied);
            Score.FixateLevelParametr(level, AppManager.Instance.GameManager.GetPlayer1.score, setBest: !hasDied);

            
            Serialize("LevelPlaythroughsData.txt", this);
        }
        public void LoadEveryRunData(EveryRunDataTotal everyRunDataTotal) //for loading from old games and between levels
        {
            
            
            this.time = everyRunDataTotal.Time;
            this.score = everyRunDataTotal.Score;
        }
        public void LoadEveryRunDataFromMemory()
        {
            if (!File.Exists("LevelPlaythroughsData.txt")) return;
            try
            {
                EveryRunDataTotal everyRunDataTotalFromSave = (EveryRunDataTotal)Deserialize("LevelPlaythroughsData.txt");
                LoadEveryRunData(everyRunDataTotalFromSave);
            }
            catch 
            {
            }
        }



        public void Serialize(string nameFile, EveryRunDataTotal obj)
        {
            using (var stream = new StreamWriter(nameFile))
            {
                string str = JsonConvert.SerializeObject(this);  
                stream.WriteLine(str);
            }
        }

        public object Deserialize(string nameFile)
        {
            using (var stream = new StreamReader(nameFile))
            { 
                return JsonConvert.DeserializeObject(stream.ReadToEnd(), typeof(EveryRunDataTotal));
            }
        }
    }
    [Serializable]
    public class EveryRunDataElement
    {

        [JsonProperty("LevelsAndParametrs")]
        public Dictionary<string, double> LevelsAndTheirParametr = new Dictionary<string, double>(); //each run they are rewritten
        [JsonProperty("LevelsAndBestParametrs")]
        public Dictionary<string, double> LevelsAndTheirBestParametr = new Dictionary<string, double>(); //TODO: add loading and saving 
        [JsonProperty("betterBigModifier")]
        int betterBigModifier = 1;
        public bool BetterBigModifier { set { betterBigModifier = value ? 1 : -1; } }
        public void FixateLevelParametr(string level, double parametr, bool setBest = true)
        {
            if (setBest) FixateBestLevelParametr(level, parametr);

            if (!LevelsAndTheirParametr.ContainsKey(level))
                LevelsAndTheirParametr.Add(level, parametr);
            else
                LevelsAndTheirParametr[level] = parametr;

        }
        public void FixateBestLevelParametr(string level, double parametr)
        {
            if (LevelsAndTheirBestParametr.ContainsKey(level))
            {
                if (betterBigModifier * LevelsAndTheirBestParametr[level] < betterBigModifier * parametr)
                    LevelsAndTheirBestParametr[level] = parametr;
            }
            else
                LevelsAndTheirBestParametr.Add(level, parametr);
        }
        public double GetLevelParametr(string level)
        {
            if (LevelsAndTheirParametr.ContainsKey(level))
                return LevelsAndTheirParametr[level];
            return 0;
        }
        public double GetBestLevelParametr(string level)
        {
            if (LevelsAndTheirBestParametr.ContainsKey(level))
                return LevelsAndTheirBestParametr[level];
            return 0;// GetLevelParametr(level);
        }

    }


}