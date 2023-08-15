﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using DangerousD.GameCore.GUI;
using Microsoft.Xna.Framework.Input;
using DangerousD.GameCore.Graphics;

namespace DangerousD.GameCore
{
    public enum GameState { Menu, Options, Lobby, Game, Login }
    public class AppManager : Game
    {
        public static AppManager Instance { get; private set;  }
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Point resolution;
        GameState gameState;
        IDrawableObject MenuGUI;
        IDrawableObject OptionsGUI;
        IDrawableObject LoginGUI;
        IDrawableObject LobbyGUI;

        public GameManager GameManager { get; private set; }
        public AnimationBuilder AnimationBuilder { get; private set; } = new AnimationBuilder();
        public AppManager()
        {
            Instance = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 30);

            resolution = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            GameManager = new GameManager();
            gameState = GameState.Menu;
            MenuGUI = new MenuGUI();
            LoginGUI = new LoginGUI();
        }

        protected override void Initialize()
        {
            MenuGUI.Initialize(GraphicsDevice);
            LoginGUI.Initialize(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            MenuGUI.LoadContent();
            LoginGUI.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
                case GameState.Game:
                    _spriteBatch.Begin();
                    GameManager.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;
                default:
                    break;
            }

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
