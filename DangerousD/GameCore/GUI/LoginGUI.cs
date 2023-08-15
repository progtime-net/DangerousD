using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using MonogameLibrary.UI.Elements;

namespace DangerousD.GameCore.GUI
{
    class LoginGUI : AbstractGui
    {
        protected override void CreateUI()
        {
            Elements.Add(new Label(Manager) { rectangle = new Rectangle(AppManager.Instance.resolution.X / 2, AppManager.Instance.resolution.Y / 2, 100, 50) });
        }
    }
}
