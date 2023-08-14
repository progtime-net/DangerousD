using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using MonogameLibrary.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;

namespace MonogameLibrary.UI.Compounds
{
    public enum BasicDrawableCompound_Type { Vertical, Horizontal };
    public class BasicDrawableCompound : DrawableTextedUiElement
    {
        public BasicDrawableCompound(UIManager manager, int layerIndex = 0) : base(manager, layerIndex)
        {
            rectangle = new Rectangle(0, 0, 20, 20);
            lastPos = new Vector2(0, offset.Y);
        }
        Dictionary<string, DrawableTextedUiElement> drawables = new Dictionary<string, DrawableTextedUiElement>();
        public Vector2 lastPos;
        Vector2 offset = new Vector2(10, 10);
        int mainWidth = 40;
        public void Add(string name, DrawableTextedUiElement element)
        {
            //var a = Assembly.GetExecutingAssembly().GetTypes();
            //var b = a.Where(t => t.Get().Contains(type));
            //var element = ((MonoDrawableTextedUI)Assembly.GetExecutingAssembly().GetTypes()
            //    .Where(t => t.GetInterfaces().Contains(type))
            //    .Select(x => Activator.CreateInstance(x) as ICommand).ToArray()[0]);
            if (element is Slider)
            {
                element.rectangle = new Rectangle(0, 0, mainWidth * 3, 30);
            }
            if (element is Label)
            {
                element.rectangle = new Rectangle(0, 0, mainWidth * 2, 30);
            }
            if (element is TextBox)
            {
                element.rectangle = new Rectangle(0, 0, mainWidth * 2, 30);
            }
            if (element is Button)
            {
                element.rectangle = new Rectangle(0, 0, mainWidth, 30);
            }
            element.rectangle.Location = GetPositionOfElement(element).ToPoint();
            if (drawables.ContainsKey(name))
            {
                int i = 1;
                while (drawables.ContainsKey(name + $"({i})"))
                {
                    i++;
                }
                name += $"({i})";
            }
            drawables.Add(name, element);
        }
        public Vector2 GetPositionOfElement(DrawableTextedUiElement element)
        {
            Vector2 pos = lastPos + new Vector2(offset.X, 0);
            lastPos = pos + new Vector2(element.rectangle.Width, 0);
            if (lastPos.X>= rectangle.Width)
            {
                pos.X = offset.X;
                pos.Y += offset.Y + element.rectangle.Height;
                lastPos = pos + new Vector2(element.rectangle.Width, 0);
            }
            return rectangle.Location.ToVector2() + pos;
        }

        public override void LoadTexture(ContentManager content)
        {
            base.LoadTexture(content);
            if (fontName != "")
            {
                try
                {
                    spriteFont = content.Load<SpriteFont>(fontName);
                }
                catch
                {
                }
            }
            foreach (var d in drawables)
            {
                d.Value.LoadTexture(content);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, mainColor);
            foreach (var d in drawables)
            {
                d.Value.Draw(spriteBatch);
            } 
        }
    }
}
