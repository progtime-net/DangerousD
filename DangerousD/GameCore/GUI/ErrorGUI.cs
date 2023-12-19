using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Elements;

namespace DangerousD.GameCore.GUI;

public class ErrorGUI : AbstractGui
{
    
    // TODO: soooo bad
    protected override void CreateUI()
    {
        int screenWidth = AppManager.Instance.inGameResolution.X;
        int screenHeight = AppManager.Instance.inGameResolution.Y;
        
        Elements.Add(new Label(Manager)
        {
            rectangle = new Rectangle(screenWidth / 2 - (int)(250 * 2.4), screenHeight / 6 - 100, (int)(500 * 2.4),
                (int)(100 * 2.4)),
            text = "ERROR!",
            scale = 1.7f,
            fontColor = Color.White,
            mainColor = Color.Transparent,
            fontName = "Font_2"
        });
        
        Elements.Add(new Label(Manager)
        {
            rectangle = new Rectangle(screenWidth / 2 - (int)(250 * 2.4), screenHeight / 6 + 60, (int)(500 * 2.4),
                (int)(100 * 2.4)),
            scale = 1f,
            fontColor = Color.White,
            mainColor = Color.Transparent,
            fontName = "PixelFont"
        });

        Elements.Add(new Label(Manager)
        {
            rectangle = new Rectangle(screenWidth / 2 - (int)(250 * 2.4), screenHeight / 6 + 100, (int)(500 * 2.4),
                (int)(100 * 2.4)),
            scale = 1f,
            fontColor = Color.White,
            mainColor = Color.Transparent,
            fontName = "PixelFont"
        });
        
        Button btn = new Button(Manager)
        {
            rectangle = new Rectangle(screenWidth / 2 - (int)(80 * 2.4) / 2, screenHeight / 2, (int)(80 * 2.4), (int)(40 * 2.4)), 
            text = "Reset",
            fontColor = Color.Black,
            fontName = "font2",
            textureName = "textures\\ui\\textbox_background1-1"
        };
        btn.LeftButtonPressed += () => {
            AppManager.Instance.Restart(AppManager.Instance.currentMap);
        };
        Elements.Add(btn);

    }
    
    public void Raise(string value)
    {
        ((Label)Elements[1]).text = $"(Map {AppManager.Instance.currentMap}; Mode {AppManager.Instance.multiPlayerStatus})";
        ((Label)Elements[2]).text = value;
        AppManager.Instance.ChangeGameState(GameState.Error);
    }
}