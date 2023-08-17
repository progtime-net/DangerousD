﻿using Microsoft.Xna.Framework;
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

            var lobbyBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, screenWidth, screenHeight), textureName = "menuFon3" };
            Elements.Add(lobbyBackground);
            lobbyBackground.LoadTexture(AppManager.Instance.Content);

            // CheckBoxs
            var lobby = new Label(Manager) { rectangle = new Rectangle(screenWidth / 30 * 2, screenHeight / 30 * 5,
                screenWidth / 30 * 26, screenHeight / 15 * 10), textureName = "textboxbackground2,5-1" };
            Elements.Add(lobby);
            lobby.LoadTexture(AppManager.Instance.Content);


            // Buttons
            {
                Button backButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30, screenHeight / 30, (int)(40 * 2.4), (int)(40 * 2.4)),
                    text = "<-",
                    scale = 0.72f,
                    fontColor = Color.Black,
                    fontName = "font2",
                    textureName = "textboxbackground1-1"
                };
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
                    textureName = "textboxbackground2-1"
                };
                hostButton.LeftButtonPressed += () => {
                    
                };

                Button refreshButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 6, screenHeight / 15 * 13, (int)(120 * 2.4), (int)(50 * 2.4)),
                    text = "Refresh",
                    scale = 0.48f,
                    fontColor = Color.DarkBlue,
                    fontName = "buttonFont",
                    textureName = "textboxbackground2-1"
                };
                refreshButton.LeftButtonPressed += () => {
                    
                };

                Button joinSelectedButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 25, screenHeight / 15 * 13, (int)(120 * 2.4), (int)(50 * 2.4)),
                    text = "Join",
                    scale = 0.48f,
                    fontColor = Color.DarkBlue,
                    fontName = "buttonFont",
                    textureName = "textboxbackground2-1"
                };
                joinSelectedButton.LeftButtonPressed += () => {

                };
                Button joinByIpButton = new Button(Manager)
                {
                    rectangle = new Rectangle(screenWidth / 30 * 25, screenHeight / 30, (int)(120 * 2.4), (int)(50 * 2.4)),
                    text = "JoinByIp",
                    scale = 0.48f,
                    fontColor = Color.DarkBlue,
                    fontName = "buttonFont",
                    textureName = "textboxbackground2-1"
                };
                joinByIpButton.LeftButtonPressed += () => {

                };
            }

            // SearchBar
            {
                TextBox searchBarTextBox = new TextBox(Manager) {
                    rectangle = new Rectangle(screenWidth / 30 * 14, screenHeight / 30,
                        screenWidth / 30 * 10, screenHeight / 30 * 3),
                    text = "ip",
                    scale = 0.384f,
                    fontColor = Color.White,
                    fontName = "font2",
                    textAligment = TextAligment.Left,
                    textureName = "textboxbackground6-1"

                };
                searchBarTextBox.LoadTexture(AppManager.Instance.Content);
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
            }
        }
    }
}
