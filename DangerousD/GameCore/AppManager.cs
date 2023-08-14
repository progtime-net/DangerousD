using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input; 
using DangerousD.GameCore.HUD;

namespace DangerousD.GameCore
{
    public enum GameState { Menu, Options, Lobby, Game }
    public class AppManager : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        GameState gameState;
        IHUD MenuGUI;
        IHUD OptionsGUI;
        IHUD LobbyGUI;
        public AppManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


            gameState = GameState.Menu;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.contentManager = Content;
            TextureManager.graphicsDevice = GraphicsDevice;
            MenuGUI = new HUD.MenuHUD();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameState.Menu:
                    MenuGUI.Update();
                    break;
                case GameState.Options:
                    OptionsGUI.Update();
                    break;
                case GameState.Lobby:
                    LobbyGUI.Update();
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
                case GameState.Lobby:
                    LobbyGUI.Draw(_spriteBatch);
                    break;
                case GameState.Game:
                    GameManager.Draw(_spriteBatch);
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }

        public void ChangeGameState(GameState gameState)
        {
            this.gameState = gameState;
        }

    }
}
