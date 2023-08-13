using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogameLibrary.UI.Interfaces
{
    interface IInteractable
    {
        bool InteractUpdate(MouseState mouseState, MouseState prevmouseState);
    }
}
