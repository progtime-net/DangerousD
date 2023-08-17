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

namespace DangerousD.GameCore
{
    public enum MultiPlayerStatus { SinglePlayer, Host, Client }
    public enum GameState { Menu, Options, Lobby, Game, Login, Death, HUD,
        GameOver
    }
    public class AppManager : Game
    {
        public static AppManager Instance { get; private set; }
        public string IpAddress { get; private set; } = "127.0.0.1";
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public GameState gameState { get; private set; }
        public MultiPlayerStatus multiPlayerStatus { get; private set; } = MultiPlayerStatus.SinglePlayer;
        public Point resolution = new Point(1920, 1080);
        public Point inGameResolution = new Point(1920, 1080);
        IDrawableObject MenuGUI;
        IDrawableObject OptionsGUI;
        IDrawableObject LoginGUI;
        IDrawableObject LobbyGUI;
        IDrawableObject DeathGUI;
        IDrawableObject HUD;
        public DebugHUD DebugHUD;

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
            _graphics.PreferredBackBufferWidth = resolution.X;
            _graphics.PreferredBackBufferHeight = resolution.Y;
            _graphics.IsFullScreen = true;
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
                case GameState.HUD:
                    HUD.Update(gameTime);
                    break;
                case GameState.Game:
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
                    GameManager.mapManager.LoadLevel("lvl");
                    break;
                case GameState.Death:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void NetworkSync(List<NetworkTask> networkTasks)
        {
            foreach (NetworkTask networkTask in networkTasks)
            {
                switch (networkTask.operation)
                {
                    case NetworkTaskOperationEnum.TakeDamage:
                        break;
                    case NetworkTaskOperationEnum.SendSound:
                        SoundManager.StartSound(networkTask.name, networkTask.position, GameManager.GetPlayer1.Pos);
                        break;
                    case NetworkTaskOperationEnum.CreateEntity:
                        break;
                    case NetworkTaskOperationEnum.SendPosition:
                        break;
                    case NetworkTaskOperationEnum.ChangeState:
                        break;
                    case NetworkTaskOperationEnum.ConnectToHost:
                        break;
                    case NetworkTaskOperationEnum.GetClientPlayerId:
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
    }
}
