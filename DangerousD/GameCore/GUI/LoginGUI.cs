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
            float scaler = AppManager.Instance.inGameResolution.Y / (float)AppManager.Instance.inGameHUDHelperResolution.Y;

            var loginBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, screenWidth, screenHeight), textureName = "menuFon2" };
            Elements.Add(loginBackground);
            loginBackground.LoadTexture(AppManager.Instance.Content);

            Elements.Add(new Label(Manager) {
                rectangle = new Rectangle(screenWidth / 2 - (int)(250 * 2.4), screenHeight / 6 - 100, (int)(500 * 2.4), (int)(100 * 2.4)),
                text = "Login",
                scale = 1.7f,
                fontColor = Color.White,
                mainColor = Color.Transparent,
                fontName = "ButtonFont"
            });

            Button backButton = new Button(Manager)
            {
                rectangle = new Rectangle(screenWidth / 20, screenHeight / 15, (int)(40 * 2.4), (int)(40 * 2.4)), 
                fontColor = Color.Black,
                fontName = "font2",
                textureName = "textboxbackground1-1"
            };
            backButton.LeftButtonPressed += () => {
                AppManager.Instance.ChangeGameState(GameState.Menu);
            };
            Elements.Add(backButton);
             

            // TextBox-ы
            {
                TextBox loginTextBox = new TextBox(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 2 - (int)(125 * 2.4), screenHeight / 6 * 2 - 20, (int)(250 * 2.4), (int)(40 * 2.4)),
                    text = "NickName",
                    scale = 0.33f,
                    fontColor = Color.Gray,
                    fontName = "Font2",
                    textAligment = TextAligment.Left,
                    textureName = "textboxbackground6-1"

                };
                Elements.Add(loginTextBox);
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
                Elements.Add(loginTextBox);

                TextBox passwordTextBox = new TextBox(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 2 - (int)(125 * 2.4), screenHeight / 6 * 3 - 40, (int)(250 * 2.4), (int)(40 * 2.4)),
                    text = "Password",
                    scale = 0.33f,
                    fontColor = Color.Gray,
                    fontName = "font2",
                    textAligment = TextAligment.Left,
                    textureName = "textboxbackground6-1"
                };
                Elements.Add(passwordTextBox);
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
                Elements.Add(passwordTextBox);
            }

            // Кнопки
            {
                Button logButton = new Button(Manager) {
                    rectangle = new Rectangle(screenWidth / 4 + (int)(50 * 2.4), screenHeight / 6 * 3 + 100, (int)(100 * 2.4), (int)(50 * 2.4)),
                    text = "LogIn",
                    scale = 0.6f,
                    fontColor = Color.White,
                    fontName = "ButtonFont",
                    textureName = "textboxbackground2-1"
                };
                Elements.Add(logButton);
                logButton.LeftButtonPressed += () => {
                    if (CheckUser())
                    {
                        AppManager.Instance.ChangeGameState(GameState.Lobby);
                    }
                };
                Elements.Add(logButton);

                Button regButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 4 * 2 + (int)(50 * 2.4), screenHeight / 6 * 3 + 100, (int)(100 * 2.4), (int)(50 * 2.4)),
                    text = "Reg",
                    scale = 0.6f,
                    fontColor = Color.White,
                    fontName = "ButtonFont",
                    textureName = "textboxbackground2-1"
                };
                Elements.Add(regButton);
                regButton.LeftButtonPressed += GoToRegWebServer;
                Elements.Add(regButton);


            }
            foreach (var item in Elements)
            {
                item.rectangle.X = (int)(scaler * item.rectangle.X);
                item.rectangle.Y = (int)(scaler * item.rectangle.Y);
                item.rectangle.Width = (int)(scaler * item.rectangle.Width);
                item.rectangle.Height = (int)(scaler * item.rectangle.Height);
                if (item is DrawableTextedUiElement)
                {
                    (item as DrawableTextedUiElement).scale *= scaler;
                }
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
