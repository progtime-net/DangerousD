﻿using Microsoft.Xna.Framework;
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

namespace DangerousD.GameCore
{
    public enum GameState { Menu, Options, Lobby, Game, Login, Death }
    public class AppManager : Game
    {
        public static AppManager Instance { get; private set;  }
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Point resolution = new Point(1920, 1080);
        public Point inGameResolution = new Point(1920, 1080);
        GameState gameState;
        IDrawableObject MenuGUI;
        IDrawableObject OptionsGUI;
        IDrawableObject LoginGUI;
        IDrawableObject LobbyGUI;
        IDrawableObject DeathGUI;

        public GameManager GameManager { get; private set; } = new GameManager();
        public AnimationBuilder AnimationBuilder { get; private set; } = new AnimationBuilder();
        public NetworkManager NetworkManager { get; private set; } = new NetworkManager();
        public InputManager InputManager { get; private set; } = new InputManager();
        private RenderTarget2D renderTarget;
        public AppManager()
        {
            Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 30);
            _graphics.PreferredBackBufferWidth = resolution.X;
            _graphics.PreferredBackBufferHeight = resolution.Y;
            _graphics.IsFullScreen = true;
            gameState = GameState.Menu ;
            MenuGUI = new MenuGUI();
            LoginGUI = new LoginGUI();
            OptionsGUI = new OptionsGUI();
            LobbyGUI = new LobbyGUI();
            DeathGUI = new DeathGUI();
            UIManager.resolution = resolution;
            UIManager.resolutionInGame = inGameResolution;
        }

        protected override void Initialize()
        {
            AnimationBuilder.LoadAnimations();
            MenuGUI.Initialize(GraphicsDevice);
            LoginGUI.Initialize(GraphicsDevice);

            OptionsGUI.Initialize(GraphicsDevice);

            LobbyGUI.Initialize(GraphicsDevice);
            DeathGUI.Initialize(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MenuGUI.LoadContent();
            LoginGUI.LoadContent();
            OptionsGUI.LoadContent();
            LobbyGUI.LoadContent();
            DeathGUI.LoadContent();
            GameObject.debugTexture = new Texture2D(GraphicsDevice, 1, 1);
            GameObject.debugTexture.SetData<Color>(new Color[] { new Color(1, 0,0,0.25f) });
            renderTarget = new RenderTarget2D(GraphicsDevice, inGameResolution.X, inGameResolution.Y);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();

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
                    GameManager.Update(gameTime);
                    break;
                default:
                    break;
            }

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
                case GameState.Game:
                    _spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);
                    GameManager.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;
                default:
                    break;
            }
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin();
            _spriteBatch.Draw(renderTarget, new Rectangle(0,0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.End();


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
                    GameManager.mapManager.LoadLevel("");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
