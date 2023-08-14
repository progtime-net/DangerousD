using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameLibrary.UI.Elements
{
    public class Label : DrawableTextedUiElement
    {

        public Label(UIManager manager, int layerIndex = 0) : base(manager, layerIndex)
        {
        }
        protected HoverState hoverState = HoverState.None;

        public virtual bool InteractUpdate(MouseState mouseState, MouseState prevmouseState)
        {
            if (rectangle.Intersects(new Rectangle(mouseState.Position, Point.Zero)))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    hoverState = HoverState.Pressing;
                }
                else
                {
                    hoverState = HoverState.Hovering;
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
            {
                _spriteBatch.Draw(texture, rectangle, new Color(235, 235, 235));
            }
            else if (hoverState == HoverState.Hovering)
                _spriteBatch.Draw(texture, rectangle, new Color(211, 211, 211));
            else
                _spriteBatch.Draw(texture, rectangle, new Color(112, 128, 144));
            DrawText(_spriteBatch);
        }
    }
}
