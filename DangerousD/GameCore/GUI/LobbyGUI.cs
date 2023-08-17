using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Elements;
using MonogameLibrary.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DangerousD.GameCore.Managers;
using MonogameLibrary.UI.Base;
using System.Diagnostics;
using DangerousD.GameCore.Network;

namespace DangerousD.GameCore.GUI
{
    class LobbyGUI : AbstractGui
    {
        public LobbyGUI()
        {
            
        }
        protected override void CreateUI()
        {
            int screenWidth = AppManager.Instance.inGameResolution.X;
            int screenHeight = AppManager.Instance.inGameResolution.Y;

            var lobbyBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, screenWidth, screenHeight), textureName = "menuFon3" };
            Elements.Add(lobbyBackground);
            lobbyBackground.LoadTexture(AppManager.Instance.Content);

            // CheckBoxs
            Elements.Add(new Label(Manager) { rectangle = new Rectangle(screenWidth / 30 * 2, screenHeight / 30 * 5,
                screenWidth / 30 * 26, screenHeight / 15 * 10) });

            // Buttons and ip textbox
            {
                TextBox searchBarTextBox = new TextBox(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 14, screenHeight / 30,
                        screenWidth / 30 * 10, screenHeight / 30 * 3),
                    text = "ip",
                    scale = 0.16f,
                    fontColor = Color.Gray,
                    fontName = "font2",
                    textAligment = TextAligment.Left

                };
                searchBarTextBox.TextChanged += input => {
                    if (searchBarTextBox.fontColor == Color.Gray)
                    {
                        searchBarTextBox.text = ""; searchBarTextBox.fontColor = Color.Black;
                    }
                };
                searchBarTextBox.StopChanging += input => {
                    if (input.Length == 0)
                    {
                        searchBarTextBox.fontColor = Color.Gray;
                        searchBarTextBox.text = "ip";
                    }
                };
                Button backButton = new ButtonText(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30, screenHeight / 30, 60, 50),
                    text = "<-",
                    scale = 0.3f,
                    fontColor = Color.Black,
                    fontName = "font2"
                };
                backButton.LeftButtonPressed += () => {
                    AppManager.Instance.ChangeGameState(GameState.Menu);
                };

                Button hostButton = new ButtonText(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30, screenHeight / 15 * 13, 120, 50),
                    text = "Host",
                    scale = 0.2f,
                    fontColor = Color.Black,
                    fontName = "font2"
                };
                hostButton.LeftButtonPressed += () => {
                    AppManager.Instance.ChangeGameState(GameState.Game);
                    AppManager.Instance.NetworkManager.HostInit(AppManager.Instance.IpAddress);

                };

                Button refreshButton = new ButtonText(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 6, screenHeight / 15 * 13, 120, 50),
                    text = "Refresh",
                    scale = 0.2f,
                    fontColor = Color.Black,
                    fontName = "font2"
                };
                refreshButton.LeftButtonPressed += () => {
                    
                };

                Button joinSelectedButton = new ButtonText(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 25, screenHeight / 15 * 13, 120, 50),
                    text = "Join",
                    scale = 0.2f,
                    fontColor = Color.Black,
                    fontName = "font2"
                };
                joinSelectedButton.LeftButtonPressed += () => {
                    AppManager.Instance.ChangeGameState(GameState.Game);
                    AppManager.Instance.NetworkManager.ClientInit(AppManager.Instance.IpAddress);
                };
                Button joinByIpButton = new ButtonText(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 25, screenHeight / 30, 120, 50),
                    text = "JoinByIp",
                    scale = 0.2f,
                    fontColor = Color.Black,
                    fontName = "font2"
                };
                joinByIpButton.LeftButtonPressed += () => {
                    AppManager.Instance.NetworkManager.ClientInit(searchBarTextBox.text);
                };
            }
        }
    }
}
