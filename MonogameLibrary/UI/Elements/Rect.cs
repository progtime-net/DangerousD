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
    public class Rect : DrawableTextedUiElement, IInteractable
    {
        public delegate void OnButtonPressed();
        public event OnButtonPressed? RightButtonPressed;
        public event OnButtonPressed? LeftButtonPressed;
        protected HoverState hoverState = HoverState.None;

        public Rect(UIManager manager, int layerIndex = 0) : base(manager, layerIndex)
        {
        }
        
        public virtual bool InteractUpdate(MouseState mouseState, MouseState prevmouseState)
        {
            //if (Manager.)
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
            _spriteBatch.Draw(texture, rectangle, Color.White);
            DrawText(_spriteBatch);
        }
    }
}
