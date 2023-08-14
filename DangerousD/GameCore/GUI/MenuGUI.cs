using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Elements;

namespace DangerousD.GameCore.GUI;

internal class MenuGUI : AbstractGui
{
    protected override void CreateUI()
    {
        Elements.Add(new CheckBox(Manager) { rectangle = new Rectangle(10, 10, 50, 50) });
    }
}