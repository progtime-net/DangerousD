using System;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System.Xml.Linq;
using DangerousD.GameCore.Managers;
using DangerousD.GameCore;

namespace DangerousD.GameCore.GUI
{
	public class OptionsGUI : AbstractGui
	{
        int selectedGUI = 0;
        protected override void CreateUI()
        {
            int wigth = AppManager.Instance.inGameHUDHelperResolution.X;
            int height = AppManager.Instance.inGameHUDHelperResolution.Y;
            float scaler = AppManager.Instance.resolution.Y / (float)AppManager.Instance.inGameHUDHelperResolution.Y;
            var menuBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, wigth, height), textureName = "optionsBackground" };
            //Elements.Add(menuBackground);
            menuBackground.LoadTexture(AppManager.Instance.Content);

            var slider = new Slider(Manager)
            {
                MinValue = 0,
                MaxValue = 1,
                rectangle = new Rectangle(wigth / 2 + 220, 275, (int)(100 * 2.4), 40),
                indentation = 5,
                textureName = "sliderBackground"
            };
            //Elements.Add(slider);
            //AppManager.Instance.SettingsManager.SetMainVolume(slider.GetSliderValue);

            var cB = new CheckBox(Manager);
            cB.rectangle = new Rectangle(wigth / 2 + 440, 405, (int)(40 * 2.4), (int)(40 * 2.4));
            cB.Checked += (newCheckState) =>
            {
                SettingsManager sM = new SettingsManager();
            };
            cB.LoadTexture(AppManager.Instance.Content);
            Elements.Add(cB);

            var checkBox = new CheckBox(Manager);
            checkBox.rectangle = new Rectangle(wigth / 2 + 360, 540, (int)(40 * 2.4), (int)(40 * 2.4));
            checkBox.Checked += (newCheckState) =>
            {
                AppManager.Instance.SettingsManager.SetIsFullScreen(newCheckState);
            };
            checkBox.LoadTexture(AppManager.Instance.Content);
            Elements.Add(checkBox);

            Label lblOptions = new Label(Manager)
            { fontName = "buttonFont", scale = 1.2f, text = "Options", fontColor = Color.White, rectangle = new Rectangle((wigth - 50) / 2, 40, 50, 50),  mainColor = Color.Transparent };
            Elements.Add(lblOptions);

            Label lblValue = new Label(Manager)
            { fontName = "buttonFont", scale = 1f , text = "Volume", fontColor = Color.White, rectangle = new Rectangle((wigth - 50) / 2, 250, 50, 50), mainColor = Color.Transparent };
            Elements.Add(lblValue);

            Label lblIsFullScreen = new Label(Manager)
            { fontName = "buttonFont", scale = 1f , text = "Full Screen", fontColor = Color.White, rectangle = new Rectangle((wigth - 50) / 2, 580, 50, 50), mainColor = Color.Transparent };
            Elements.Add(lblIsFullScreen);

            Label lblSwitchMode = new Label(Manager)
            { fontName = "buttonFont", scale = 1f, text = "Left/Right Mode", fontColor = Color.White, rectangle = new Rectangle((wigth - 50) / 2, 415, 50, 50),  mainColor = Color.Transparent };
            Elements.Add(lblSwitchMode);

            Button bTExit = new Button(Manager) 
            { fontName = "Font2", scale = 0.72f, text = "<-", rectangle = new Rectangle(wigth / 30, height / 30, (int)(40 * 2.4), (int)(40 * 2.4)), textureName = "textboxbackground1-1" };
            Elements.Add(bTExit);
            bTExit.LeftButtonPressed += () => 
            {
                AppManager.Instance.ChangeGameState(GameState.Menu);
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
            slider.rectangle.X = (int)(scaler * slider.rectangle.X);
            slider.rectangle.Y = (int)(scaler * slider.rectangle.Y);
            //slider.rectangle.Width = (int)(scaler * slider.rectangle.Width);
            //slider.rectangle.Height = (int)(scaler * slider.rectangle.Height);
            if (slider is DrawableTextedUiElement)
            {
                (slider as DrawableTextedUiElement).scale *= scaler;
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    } 
}