using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Enums;
using MonogameLibrary.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static MonogameLibrary.UI.Elements.Button;

namespace MonogameLibrary.UI.Elements
{
    public class ButtonText : Button
    {
        public ButtonText(UIManager manager, int layerIndex = 0) : base(manager, layerIndex)
        {
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
            if (hoverState == HoverState.None)
            {
                fontColor = Color.White;
            }
            else if (hoverState == HoverState.Hovering)
            {
                fontColor = new Color(211, 211, 211);
            }
            else
            {
                fontColor = new Color(112, 128, 144);
            }

            DrawText(_spriteBatch);
        }
    }
}
