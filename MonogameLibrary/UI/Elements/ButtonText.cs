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
        float gameTime = 0; 
        public override bool InteractUpdate(MouseState mouseState, MouseState prevmouseState)
        {
            gameTime += (float)Manager.gameTime.ElapsedGameTime.TotalSeconds;
            return base.InteractUpdate(mouseState, prevmouseState);
        }
        
        public override void Draw(SpriteBatch _spriteBatch)
        {
            if (hoverState == HoverState.None)
            {
                var d = (float)(((Math.Sin(gameTime * 2 - Math.PI) + 1) / 2f) * 0.1 + 0.7f);
                fontColor = Color.FromNonPremultiplied(new Vector4(0.8f,0.15f, 0.15f, 1) * d 
                );
            }
            else if (hoverState == HoverState.Hovering)
            {  
                var d2 = (float)(((Math.Sin(gameTime * 2 - Math.PI) + 1) / 2f) * 0.1 + 0.7f);
                fontColor = Color.FromNonPremultiplied(new Vector4(0.8f, 0.15f, 0.15f, 1) * d2
                );
            }
            else
            {
                //fontColor = new Color(112, 128, 144);
                fontColor = new Color(212, 228, 244);
            }

            if (hoverState == HoverState.Hovering)
            { 
                int kk = 50;
                scale += 0.005f;
                var d = (float)(((Math.Sin(gameTime * 1 - Math.PI) + 1) / 2f) * 0.1 + 1f);
                Color oldColor = fontColor;
                fontColor = Color.FromNonPremultiplied(new Vector4(252 / 255f, 231 / 255f, 124 / 255f, 1) * d
                );
                DrawText(_spriteBatch);
                fontColor = oldColor;
                fontColor.A = 255;
                scale -= 0.005f;
                scale -= 0.0002f;
            }
            if (hoverState == HoverState.Pressing)
            {
                scale -= 0.025f;
                DrawText(_spriteBatch);
                scale += 0.025f;
            }
            else
            {
                DrawText(_spriteBatch);
            }
        }
    }
}
