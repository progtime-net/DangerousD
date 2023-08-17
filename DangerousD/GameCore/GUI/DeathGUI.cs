using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DangerousD.GameCore.GUI;

internal class DeathGUI : AbstractGui
{
    protected override void CreateUI()
    {
        int wigth = AppManager.Instance.Window.ClientBounds.Width;
        int height = AppManager.Instance.Window.ClientBounds.Height;
        var menuBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, wigth, height), textureName = "deathBackground" };
        Elements.Add(menuBackground);
        menuBackground.LoadTexture(AppManager.Instance.Content);
        Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2, 120, 50, 50), text = "You death", mainColor = Color.Transparent, scale = 0.5f, fontName = "ButtonFont", fontColor = Color.White });
        Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2, 190, 50, 50), text = $"Score = {0}", mainColor = Color.Transparent, scale = 0.5f, fontName = "ButtonFont", fontColor = Color.White });
        var butMenu = new ButtonText(Manager) { rectangle = new Rectangle((wigth - 300) / 2, 260, 300, 50), text = "Back to menu", fontName = "ButtonFont" };
        Elements.Add(butMenu);
        butMenu.LeftButtonPressed += () =>
        {
            AppManager.Instance.ChangeGameState(GameState.Menu);
        };
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}