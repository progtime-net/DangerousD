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
            var slider = new Slider(Manager) { };

        }
    } 
}