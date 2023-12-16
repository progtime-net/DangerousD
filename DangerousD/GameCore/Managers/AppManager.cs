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
    public enum GameState
    {
        Menu, Options, Lobby, Game, Login, Death, HUD,
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
        public Point inGameHUDHelperResolution = new Point(1920, 1080);
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
        public Effect spriteEffect;
        public AppManager()
        {
            Content.RootDirectory = "Content";
            Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 30);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

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
            currentMap = "lvl1";
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
            GameObject.debugTexture.SetData<Color>(new Color[] { Color.White });
            SoundManager.LoadSounds();
            SoundManager.StartAmbientSound("DoomTestSong");
            renderTarget = new RenderTarget2D(GraphicsDevice, inGameResolution.X, inGameResolution.Y);
            spriteEffect = Content.Load<Effect>("Shaders//Glow");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (GameManager.GetPlayer1 != null)
            {
                DebugHUD?.Set("isShooting:", GameManager.GetPlayer1.isShooting.ToString());
                DebugHUD.Set("id: ", GameManager.GetPlayer1.id.ToString());
            }
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
                    GameManager.Draw(_spriteBatch);
                    HUD.Draw(_spriteBatch);
                    break;
                default:
                    break;
            }
            GraphicsDevice.SetRenderTarget(null);

            //DrawTheScreenWithShootEffects(); //DrawScreen

            spriteEffect.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            DrawTheScreenWithNoEffects();
            DebugHUD.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
        #region effects and experiments - SergoDobro
        public void DrawTheScreenWithNoEffects()
        {
            spriteEffect.CurrentTechnique = spriteEffect.Techniques["Distortion"];
            DrawScreenByParts(0,1, spriteEffect);
        }
        Random random = new Random();
        public void DrawTheScreenWithShootEffects()
        {
            DrawScreenByParts(0, 1);
            return;
            #region test 

            if (gameState == GameState.Game)
            {
                if (GameManager.GetPlayer1.isShooting)
                {
                    if (random.NextDouble()>0.0)
                    {
                        spriteEffect.CurrentTechnique = spriteEffect.Techniques["Dark"];
                        DrawScreenByParts(0, 1, spriteEffect);
                        if (GameManager.GetPlayer1.isShooting)
                        {
                            AppManager.Instance.spriteEffect.CurrentTechnique = AppManager.Instance.spriteEffect.Techniques["Yellow"];
                            AppManager.Instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, effect: AppManager.Instance.spriteEffect);
                            GameManager.GetPlayer1.Draw(_spriteBatch);
                            _spriteBatch.End();
                        }
                    }
                    else
                        DrawScreenByParts(0, 1);
                }
                else
                    DrawScreenByParts(0, 1);
            }
            else
                DrawScreenByParts(0, 1);

            #endregion 
        }
        public void DrawTheScreenWithEffects()
        {
            #region test 

            //GraphicsDevice.BlendState = BlendState.Additive;//отвечает за способ нанесения новый текстур (AlphaBlend - обычный случай)
             
            spriteEffect.CurrentTechnique = spriteEffect.Techniques["Distortion"]; //настройка шейдера (выбор техники), при _spriteBatch.End() будет выполнена обработка шейдером последней настроййки
                                                                             //TODO: Понять что за Pass[0].Apply() 
            DrawScreenByParts(0, 0.2, spriteEffect);

            spriteEffect.CurrentTechnique = spriteEffect.Techniques["Blur2"];
            DrawScreenByParts(0.2, 0.4, spriteEffect);


            DrawScreenByParts(0.6, 0.8); 
            GraphicsDevice.BlendState = BlendState.Opaque;
            spriteEffect.CurrentTechnique = spriteEffect.Techniques["Blur3"];
            DrawScreenByParts(0.4, 0.7, spriteEffect);


            DrawScreenByParts(0.8, 1);
            #endregion 
        } 
        public void DrawScreenByParts(double startProc, double endProc, Effect effect = null) //for shader tests
        {
            _spriteBatch.Begin(effect: effect);
            _spriteBatch.Draw(renderTarget, new Rectangle((int)(_graphics.PreferredBackBufferWidth * startProc), 0
                , (int)(_graphics.PreferredBackBufferWidth * (endProc - startProc)), _graphics.PreferredBackBufferHeight),
                 new Rectangle((int)(renderTarget.Width * startProc), 0, (int)(renderTarget.Width * (endProc - startProc)), renderTarget.Height), Color.White);
            _spriteBatch.End();
        } 
        #endregion


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
                        if (networkTask.type == typeof(Bullet))
                        {
                            Bullet bullet = new Bullet(networkTask.position);
                            bullet.id = networkTask.objId;
                            bullet.velocity = networkTask.velocity;
                            bullet.acceleration = Vector2.Zero;
                            bullet.maindirection = bullet.velocity;
                        }
                        else if (networkTask.type == typeof(Particle))
                        {
                            Particle particle = new Particle(networkTask.position);
                            particle.id = networkTask.objId;
                            particle.velocity = networkTask.velocity;
                        }
                        break;
                    case NetworkTaskOperationEnum.SendPosition:
                        if (networkTask.objId != GameManager.GetPlayer1.id)
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
                                if (gc.GetCurrentAnimation != networkTask.name) gc.StartAnimation(networkTask.name);
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
                        Player player1 = GameManager.players.Find(x => x.id == networkTask.objId);
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

        
        public void ChangeMap(string map, Vector2 startPos)
        {
            List<Player> players = GameManager.players;
            GameManager = new();
            foreach (var player in players)
            {
                player.SetPosition(new Vector2(startPos.X, startPos.Y - player.Height));
                GameManager.Register(player);
            }
            
            currentMap = map;
            ChangeGameState(GameState.Game);
        }
    }
}
