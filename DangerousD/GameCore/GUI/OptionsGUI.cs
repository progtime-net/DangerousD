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
            int width = AppManager.Instance.inGameHUDHelperResolution.X;
            int height = AppManager.Instance.inGameHUDHelperResolution.Y;
            float scaler = AppManager.Instance.inGameResolution.Y / (float)AppManager.Instance.inGameHUDHelperResolution.Y;
            var menuBackground = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, width, height), textureName = "textures\\ui\\background_options" };
            //Elements.Add(menuBackground);
            menuBackground.LoadTexture(AppManager.Instance.Content);


            //Elements.Add(slider);
            //AppManager.Instance.SettingsManager.SetMainVolume(slider.GetSliderValue);



            Label lblOptions = new Label(Manager)
            { fontName = "buttonFont", scale = 1.2f, text = "Options", fontColor = Color.White, rectangle = new Rectangle((width - 50) / 2, 60, 50, 50), mainColor = Color.Transparent };
            Elements.Add(lblOptions);


            int ii = 0;
            int gap = 120;
            int leftlabelBorder = width / 6;

            int rightBorder = (int)(width * 5 / 6f);
            int sliderlength = 400;
            int sliderWidth = 50;
            int checkboxlength = 96;

            Label label_OverallVolume = new Label(Manager)
            { fontName = "buttonFont", scale = 1f, text = "All Volume", fontColor = Color.White, rectangle = new Rectangle(leftlabelBorder, 250 + (ii++ * gap), 50, 50), mainColor = Color.Transparent, textAligment = MonogameLibrary.UI.Enums.TextAligment.Left };
            Elements.Add(label_OverallVolume);

            var slider_OverallVolume = new Slider(Manager)
            { rectangle = new Rectangle(rightBorder - sliderlength, label_OverallVolume.rectangle.Y, sliderlength, sliderWidth), indentation = 4, textureName = "textures\\ui\\slider_background", MinValue = 0, MaxValue = 1 };
            slider_OverallVolume.SliderChanged += (newVal) =>
            {
                AppManager.Instance.SettingsManager.SetMainVolume(newVal);
            };
            Elements.Add(slider_OverallVolume);
            slider_OverallVolume.SetValue(AppManager.Instance.SettingsManager.MainVolume);

            Label label_MusicVolume = new Label(Manager)
            { fontName = "buttonFont", scale = 1f, text = "Music Volume", fontColor = Color.White, rectangle = new Rectangle(leftlabelBorder, 250 + (ii++ * gap), 50, 50), mainColor = Color.Transparent, textAligment = MonogameLibrary.UI.Enums.TextAligment.Left };
            Elements.Add(label_MusicVolume);
            
            var slider_MusicVolume = new Slider(Manager)
            { rectangle = new Rectangle(rightBorder - sliderlength, label_MusicVolume.rectangle.Y, sliderlength, sliderWidth), indentation = 4, textureName = "textures\\ui\\slider_background", MinValue = 0, MaxValue = 1 };
            slider_MusicVolume.SliderChanged += (newVal) =>
            {
                AppManager.Instance.SettingsManager.SetMusicVolume(newVal);
            }; 
            Elements.Add(slider_MusicVolume);
            slider_MusicVolume.SetValue(AppManager.Instance.SettingsManager.MusicVolume);


            Label label_EffectsVolume = new Label(Manager)
            { fontName = "buttonFont", scale = 1f, text = "Effects Volume", fontColor = Color.White, rectangle = new Rectangle(leftlabelBorder, 250 + (ii++ * gap), 50, 50), mainColor = Color.Transparent, textAligment = MonogameLibrary.UI.Enums.TextAligment.Left };
            Elements.Add(label_EffectsVolume);

            var slider_EffectsVolume = new Slider(Manager)
            { rectangle = new Rectangle(rightBorder - sliderlength, label_EffectsVolume.rectangle.Y, sliderlength, sliderWidth), indentation = 4, textureName = "textures\\ui\\slider_background", MinValue = 0, MaxValue = 1 };
            slider_EffectsVolume.SliderChanged += (newVal) =>
            {
                AppManager.Instance.SettingsManager.SetSoundEffectsVolume(newVal);
            };
            Elements.Add(slider_EffectsVolume);
            slider_EffectsVolume.SetValue(AppManager.Instance.SettingsManager.SoundEffectsVolume);

            Label lblSwitchMode = new Label(Manager)
            { fontName = "buttonFont", scale = 1f, text = "Left/Right Mode", fontColor = Color.White, rectangle = new Rectangle(leftlabelBorder, 250 + (ii++ * gap), 50, 50), mainColor = Color.Transparent, textAligment = MonogameLibrary.UI.Enums.TextAligment.Left };
            Elements.Add(lblSwitchMode);

            var button_left_right_mode = new CheckBox(Manager) { rectangle = new Rectangle(rightBorder - checkboxlength, lblSwitchMode.rectangle.Y - 12, checkboxlength, checkboxlength) };
            button_left_right_mode.Checked += (newCheckState) => { };
            Elements.Add(button_left_right_mode);


            Label label_IsFullScreen = new Label(Manager)
            { fontName = "buttonFont", scale = 1f, text = "Full Screen", fontColor = Color.White, rectangle = new Rectangle(leftlabelBorder, 250 + (ii++ * gap), 50, 50), mainColor = Color.Transparent, textAligment = MonogameLibrary.UI.Enums.TextAligment.Left };
            Elements.Add(label_IsFullScreen);

            var button_FullScreen = new CheckBox(Manager) { rectangle = new Rectangle(rightBorder - checkboxlength, label_IsFullScreen.rectangle.Y - 12, checkboxlength, checkboxlength) };
            button_FullScreen.Checked += (newCheckState) =>
            {
                AppManager.Instance.SettingsManager.SetIsFullScreen(newCheckState);
            };
            Elements.Add(button_FullScreen);


            Button bTExit = new Button(Manager)
            { fontName = "Font2", text = "<-", rectangle = new Rectangle(width / 30, height / 30, (int)(40 * 2.4), (int)(40 * 2.4)), textureName = "textures\\ui\\textbox_background1-1" };
            Elements.Add(bTExit);
            bTExit.LeftButtonPressed += () =>
            {
                AppManager.Instance.ChangeGameState(GameState.Menu);
                AppManager.Instance.SoundManager.StartSound("reloading", Vector2.Zero, Vector2.Zero);
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
            //slider.rectangle.X = (int)(scaler * slider.rectangle.X);
            //slider.rectangle.Y = (int)(scaler * slider.rectangle.Y);
            ////slider.rectangle.Width = (int)(scaler * slider.rectangle.Width);
            ////slider.rectangle.Height = (int)(scaler * slider.rectangle.Height);
            //if (slider is DrawableTextedUiElement)
            //{
            //    (slider as DrawableTextedUiElement).scale *= scaler;
            //}
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}