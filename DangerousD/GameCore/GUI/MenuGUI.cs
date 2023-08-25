using DangerousD.GameCore.GameObjects.PlayerDeath;
using DangerousD.GameCore.Managers;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace DangerousD.GameCore.GUI;

internal class MenuGUI : AbstractGui
{
    int selected = 0;
    Color[] colors = new Color[] { new Color(64, 53, 51), new Color(84, 58, 52),
            new Color(170, 101, 63), new Color(254, 208, 116), new Color(252, 231, 124) };
    List<Label> MainLetterLabels = new List<Label>();
    List<Label> BigLetterLabels = new List<Label>();
    List<Vector2> MainLetterPositions = new List<Vector2>();
    List<Vector2> BigLetterPositions = new List<Vector2>();

    #region Вынес задний фон для обработки в апдейте
    DrawableUIElement menuBackground;
    Color mainBackgroundColor = Color.White;
    Rectangle backgrRect;
    #endregion
    protected override void CreateUI()
    {
        int width = AppManager.Instance.inGameHUDHelperResolution.X;
        int height = AppManager.Instance.inGameHUDHelperResolution.Y;
        float scaler = AppManager.Instance.inGameResolution.Y / (float)AppManager.Instance.inGameHUDHelperResolution.Y;

        menuBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, width, height), textureName = "textures\\ui\\background_menu" };
        Elements.Add(menuBackground);
        menuBackground.LoadTexture(AppManager.Instance.Content);
        backgrRect = menuBackground.rectangle;

        for (int i = 0; i < colors.Length; i++)
        {
            Elements.Add(new Label(Manager) { rectangle = new Rectangle((width - 50) / 2 - 60, 220, 50, 50), text = "Dangerous", mainColor = Color.Transparent, scale = 1.35f - 0.05f * i * i / 10, fontName = "Font2", fontColor = colors[i] });
            MainLetterLabels.Add(Elements.Last() as Label);
            MainLetterPositions.Add(new Vector2(Elements.Last().rectangle.X, Elements.Last().rectangle.Y));
        }

        int dx = 50;
        for (int i = 0; i < colors.Length; i++)
        {
            Elements.Add(new Label(Manager) { rectangle = new Rectangle((width - 50) / 2 + 480 + dx - i * i, 260 - i * i / 5, 50, 50), text = "D", mainColor = Color.Transparent, scale = 2.15f - 0.05f * i * i / 5, fontName = "Font2", fontColor = colors[i] });
            BigLetterLabels.Add(Elements.Last() as Label);
            BigLetterPositions.Add(new Vector2(Elements.Last().rectangle.X, Elements.Last().rectangle.Y));
        }

        var butSingle = new ButtonText(Manager) { rectangle = new Rectangle((width - (int)(300 * 2.4)) / 2, 350, (int)(300 * 2.4), (int)(50 * 2.4)), text = "Singleplayer", scale = 1.2f, fontName = "ButtonFont" };
        Elements.Add(butSingle);
        butSingle.LeftButtonPressed += () =>
        {
            AppManager.Instance.SoundManager.StartSound("StartGame", Vector2.Zero, Vector2.Zero);
            AppManager.Instance.ChangeGameState(GameState.Game);
            AppManager.Instance.SetMultiplayerState(MultiPlayerStatus.SinglePlayer);

        };

        var butMulti = new ButtonText(Manager) { rectangle = new Rectangle((width - (int)(300 * 2.4)) / 2, 470, (int)(300 * 2.4), (int)(50 * 2.4)), text = "Multiplayer", scale = 1.2f, fontName = "ButtonFont" };

        Elements.Add(butMulti);
        butMulti.LeftButtonPressed += () =>
        {
            AppManager.Instance.SoundManager.StartSound("reloading", Vector2.Zero, Vector2.Zero);
            AppManager.Instance.ChangeGameState(GameState.Login);
        };
        var butOption = new ButtonText(Manager) { rectangle = new Rectangle((width - (int)(160 * 2.4)) / 2, 590, (int)(160 * 2.4), (int)(50 * 2.4)), text = "Option", scale = 1.2f, fontName = "ButtonFont" };
        Elements.Add(butOption);
        butOption.LeftButtonPressed += () =>
        {
            // открытие настроек
            AppManager.Instance.SoundManager.StartSound("reloading", Vector2.Zero, Vector2.Zero);
            AppManager.Instance.ChangeGameState(GameState.Options);
        };
        var butExit = new ButtonText(Manager) { rectangle = new Rectangle((width - (int)(110 * 2.4)) / 2, 710, (int)(110 * 2.4), (int)(50 * 2.4)), text = "Exit", scale = 1.2f, fontName = "ButtonFont" };
        Elements.Add(butExit);
        butExit.LeftButtonPressed += () =>
        {
            AppManager.Instance.SoundManager.StartSound("reloading", Vector2.Zero, Vector2.Zero);
            AppManager.Instance.Exit();
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
        for (int i = 0; i < MainLetterLabels.Count; i++)
        {
            MainLetterLabels[i].fontColor = Color.FromNonPremultiplied(colors[i].ToVector4() *
                (float)(((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) + 1) / 2f) * 0.3 + 0.8f)
                );
            BigLetterLabels[i].fontColor = Color.FromNonPremultiplied(colors[i].ToVector4()
                * (float)(((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4 - Math.PI) + 1) / 2f) * 0.3 + 0.8f)
                );
            MainLetterLabels[i].fontColor.A = 255;
            BigLetterLabels[i].fontColor.A = 255;
            MainLetterLabels[i].rectangle.Y = (int)(MainLetterPositions[i].Y +
                 (20 * (Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) + 1) / 2f * 0.25) * (i - MainLetterLabels.Count / 2)
                );
            BigLetterLabels[i].rectangle.Y = (int)(BigLetterPositions[i].Y +
                 (20 * (Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4 - Math.PI) + 1) / 2f * 0.25) * (i - MainLetterLabels.Count / 2)
                );

        }
        menuBackground.mainColor = Color.FromNonPremultiplied(mainBackgroundColor.ToVector4()
                * (float)(((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1 - 2 * Math.PI) + 1) / 2f) * 0.2 + 0.6f)
                );
        double backgrSpeed = 0.25;
        double procent = 0.1f;
        menuBackground.rectangle.X = backgrRect.X - (int)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * backgrSpeed - 2 * Math.PI) + 1
            ) * backgrRect.Width * procent);
        menuBackground.rectangle.Y = backgrRect.Y - (int)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * backgrSpeed - 2 * Math.PI) + 1
            ) * backgrRect.Height * procent);
        menuBackground.rectangle.Width = backgrRect.Width + 2 * (int)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * backgrSpeed - 2 * Math.PI) + 1
            ) * backgrRect.Width * procent);
        menuBackground.rectangle.Height = backgrRect.Height + 2 * (int)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * backgrSpeed - 2 * Math.PI) + 1
            ) * backgrRect.Height * procent);
        base.Update(gameTime);
    }
}