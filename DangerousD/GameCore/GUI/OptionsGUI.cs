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
                rectangle = new Rectangle(650, 150, 100 ,40)
            };

            Label lblOptions = new Label(Manager, 0);
            lblOptions.fontName = "font2";
            lblOptions.text = "Options";
            lblOptions.rectangle = new Rectangle(300, 20, 210, 50);
            lblOptions.mainColor = Color.Transparent;
        }
    } 
}