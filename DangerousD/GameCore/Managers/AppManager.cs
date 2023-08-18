using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using DangerousD.GameCore.GUI;
using Microsoft.Xna.Framework.Input;
using DangerousD.GameCore.Graphics;
using DangerousD.GameCore.Network;
using MonogameLibrary.UI.Base;
using DangerousD.GameCore.Managers;
using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.GameObjects;
using System.Threading.Tasks;

namespace DangerousD.GameCore
{
    public enum MultiPlayerStatus { SinglePlayer, Host, Client }
    public enum GameState { Menu, Options, Lobby, Game, Login, Death, HUD,
        GameOver
    }
    public class AppManager : Game
    {
        public static AppManager Instance { get; private set; }
        public string IpAddress { get; private set; } = "0.0.0.0";
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public GameState gameState { get; private set; }
        public MultiPlayerStatus multiPlayerStatus { get; private set; } = MultiPlayerStatus.SinglePlayer;
        public Point resolution;
        public Point inGameResolution = new Point(1920, 1080);
        public Point inGameHUDHelperResolution= new Point(1920, 1080);
        IDrawableObject MenuGUI;
        IDrawableObject OptionsGUI;
        IDrawableObject LoginGUI;
        IDrawableObject LobbyGUI;
        IDrawableObject DeathGUI;
        IDrawableObject HUD;
        public DebugHUD DebugHUD;
        public List<NetworkTask> NetworkTasks = new List<NetworkTask>();
        public string currentMap;
        public GameManager GameManager { get; private set; } = new();
        public AnimationBuilder AnimationBuilder { get; private set; } = new AnimationBuilder();
        public NetworkManager NetworkManager { get; private set; } = new NetworkManager();
        public InputManager InputManager { get; private set; } = new InputManager();
        public SoundManager SoundManager { get; private set; } = new SoundManager();
        public SettingsManager SettingsManager { get; private set; } = new SettingsManager();

        private RenderTarget2D renderTarget;
        public AppManager()
        {
            Content.RootDirectory = "Content";
            Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 30);

            SettingsManager = new SettingsManager();
            SettingsManager.LoadSettings();

            NetworkManager.GetReceivingMessages += NetworkSync;

            resolution = SettingsManager.Resolution;
            SetIsFullScreen(!SettingsManager.IsFullScreen);
            SetIsFullScreen(SettingsManager.IsFullScreen);
            _graphics.PreferredBackBufferWidth = resolution.X;
            _graphics.PreferredBackBufferHeight = resolution.Y;
            _graphics.IsFullScreen = false;
            gameState = GameState.Menu;
            MenuGUI = new MenuGUI();
            LoginGUI = new LoginGUI();
            OptionsGUI = new OptionsGUI();
            LobbyGUI = new LobbyGUI();
            DeathGUI = new DeathGUI();
            HUD = new HUD();
            DebugHUD = new DebugHUD();
            UIManager.resolution = resolution;
            UIManager.resolutionInGame = inGameResolution;
            currentMap = "lvl";
        }

