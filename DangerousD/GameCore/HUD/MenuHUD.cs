using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore.HUD
{
    class MenuHUD : IHUD
    {
        MonogameLibrary.UI.Base.MonoClassManagerUI managerUI = new MonogameLibrary.UI.Base.MonoClassManagerUI();
        public MenuHUD()
        {
            managerUI.InitManager(""); 
            var lab = new MonogameLibrary.UI.Elements.CheckBox(managerUI) { rectangle = new Microsoft.Xna.Framework.Rectangle(10, 10, 50, 50)};
            lab.LoadTexture();
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            managerUI.Draw(_spriteBatch);
        }

        public void Update()
        {
            managerUI.Update(null);
        }
    }
}
