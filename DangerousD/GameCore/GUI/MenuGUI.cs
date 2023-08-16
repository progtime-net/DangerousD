using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DangerousD.GameCore.GUI;

internal class MenuGUI : AbstractGui
{
    protected override void CreateUI()
    {
        

        int wigth = AppManager.Instance.Window.ClientBounds.Width;
        int height = AppManager.Instance.Window.ClientBounds.Height;
        var menuBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, wigth, height), textureName = "menuFon" };
        Elements.Add(menuBackground);
        menuBackground.LoadTexture(AppManager.Instance.Content);
        Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2 - 60, 60, 50, 50), text = "Dangerous", mainColor = Color.Transparent, scale = 0.7f, fontName = "Font2", fontColor = Color.White });
        Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2 + 250, 90, 50, 50), text = "D", mainColor = Color.Transparent, scale = 1.2f, fontName = "Font2", fontColor = Color.White });
        var butSingle = new ButtonText(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 130, 300, 50), text = "Singleplayer", fontName = "ButtonFont" };
        Elements.Add(butSingle);
        butSingle.LeftButtonPressed += () =>
        {
            AppManager.Instance.ChangeGameState(GameState.Game);
        };
        var butMulti = new ButtonText(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 190, 300, 50), text = "Multiplayer", fontName = "ButtonFont" };
        Elements.Add(butMulti);
        butMulti.LeftButtonPressed += () =>
        {
            AppManager.Instance.ChangeGameState(GameState.Login); 
        };
        var butOption = new ButtonText(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 250, 300, 50), text = "Option", fontName = "ButtonFont" };
        Elements.Add(butOption);
        butOption.LeftButtonPressed += () =>
        {
            // открытие настроек
        };
        var butExit = new ButtonText(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 310, 300, 50), text = "Exit", fontName = "ButtonFont" };
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