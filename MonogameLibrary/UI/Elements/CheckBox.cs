using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    public class CheckBox : DrawableTextedUiElement, IInteractable
    {
        public CheckBox(UIManager manager, int layerIndex = 0) : base(manager, layerIndex)
        {
        }
        private Texture2D texture1;
        private Texture2D texture2;
        private Texture2D texture3;
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

        public override void LoadTexture(ContentManager content)
        {
            texture1 = content.Load<Texture2D>("checkboxs_off");
            texture2 = content.Load<Texture2D>("checkboxs_off-on");
            texture3 = content.Load<Texture2D>("checkboxs_on");
            base.LoadTexture(content);
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
            if (isChecked)
            {
                if (hoverState == HoverState.None)
                    _spriteBatch.Draw(texture3, rectangle, Color.White);
                else if (hoverState == HoverState.Hovering)
                    _spriteBatch.Draw(texture3, rectangle, Color.White);
                else
                    _spriteBatch.Draw(texture2, rectangle, Color.White );
            }
            else
            {
                if (hoverState == HoverState.None)
                    _spriteBatch.Draw(texture1, rectangle, Color.White);
                else if (hoverState == HoverState.Hovering)
                    _spriteBatch.Draw(texture2, rectangle, Color.White);
                else
                    _spriteBatch.Draw(texture2, rectangle, Color.White);
            }
            DrawText(_spriteBatch);
        }

    }
}
