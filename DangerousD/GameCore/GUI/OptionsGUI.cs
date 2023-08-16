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
        protected override void CreateUI()
        {
            var slider = new Slider(Manager)
            {
                MinValue = 0,
                MaxValue = 1,
                rectangle = new Rectangle(650, 150, 100, 40)
            };

            var checkBox = new CheckBox(Manager);
            checkBox.rectangle = new Rectangle(690, 400, 40, 40);
            checkBox.Checked += (newCheckState) =>
            {
                SettingsManager sM = new SettingsManager();
            };

            var cB = new CheckBox(Manager);
            cB.rectangle = new Rectangle(690, 275, 40, 40);
            cB.Checked += (newCheckState) =>
            {
                SettingsManager sM = new SettingsManager();
            };

            Label lblOptions = new Label(Manager);
            lblOptions.fontName = "Font2";
            lblOptions.text = "Options";
            lblOptions.rectangle = new Rectangle(300, 20, 210, 50);
            lblOptions.mainColor = Color.Transparent;

            Label lblValue = new Label(Manager);
            lblValue.fontName = "Font2";
            lblValue.text = "Valume";
            lblValue.rectangle = new Rectangle(300, 150, 250, 40);
            lblValue.mainColor = Color.Transparent;

            Label lblIsFullScreen = new Label(Manager);
            lblIsFullScreen.fontName = "Font2";
            lblIsFullScreen.text = "Full Screen";
            lblIsFullScreen.rectangle = new Rectangle(300, 400, 250, 40);
            lblIsFullScreen.mainColor = Color.Transparent;

            Label lblSwitchMode = new Label(Manager);
            lblSwitchMode.fontName = "Font2";
            lblSwitchMode.text = "Left/Right Mode";
            lblSwitchMode.rectangle = new Rectangle(290, 275, 250, 40);
            lblSwitchMode.mainColor = Color.Transparent;

            ButtonText bTExit = new ButtonText(Manager);
            bTExit.fontName = "Font2";
            bTExit.text = "<-";
            bTExit.rectangle = new Rectangle(20 , 15, 20, 10);
            bTExit.fontColor = Color.Black;
        }
    } 
}