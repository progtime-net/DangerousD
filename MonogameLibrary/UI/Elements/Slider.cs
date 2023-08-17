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
    public class Slider : DrawableTextedUiElement, IInteractable
    {
        public Slider(UIManager manager, int layerIndex = 0) : base(manager, layerIndex)
        {
        }
        public delegate void OnSliderChanges(float value);
        public event OnSliderChanges? SliderChanged;
        public int indentation = 5;

        Texture2D texture2; 
        public Rectangle sliderRect = new Rectangle(0, 0, 30, 30);
        private float sliderValue = 0;
        private float minValue = 0, maxValue = 1;
        SliderState sliderState = SliderState.None;

        public float GetValue { get { return minValue + sliderValue * (maxValue - minValue); } }
        public float GetSliderValue { get { return sliderValue; } }
        public float MinValue { get { return minValue; } set { minValue = value; } }
        public float MaxValue { get { return maxValue; } set { maxValue = value; } }

        public bool InteractUpdate(MouseState mouseState, MouseState prevmouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (sliderRect.Intersects(new Rectangle(mouseState.Position, Point.Zero)) ||
                    rectangle.Intersects(new Rectangle(mouseState.Position, Point.Zero)) ||
                    sliderState == SliderState.Moving)
                {
                    sliderValue = Math.Clamp((mouseState.Position.X - rectangle.X - sliderRect.Width / 2f) / (rectangle.Width - sliderRect.Width), 0, 1);
                    sliderState = SliderState.Moving;
                    SliderChanged?.Invoke(GetValue);
                    return true;
                }
            }
            else if (sliderRect.Intersects(new Rectangle(mouseState.Position, Point.Zero)))
            {
                sliderState = SliderState.HoveringOverSliderButton;
            }
            else
                sliderState = SliderState.None;
            return false;
        }

        public override void LoadTexture(ContentManager content)
        {
            texture2 = content.Load<Texture2D>("slider");
            base.LoadTexture(content);
        }

        public void SetValue(float setvalue)
        {
            sliderValue = setvalue;
            SliderChanged?.Invoke(GetValue);
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            sliderRect.Location = rectangle.Location;
            sliderRect.X += (int)(sliderValue * (rectangle.Width - sliderRect.Width - indentation * 2) + indentation);
            sliderRect.Y -= sliderRect.Height / 2 - rectangle.Height / 2;
            if (sliderState == SliderState.Moving)
                _spriteBatch.Draw(texture2, sliderRect, Color.DarkRed);
            else if(sliderState == SliderState.HoveringOverSliderButton)
                _spriteBatch.Draw(texture2, sliderRect, new Color(200,0 ,0));
            else
                _spriteBatch.Draw(texture2, sliderRect, Color.Red);
            DrawText(_spriteBatch);
        }
    }
}
