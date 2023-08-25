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
using System.Xml.Linq;

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
            float scaler = AppManager.Instance.inGameResolution.Y / (float)AppManager.Instance.inGameHUDHelperResolution.Y;

            var lobbyBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, screenWidth, screenHeight), textureName = "textures\\ui\\background_menu_3" };
            Elements.Add(lobbyBackground);
            lobbyBackground.LoadTexture(AppManager.Instance.Content);

            // CheckBoxs
            var lobby = new Label(Manager) { rectangle = new Rectangle(screenWidth / 30 * 2, screenHeight / 30 * 5,
                screenWidth / 30 * 26, screenHeight / 15 * 10), textureName = "textures\\ui\\textbox_background2,5-1"
            };
            Elements.Add(lobby);
            lobby.LoadTexture(AppManager.Instance.Content);


            // Buttons and ip textbox
            {
                TextBox searchBarTextBox = new TextBox(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 14, screenHeight / 30,
                        screenWidth / 30 * 10, screenHeight / 30 * 3),
                    text = "ip",
                    scale = 0.16f,
                    fontColor = Color.Black,
                    fontName = "font2",
                    textAligment = TextAligment.Left,
                    textureName = "textures\\ui\\textbox_background6-1"

                };
                Elements.Add(searchBarTextBox);
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
                Button backButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30, screenHeight / 30, (int)(40 * 2.4), (int)(40 * 2.4)),
                    text = "<-",
                    scale = 0.72f,
                    fontColor = Color.Black,
                    fontName = "font2",
                    textureName = "textures\\ui\\textbox_background1-1"
                };
                Elements.Add(backButton);
                backButton.LeftButtonPressed += () => {
                    AppManager.Instance.ChangeGameState(GameState.Menu);
                };

                Button hostButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30, screenHeight / 15 * 13, (int)(120 * 2.4), (int)(50 * 2.4)),
                    text = "Host",
                    scale = 0.48f,
                    fontColor = Color.DarkBlue,
                    fontName = "buttonFont",
                    textureName = "textures\\ui\\textbox_background2-1"
                };
                Elements.Add(hostButton);
                hostButton.LeftButtonPressed += () => {
                    AppManager.Instance.ChangeGameState(GameState.Game);
                    AppManager.Instance.NetworkManager.HostInit(AppManager.Instance.IpAddress);

                };

                Button refreshButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 6, screenHeight / 15 * 13, (int)(120 * 2.4), (int)(50 * 2.4)),
                    text = "Refresh",
                    scale = 0.48f,
                    fontColor = Color.DarkBlue,
                    fontName = "buttonFont",
                    textureName = "textures\\ui\\textbox_background2-1"
                };
                Elements.Add(refreshButton);
                refreshButton.LeftButtonPressed += () => {
                    
                };

                Button joinSelectedButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 25, screenHeight / 15 * 13, (int)(120 * 2.4), (int)(50 * 2.4)),
                    text = "Join",
                    scale = 0.48f,
                    fontColor = Color.DarkBlue,
                    fontName = "buttonFont",
                    textureName = "textures\\ui\\textbox_background2-1"
                };
                Elements.Add(joinSelectedButton);
                joinSelectedButton.LeftButtonPressed += () => {
                    AppManager.Instance.ChangeGameState(GameState.Game);
                    AppManager.Instance.NetworkManager.ClientInit(AppManager.Instance.IpAddress);
                };
                Button joinByIpButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 25, screenHeight / 30, (int)(120 * 2.4), (int)(50 * 2.4)),
                    text = "JoinByIp",
                    scale = 0.48f,
                    fontColor = Color.DarkBlue,
                    fontName = "buttonFont",
                    textureName = "textures\\ui\\textbox_background2-1"
                };
                Elements.Add(joinByIpButton);
                joinByIpButton.LeftButtonPressed += () => {
                    AppManager.Instance.ChangeGameState(GameState.Game);
                    AppManager.Instance.NetworkManager.ClientInit(searchBarTextBox.text);
                };
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
    }
}
