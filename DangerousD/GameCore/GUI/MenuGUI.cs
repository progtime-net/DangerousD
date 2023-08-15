using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Elements;

namespace DangerousD.GameCore.GUI;

internal class MenuGUI : AbstractGui
{
    protected override void CreateUI()
    {
        int wigth = AppManager.Instance.Window.ClientBounds.Width;
        int height = AppManager.Instance.Window.ClientBounds.Height;
        var butSingle = new Button(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 130, 300, 50), text = "Singleplayer", fontName = "File" };
        Elements.Add(butSingle);
        butSingle.LeftButtonPressed += () =>
        {
            AppManager.Instance.ChangeGameState(GameState.Game);
        };
        var butMulti = new Button(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 190, 300, 50), text = "Multiplayer", fontName = "File" };
        Elements.Add(butMulti);
        butMulti.LeftButtonPressed += () =>
        {
            AppManager.Instance.ChangeGameState(GameState.Login); 
        };
        var butOption = new Button(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 250, 300, 50), text = "Option", fontName = "File" };
        Elements.Add(butOption);
        butOption.LeftButtonPressed += () =>
        {
            // открытие настроек
        };
        var butExit = new Button(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 310, 300, 50), text = "Exit", fontName = "File" };
        Elements.Add(butExit);
        butExit.LeftButtonPressed += () =>
        {
            AppManager.Instance.Exit();
        };
    }
}   