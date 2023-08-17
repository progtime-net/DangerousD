using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using DangerousD.GameCore.Managers;
using MonogameLibrary.UI.Base;
using System.Diagnostics;

using MonogameLibrary.UI.Elements;
using MonogameLibrary.UI.Enums;

namespace DangerousD.GameCore.GUI
{
    class LoginGUI : AbstractGui
    {
        private string username;
        private string password;

        public string Username { get => username; }
        public string Password { get => password; }

        protected override void CreateUI()
        {
            int screenWidth = AppManager.Instance.inGameResolution.X;
            int screenHeight = AppManager.Instance.inGameResolution.Y;

            var loginBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, screenWidth, screenHeight), textureName = "menuFon2" };
            Elements.Add(loginBackground);
            loginBackground.LoadTexture(AppManager.Instance.Content);

            Elements.Add(new Label(Manager) {
                rectangle = new Rectangle(screenWidth / 2 - 250, screenHeight / 6 - 50, 500, 100),
                text = "Login",
                scale = 0.8f,
                fontColor = Color.White,
                mainColor = Color.Transparent,
                fontName = "ButtonFont"
            });

            // TextBox-ы
            {
                TextBox loginTextBox = new TextBox(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 2 - 125, screenHeight / 6 * 2 - 20, 250, 40),
                    text = "NickName",
                    scale = 0.16f,
                    fontColor = Color.Gray,
                    fontName = "Font2",
                    textAligment = TextAligment.Left,
                    textureName = "textboxbackground"

                };
                loginTextBox.LoadTexture(AppManager.Instance.Content);
                loginTextBox.TextChanged += input => {
                    if (loginTextBox.fontColor == Color.Gray)
                    {
                        loginTextBox.text = ""; loginTextBox.fontColor = Color.Black;
                    }
                };
                loginTextBox.StopChanging += input => {
                    if (input.Length == 0)
                    {
                        loginTextBox.text = "NickName";
                        loginTextBox.fontColor = Color.Gray;
                    }
                };

                TextBox passwordTextBox = new TextBox(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 2 - 125, screenHeight / 6 * 3 - 40, 250, 40),
                    text = "Password",
                    scale = 0.16f,
                    fontColor = Color.Gray,
                    fontName = "font2",
                    textAligment = TextAligment.Left,
                    textureName = "textboxbackground"
                };
                passwordTextBox.LoadTexture(AppManager.Instance.Content);
                passwordTextBox.TextChanged += input => {
                    if (passwordTextBox.fontColor == Color.Gray)
                    {
                        passwordTextBox.text = ""; passwordTextBox.fontColor = Color.Black;
                    }
                };
                passwordTextBox.StopChanging += input => {
                    if (input.Length == 0)
                    {
                        passwordTextBox.text = "Password";
                        passwordTextBox.fontColor = Color.Gray;
                    }
                };
            }

            // Кнопки
            {
                Button logButton = new ButtonText(Manager) {
                    rectangle = new Rectangle(screenWidth / 4 + 50, screenHeight / 6 * 4, 100, 50),
                    text = "LogIn",
                    scale = 0.2f,
                    fontColor = Color.Black,
                    fontName = "font2"
                };
                logButton.LeftButtonPressed += () => {
                    if (CheckUser())
                    {
                        AppManager.Instance.ChangeGameState(GameState.Lobby);
                    }
                };

                Button regButton = new ButtonText(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 4 * 2 + 50, screenHeight / 6 * 4, 100, 50),
                    text = "Reg",
                    scale = 0.2f,
                    fontColor = Color.Black,
                    fontName = "font2"
                };
                regButton.LeftButtonPressed += GoToRegWebServer;

                Button backButton = new ButtonText(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 20, screenHeight / 15, 50, 50),
                    text = "<-",
                    scale = 0.3f,
                    fontColor = Color.Black,
                    fontName = "font2"
                };
                backButton.LeftButtonPressed += () => {
                    AppManager.Instance.ChangeGameState(GameState.Menu);
                };
            }
        }

        private void GoToRegWebServer()
        {
            // TODO
        }
        private bool CheckUser()
        {
            return true;
        }
    }
}
