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
    public class Button : MonoDrawableTextedUI, IInteractable
    {
        public delegate void OnButtonPressed();
        public event OnButtonPressed? RightButtonPressed;
        public event OnButtonPressed? LeftButtonPressed;
        protected HoverState hoverState = HoverState.None;
        public bool InteractUpdate(MouseState mouseState, MouseState prevmouseState)
        {
            if (rectangle.Intersects(new Rectangle(mouseState.Position, Point.Zero)))
            {
                if (mouseState.LeftButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed)
                {
                    hoverState = HoverState.Pressing;
                }
                else
                {
                    hoverState = HoverState.Hovering;
                }
                if (prevmouseState.LeftButton == ButtonState.Pressed)
                {
                    if (mouseState.LeftButton != prevmouseState.LeftButton)
                    {
                        hoverState = HoverState.Pressing;
                        LeftButtonPressed?.Invoke();
                        return true;
                    }
                }
                else if(prevmouseState.RightButton == ButtonState.Pressed)
                {
                    if (mouseState.RightButton != prevmouseState.RightButton)
                    {
                        RightButtonPressed?.Invoke();
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
            if (hoverState == HoverState.None)
                _spriteBatch.Draw(texture, rectangle, Color.White);
            else if (hoverState == HoverState.Hovering)
                _spriteBatch.Draw(texture, rectangle, new Color(211,211,211));
            else
                _spriteBatch.Draw(texture, rectangle, new Color(112, 128, 144));
            DrawText(_spriteBatch);
        }
    }
}
