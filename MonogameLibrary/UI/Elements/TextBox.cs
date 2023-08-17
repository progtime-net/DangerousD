using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Enums;
using MonogameLibrary.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MonogameLibrary.UI.Elements
{
    public class TextBox : DrawableTextedUiElement, IInteractable
    {
        public TextBox(UIManager manager, int layerIndex = 0) : base(manager, layerIndex)
        {
            OnEnter += (txt) => {
                isSelected = IsSelected.NotSelected;
                StopChanging?.Invoke(text);
            };
        }
        public delegate void OnTextChange(string text);
        public event OnTextChange? TextChanged;
        public event OnTextChange? StopChanging;
        public event OnTextChange? OnEnter;

        protected HoverState hoverState = HoverState.None;
        protected IsSelected isSelected = IsSelected.NotSelected;
        public bool shouldEndOnEnter;

        public virtual bool InteractUpdate(MouseState mouseState, MouseState prevmouseState)
        {
            if (isSelected == IsSelected.Selected)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    OnEnter?.Invoke(text);
                    if (shouldEndOnEnter)
                    {
                        return false;
                    }
                }

                InputManager.GetInput(ref text);
            }
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
                if (prevmouseState.LeftButton == ButtonState.Pressed)
                {
                    if (mouseState.LeftButton != prevmouseState.LeftButton)
                    {
                        hoverState = HoverState.Pressing;
                        isSelected = IsSelected.Selected;
                        TextChanged?.Invoke(text);
                        return true;
                    }
                }
            }
            else
            {


                if (mouseState.LeftButton == ButtonState.Pressed)
                { 
                    isSelected = IsSelected.NotSelected;
                    StopChanging?.Invoke(text); 
                } 

                hoverState = HoverState.None;
            }
            return false; 
        }
        public override void Draw(SpriteBatch _spriteBatch)
        {
            if (hoverState == HoverState.None)
            {
                if (isSelected == IsSelected.Selected)
                    _spriteBatch.Draw(texture, rectangle, new Color(220, 220, 220));
                else
                    _spriteBatch.Draw(texture, rectangle, new Color(245, 245, 245));
            }
            else if (hoverState == HoverState.Hovering)
                _spriteBatch.Draw(texture, rectangle, new Color(211, 211, 211));
            else
                _spriteBatch.Draw(texture, rectangle, new Color(112, 128, 144));
            DrawText(_spriteBatch); 
        }
    }


    //TODO: add translation
    public static class InputManager
    {
        static List<Keys> pressed = new List<Keys>();
        static float del = 0;
        static bool isShiftPressed = false;
        static bool isCTRLPressed = false;
        static bool isALTPressed = false;
        static bool isCAPSLOCKOn = false;

        public static void GetInput(ref string text)
        {
            var state = Keyboard.GetState();
            var keys = state.GetPressedKeys();
            isShiftPressed = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
            isCTRLPressed = state.IsKeyDown(Keys.LeftControl) || state.IsKeyDown(Keys.RightControl);
            isALTPressed = state.IsKeyDown(Keys.LeftAlt) || state.IsKeyDown(Keys.RightAlt);
            for (int i = 0; i < pressed.Count; i++)
            {
                if (!keys.Contains(pressed[i]))
                {
                    pressed.RemoveAt(i);
                    i--;
                }
            }
            foreach (var key in keys)
            {
                //System.Diagnostics.Debug.WriteLine(key.ToString());
                if (!pressed.Contains(key))
                {
                    pressed.Add(key);



                    string getPressed = KeyDecode(key);
                    if (getPressed != null)
                    {
                        text += getPressed;
                    }
                    else
                    {
                        if (key == Keys.Back)
                        {
                            if (text.Length > 0)
                            {
                                text = text.Remove(text.Length - 1);
                                del = 0;
                            }
                        }
                        if (key == Keys.CapsLock)
                        {
                            isCAPSLOCKOn = !isCAPSLOCKOn;
                        }
                    }


                }
                if (IsKeySpecial(key))
                {
                    del++;
                    if (key == Keys.Back)
                    {
                        if (del>15)
                        {
                            if (text.Length > 0)
                                text = text.Remove(text.Length - 1);
                            del = 13;
                        }
                    } 
                }
            }
        }
        public static string KeyDecode(Keys key)
        {
            string str = null;
            if ((int)key >= 65 && (int)key <= 90)
            {
                str = key.ToString();
            }
            else if ((int)key >= 48 && (int)key <= 57)
            {
                if (!isShiftPressed)
                {
                    str = key.ToString().Trim('D');
                }
                else
                {
                    switch (key)
                    {
                        case Keys.D1:
                            return str = "!";
                        case Keys.D2:
                            return str = "@";
                        case Keys.D3:
                            return str = "#";
                        case Keys.D4:
                            return str = "$";
                        case Keys.D5:
                            return str = "%";
                        case Keys.D6:
                            return str = "^";
                        case Keys.D7:
                            return str = "&";
                        case Keys.D8:
                            return str = "*";
                        case Keys.D9:
                            return str = "(";
                        case Keys.D0:
                            return str = ")";
                    }
                }
            }
            else if ((int)key >= 96  && (int)key <= 105)
            {
                str = key.ToString().Remove(0, 6);
            }
            if (str != null)
            {
                if ((!isShiftPressed && !isCAPSLOCKOn) || (isShiftPressed && isCAPSLOCKOn)) 
                    return str.ToLower(); 
                else
                    return str.ToUpper();
            }


            if (!isShiftPressed)
            {
                switch (key)
                {
                    case Keys.Space:
                        return " ";
                    case Keys.OemTilde:
                        return "`";
                    case Keys.Enter:
                        return "\n";
                    case Keys.OemPipe:
                        return "\\";
                    case Keys.OemPlus:
                        return "=";
                    case Keys.OemMinus:
                        return "-";


                    case Keys.OemComma:
                        return ",";
                    case Keys.OemQuestion:
                        return ".";
                    case Keys.OemPeriod:
                        return "/";
                    case Keys.OemSemicolon:
                        return ";";
                    case Keys.OemQuotes:
                        return "'";
                    case Keys.OemOpenBrackets:
                        return "[";
                    case Keys.OemCloseBrackets:
                        return "]";
                }
            }
            else
            {
                switch (key)
                {
                    case Keys.Space:
                        return " ";
                    case Keys.OemTilde:
                        return "~";
                    case Keys.Enter:
                        return "\n";
                    case Keys.OemPipe:
                        return "|";
                    case Keys.OemPlus:
                        return "+";
                    case Keys.OemMinus:
                        return "_";


                    case Keys.OemComma:
                        return "<";
                    case Keys.OemQuestion:
                        return ">";
                    case Keys.OemPeriod:
                        return "?";
                    case Keys.OemSemicolon:
                        return ":";
                    case Keys.OemQuotes:
                        return "\"";
                    case Keys.OemOpenBrackets:
                        return "{";
                    case Keys.OemCloseBrackets:
                        return "}";
                }
            }

            //else if ((int)key >= 112 && (int)key <= 125)
            //{
            //    return key.ToString().Trim('F');
            //}
            return null;
        }
        public static bool IsKeySpecial(Keys key)
        {
            return (key == Keys.Back) || (key == Keys.Space);
        }
    }
}
