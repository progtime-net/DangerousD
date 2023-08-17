using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DangerousD.GameCore.GUI;

internal class MenuGUI : AbstractGui
{
    int selected = 0;
    protected override void CreateUI()
    {
        int wigth = AppManager.Instance.inGameResolution.X;
        int height = AppManager.Instance.inGameResolution.Y;
        
        var menuBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, wigth, height), textureName = "menuFon" };
        Elements.Add(menuBackground);
        menuBackground.LoadTexture(AppManager.Instance.Content);

        Color[] colors = new Color[] { new Color(64, 53, 51), new Color(84, 58, 52),
            new Color(170, 101, 63), new Color(254, 208, 116), new Color(252, 231, 124) };
        for (int i = 0; i < colors.Length; i++)
            Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2 - 60, 200, 50, 50), text = "Dangerous", mainColor = Color.Transparent, scale = 1.35f - 0.05f * i * i / 10, fontName = "Font2", fontColor = colors[i] });

        int dx = 50;
        Color[] colors2 = new Color[] { new Color(64, 53, 51), new Color(84, 58, 52),
            new Color(170, 101, 63), new Color(254, 208, 116), new Color(252, 231, 124) };
        for (int i = 0; i < colors2.Length; i++)
            Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2 + 480 + dx - i * i, 260 - i * i / 5, 50, 50), text = "D", mainColor = Color.Transparent, scale = 2.15f - 0.05f * i * i / 5, fontName = "Font2", fontColor = colors2[i] });

        var butSingle = new ButtonText(Manager) { rectangle = new Rectangle((wigth - (int)(300 * 2.4)) / 2, 350, (int)(300 * 2.4), (int)(50 * 2.4)), text = "Singleplayer", scale = 1.2f, fontName = "ButtonFont" };
        Elements.Add(butSingle);
        butSingle.LeftButtonPressed += () =>
        {
            AppManager.Instance.ChangeGameState(GameState.Game);
            AppManager.Instance.SetMultiplayerState(MultiPlayerStatus.SinglePlayer);
        };

        var butMulti = new ButtonText(Manager) { rectangle = new Rectangle((wigth - (int)(300 * 2.4)) / 2, 470, (int)(300 * 2.4), (int)(50 * 2.4)), text = "Multiplayer", scale = 1.2f, fontName = "ButtonFont" };

        Elements.Add(butMulti);
        butMulti.LeftButtonPressed += () =>
        {
            AppManager.Instance.ChangeGameState(GameState.Login); 
        };
        var butOption = new ButtonText(Manager) { rectangle = new Rectangle((wigth - (int)(160 * 2.4)) / 2, 590, (int)(160 * 2.4), (int)(50 * 2.4)), text = "Option", scale = 1.2f, fontName = "ButtonFont" };
        Elements.Add(butOption);
        butOption.LeftButtonPressed += () =>
        {
            // открытие настроек
            AppManager.Instance.ChangeGameState(GameState.Options);
        };
        var butExit = new ButtonText(Manager) { rectangle = new Rectangle((wigth - (int)(110 * 2.4)) / 2, 710, (int)(110 * 2.4), (int)(50 * 2.4)), text = "Exit", scale = 1.2f, fontName = "ButtonFont" };
        Elements.Add(butExit);
        butExit.LeftButtonPressed += () =>
        {
            AppManager.Instance.Exit();
        };
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}   