        protected override void Initialize()
        {
            AnimationBuilder.LoadAnimations();
            MenuGUI.Initialize();
            LoginGUI.Initialize();

            DebugHUD.Initialize();
            OptionsGUI.Initialize();
            HUD.Initialize();
            LobbyGUI.Initialize();
            DeathGUI.Initialize();
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DebugHUD.LoadContent();
            MenuGUI.LoadContent();
            LoginGUI.LoadContent();
            OptionsGUI.LoadContent();
            LobbyGUI.LoadContent();
            DeathGUI.LoadContent();
            HUD.LoadContent();
            GameObject.debugTexture = new Texture2D(GraphicsDevice, 1, 1);
            GameObject.debugTexture.SetData<Color>(new Color[] { new Color(1, 0,0,0.25f) });
            SoundManager.LoadSounds();
            SoundManager.StartAmbientSound("DoomTestSong"); 
            renderTarget = new RenderTarget2D(GraphicsDevice, inGameResolution.X, inGameResolution.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (GameManager.GetPlayer1 != null)
                DebugHUD.Set("id: ", GameManager.GetPlayer1.id.ToString());
            InputManager.Update();
            SoundManager.Update();

            switch (gameState)
            {
                case GameState.Menu:
                    MenuGUI.Update(gameTime);
                    break;
                case GameState.Options:
                    OptionsGUI.Update(gameTime);
                    break;
                case GameState.Login:
                    LoginGUI.Update(gameTime);
                    break;
                case GameState.Lobby:
                    LobbyGUI.Update(gameTime);
                    break;
                case GameState.Death:
                    DeathGUI.Update(gameTime);
                    break;
                case GameState.Game:
                    HUD.Update(gameTime);
                    GameManager.Update(gameTime);
                    
                    break;
                default:
                    break;
            }
            DebugHUD.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
             
            GraphicsDevice.SetRenderTarget(renderTarget);
            switch (gameState)
            {
                case GameState.Menu:
                    MenuGUI.Draw(_spriteBatch);
                    break;
                case GameState.Options:
                    OptionsGUI.Draw(_spriteBatch);
                    break;
                case GameState.Login:
                    LoginGUI.Draw(_spriteBatch);
                    break;
                case GameState.Lobby:
                    LobbyGUI.Draw(_spriteBatch);
                    break;
                case GameState.Death:
                    DeathGUI.Draw(_spriteBatch);
                    break;
                case GameState.HUD:
                    HUD.Draw(_spriteBatch);
                    break;
                case GameState.Game:
                    _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                    GameManager.Draw(_spriteBatch);
                    _spriteBatch.End();
                    HUD.Draw(_spriteBatch);
                    break;
                default:
                    break;
            }
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin();
            _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.End();


            DebugHUD.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        public void ChangeGameState(GameState gameState)
        {
            this.gameState = gameState;
            switch (this.gameState)
            {
                case GameState.Menu:
                    break;
                case GameState.Options:
                    break;
                case GameState.Login:
                    break;
                case GameState.Lobby:
                    break;
                case GameState.Game:
                    GameManager.mapManager.LoadLevel(currentMap);
                    GameManager.FindBorders();
                    break;
                case GameState.Death:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void NetworkSync(List<NetworkTask> networkTasks)
        {
            DebugHUD.Log("networksync");
            foreach (NetworkTask networkTask in networkTasks)
            {
                switch (networkTask.operation)
                {
                    case NetworkTaskOperationEnum.DeleteObject:
                        GameObject gameObject = GameManager.GetAllGameObjects.Find(x => x.id == networkTask.objId);
                        if (gameObject != null)
                        {
                            GameManager.Remove(gameObject);
                        }
                        break;
                    case NetworkTaskOperationEnum.SendSound:
                        SoundManager.StartSound(networkTask.name, networkTask.position, GameManager.GetPlayer1.Pos);
                        break;
                    case NetworkTaskOperationEnum.CreateEntity:
                        if (networkTask.type == typeof(Player.Bullet))
                        {
                            Player.Bullet bullet = new Player.Bullet(networkTask.position);
                            bullet.id = networkTask.objId;
                            bullet.velocity = networkTask.velocity;
                        }
                        break;
                    case NetworkTaskOperationEnum.SendPosition:
                        if (networkTask.objId != GameManager.GetPlayer1.id )
                        {
                            LivingEntity entity = GameManager.livingEntities.Find(x => x.id == networkTask.objId);
                            if (entity != null)
                                entity.SetPosition(networkTask.position);
                            if (multiPlayerStatus == MultiPlayerStatus.Host)
                            {
                                NetworkTasks.Add(networkTask);  
                            }
                        }
                        break;
                    case NetworkTaskOperationEnum.ChangeState:
                        if (networkTask.objId != GameManager.GetPlayer1.id)
                        {
                            List<GraphicsComponent> gcs = new List<GraphicsComponent>();
                            foreach (var player in GameManager.players)
                            {
                                gcs.Add(player.GetGraphicsComponent());
                            }
                            LivingEntity entity = GameManager.livingEntities.Find(x => x.id == networkTask.objId);
                            if (entity != null)
                            {
                                GraphicsComponent gc = entity.GetGraphicsComponent();
                                gc.StartAnimation(networkTask.name);
                            }
                        }
                        break;
                    case NetworkTaskOperationEnum.ConnectToHost:
                        Player connectedPlayer = new Player(Vector2.Zero, true);
                        NetworkTasks.Add(new NetworkTask(connectedPlayer.id));
                        NetworkTask task = new NetworkTask();
                        foreach (Player player in GameManager.players)
                        {
                            if (player.id != connectedPlayer.id)
                            {
                                NetworkTasks.Add(task.AddConnectedPlayer(player.id, player.Pos));
                            }
                        }
                        break;
                    case NetworkTaskOperationEnum.GetClientPlayerId:
                        if (!GameManager.GetPlayer1.isIdFromHost)
                        {
                            GameManager.GetPlayer1.id = networkTask.objId;
                            GraphicsComponent gcsd = GameManager.GetPlayer1.GetGraphicsComponent();
                            gcsd.parentId = networkTask.objId;
                            GameManager.GetPlayer1.isIdFromHost = true;
                        }
                        break;
                    case NetworkTaskOperationEnum.AddConnectedPlayer:
                        Player remoteConnectedPlayer = new Player(networkTask.position, true);
                        remoteConnectedPlayer.id = networkTask.objId;
                        remoteConnectedPlayer.GetGraphicsComponent().parentId = networkTask.objId;
                        break;
                    case NetworkTaskOperationEnum.KillPlayer:
                        Player player1 = GameManager.players.Find(x => x.id==networkTask.objId);
                        player1.Death(networkTask.name);
                        NetworkTask task1 = new NetworkTask();
                        NetworkTasks.Add(task1.DeleteObject(player1.id));
                        break;
                    default:
                        break;
                }
            }
        }
        public void SetMultiplayerState(MultiPlayerStatus multiPlayerStatus)
        {
            this.multiPlayerStatus = multiPlayerStatus;
        }
        public void SetIsFullScreen(bool fullscrin)
        {
            DebugHUD?.Set("resX:", SettingsManager.Resolution.X.ToString());
            DebugHUD?.Set("resY:", SettingsManager.Resolution.Y.ToString());
            DebugHUD?.Set("FullScreen:", _graphics.IsFullScreen.ToString());
            if (fullscrin)
            {
                _graphics.PreferredBackBufferWidth = 1920;
                _graphics.PreferredBackBufferHeight = 1080;
            }
            else
            {
                _graphics.PreferredBackBufferWidth = SettingsManager.Resolution.X;
                _graphics.PreferredBackBufferHeight = SettingsManager.Resolution.Y;
            }
            UIManager.resolution = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _graphics.IsFullScreen = fullscrin;
            _graphics.ApplyChanges();
        }
        public void Restart(string map)
        {
            GameManager = new();
            ChangeGameState(GameState.Menu);
            currentMap = map;
        }
    }
}
