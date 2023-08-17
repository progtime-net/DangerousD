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
        int TopMenu = 0;
        Color[] colors = new Color[] { new Color(64, 53, 51), new Color(84, 58, 52),
            new Color(170, 101, 63), new Color(254, 208, 116), new Color(252, 231, 124) };
        for (int i = 0; i < colors.Length; i++)
            Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2 - 60, 60, 50, 50), text = "Dangerous", mainColor = Color.Transparent, scale = 1-0.05f*i*i/10, fontName = "Font2", fontColor = colors[i] });
        
        int dx = 100;
        Color[] colors2 = new Color[] { new Color(64, 53, 51), new Color(84, 58, 52),
            new Color(170, 101, 63), new Color(254, 208, 116), new Color(252, 231, 124) };
        for (int i = 0; i < colors2.Length; i++)
            Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2 + 250 + dx-i*i, 90 - i * i/5, 50, 50), text = "D", mainColor = Color.Transparent, scale = 1.3f - 0.05f * i * i / 5, fontName = "Font2", fontColor = colors2[i] });
        
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