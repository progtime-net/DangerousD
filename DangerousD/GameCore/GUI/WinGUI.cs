using DangerousD.GameCore.Managers;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DangerousD.GameCore.GUI;

internal class WinGUI : AbstractGui
{
    
    protected override void CreateUI()
    {
        int wigth = AppManager.Instance.inGameResolution.X;
        int height = AppManager.Instance.inGameResolution.Y;
        float scaler = AppManager.Instance.inGameResolution.Y / (float)AppManager.Instance.inGameHUDHelperResolution.Y;
        var menuBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, wigth, height), textureName = "textures\\ui\\background_win" };
        Elements.Add(menuBackground);
        menuBackground.LoadTexture(AppManager.Instance.Content);
        Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2, (height - 50) / 2 - 80, 50, 50), text = "You escaped!", mainColor = Color.Transparent, scale = 0.7f, fontName = "ButtonFont", fontColor = Color.White });
        Elements.Add(new Label(Manager) { rectangle = new Rectangle((wigth - 50) / 2, (height - 50) / 2, 50, 50), text = $"Score: {0}", mainColor = Color.Transparent, scale = 0.7f, fontName = "ButtonFont", fontColor = Color.White });
        var butMenu = new ButtonText(Manager) { rectangle = new Rectangle((wigth - 300) / 2, (height - 50) / 2 + 80, 300, 50), text = "Back to menu", scale = 0.7f, fontName = "ButtonFont" };
        Elements.Add(butMenu);
        butMenu.LeftButtonPressed += () =>
        {
            AppManager.Instance.SoundManager.StartSound("reloading", Vector2.Zero, Vector2.Zero);
            AppManager.Instance.Restart("lvl1");
        };
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

    public override void Update(GameTime gameTime)
    {
        (Elements[2] as DrawableTextedUiElement).text = $"Score: {AppManager.Instance.GameManager.GetPlayer1.score}";
        base.Update(gameTime);
    }
}