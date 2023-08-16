using System;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System.Xml.Linq;

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
        }
    } 
}