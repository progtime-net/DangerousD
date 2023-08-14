using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Enums;
using MonogameLibrary.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogameLibrary.UI.Elements
{
    public class CheckBox : MonoDrawableTextedUI, IInteractable
    {
        public CheckBox(MonoClassManagerUI MyUIManager = null, int layerIndex = 0) : base(MyUIManager, layerIndex)
        {
        }
        public delegate void OnCheck(bool checkState);
        public event OnCheck? Checked;
        private bool isChecked;
        HoverState hoverState = HoverState.None;
        public bool GetChecked { get { return isChecked; } }
        public bool InteractUpdate(MouseState mouseState, MouseState prevmouseState)
        {
            if (rectangle.Intersects(new Rectangle(mouseState.Position, Point.Zero)))
            {
                hoverState = HoverState.Hovering;
                if (prevmouseState.LeftButton == ButtonState.Pressed)
                {
                    hoverState = HoverState.Pressing;
                    if (mouseState.LeftButton != prevmouseState.LeftButton)
                    {
                        isChecked = !isChecked;
                        Checked?.Invoke(isChecked);
                        return true;
                    }
                }
            }
            else
            {
                hoverState = HoverState.None;
            }
            return false;
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
            if (isChecked)
            {
                if (hoverState == HoverState.None)
                    _spriteBatch.Draw(texture, rectangle, new Color(124, 255, 0));
                else if (hoverState == HoverState.Hovering)
                    _spriteBatch.Draw(texture, rectangle, new Color(124, 215, 0));
                else
                    _spriteBatch.Draw(texture, rectangle, new Color(124, 175, 0));
            }
            else
            {
                if (hoverState == HoverState.None)
                    _spriteBatch.Draw(texture, rectangle, new Color(255, 20, 0));
                else if (hoverState == HoverState.Hovering)
                    _spriteBatch.Draw(texture, rectangle, new Color(215, 20, 0));
                else
                    _spriteBatch.Draw(texture, rectangle, new Color(175, 20, 0));
            }
            DrawText(_spriteBatch);
        }

    }
}
