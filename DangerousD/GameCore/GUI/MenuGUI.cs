using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Elements;

namespace DangerousD.GameCore.GUI;

internal class MenuGUI : AbstractGui
{
    protected override void CreateUI()
    {
        Elements.Add(new CheckBox(Manager) { rectangle = new Rectangle(10, 10, 50, 50) });
        var but = new Button(Manager) { rectangle = new Rectangle(100, 10, 80, 50) };
        Elements.Add(but);
        but.LeftButtonPressed += () =>
        {
            AppManager.AppManagerInstance.ChangeGameState(GameState.Game);

            GameManager.mapManager.LoadLevel("");
        };
    }
}