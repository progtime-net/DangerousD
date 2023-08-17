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
using System.Net;
using System.IO;

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
                    textAligment = TextAligment.Left
                };
                loginTextBox.TextChanged += input => {
                    if (loginTextBox.fontColor == Color.Gray)
                    {
                        loginTextBox.text = ""; loginTextBox.fontColor = Color.Black;
                    }
                    username = loginTextBox.text;
                };
                loginTextBox.StopChanging += input => {
                    if (input.Length == 0)
                    {
                        loginTextBox.text = "NickName/Email";
                        loginTextBox.fontColor = Color.Gray;
                    }
                    username = loginTextBox.text;
                };

                TextBox passwordTextBox = new TextBox(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 2 - 125, screenHeight / 6 * 3 - 40, 250, 40),
                    text = "Password",
                    scale = 0.16f,
                    fontColor = Color.Gray,
                    fontName = "font2",
                    textAligment = TextAligment.Left
                };
                passwordTextBox.TextChanged += input => {
                    if (passwordTextBox.fontColor == Color.Gray)
                    {
                        passwordTextBox.text = ""; passwordTextBox.fontColor = Color.Black;
                    }
                    password = passwordTextBox.text;
                };
                passwordTextBox.StopChanging += input => {
                    if (input.Length == 0)
                    {
                        passwordTextBox.text = "Password";
                        passwordTextBox.fontColor = Color.Gray;
                    }
                    password = passwordTextBox.text;
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
            string target = "http://dangerousd.1gb.ru/Registration";

            /*
            System.Diagnostics.Process.Start(target);

            //Use no more than one assignment when you test this code.
            //string target = "ftp://ftp.microsoft.com";
            //string target = "C:\\Program Files\\Microsoft Visual Studio\\INSTALL.HTM";
            try
            {
                System.Diagnostics.Process.Start(target);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                //if (noBrowser.ErrorCode == -2147467259)
                    //MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                //MessageBox.Show(other.Message);
            }
            */
        }
        private bool CheckUser()
        {
            //string email = "a@a";
            //string pass = "123456";

            string email = username;
            string pass = password;

           // Manager.

            var request = (HttpWebRequest)WebRequest.Create($"http://dangerousd.1gb.ru/logingame?emailusername={email}&password={password}");
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // responseString => {"answerCode":false,"email":null,"username":null}
            // {"answerCode":true,"email":"a@a","username":"Firest"}

            return GetAnswer(responseString);
        }

        // {"answerCode":false,"email":null,"username":null}
        private bool GetAnswer(string response)
        {
            string[] answerCode = response.Split(':');
            answerCode = answerCode[1].Split(',');



            if (answerCode[0] == "false")
            {
                return false;
            }

            return true;
        }

    }
}
