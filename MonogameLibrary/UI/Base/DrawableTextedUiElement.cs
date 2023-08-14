using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MonogameLibrary.UI.Base
{
    public class DrawableTextedUiElement : DrawableUIElement
    {
        protected SpriteFont spriteFont;
        protected string fontName;
        public string text = "";
        public float scale = 0.5f;
        public Color fontColor = Color.Black;
        public TextAligment textAligment = TextAligment.Center;

        public DrawableTextedUiElement(UIManager manager, int layerIndex = 0, string textureName = "", string fontName = "")
            : base(manager, layerIndex, textureName)
        {
            this.fontName = fontName;
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
        }

        public virtual void DrawText(SpriteBatch spriteBatch)
        {
            if (text == "")
                return;

            if (spriteFont != null)
            {
                var measured = spriteFont.MeasureString(text) * scale;

                if (textAligment == TextAligment.Center)
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)((rectangle.Width - measured.X) / 2);
                    spriteBatch.DrawString(spriteFont, text, pos, fontColor, 0, Vector2.Zero, scale,
                        SpriteEffects.None, 0);
                }
                else if (textAligment == TextAligment.Center)
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)(2 * scale);
                    spriteBatch.DrawString(spriteFont, text, pos, fontColor, 0, Vector2.Zero, scale,
                        SpriteEffects.None, 0);
                }
                else
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)(rectangle.Width - measured.X - 2 * scale);
                    spriteBatch.DrawString(spriteFont, text, pos, fontColor, 0, Vector2.Zero, scale,
                        SpriteEffects.None, 0);
                }
            }
            else
            {
                var measured = Manager.BaseFont.MeasureString(text) * scale;
                measured.X -= measured.X % 10;
                //measured.Y *= -1;
                if (textAligment == TextAligment.Center)
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)((rectangle.Width - measured.X) / 2);
                    spriteBatch.DrawString(Manager.BaseFont, text, pos, fontColor, 0, Vector2.Zero, scale,
                        SpriteEffects.None, 0);
                }
                else if (textAligment == TextAligment.Left)
                {
                    //var rct = new Rectangle(rectangle.Location, rectangle.Size);
                    //rct.Width = (int)measured.X;
                    //rct.Height = (int)measured.Y;
                    //rct.Y += (int)((rectangle.Height - measured.Y) / 2);
                    //rct.X += (int)((rectangle.Width - measured.X) / 2);

                    //_spriteBatch.Draw(texture, rct, new Color(255, 0, 0, 125));
                    //_spriteBatch.DrawString(MonoClassManagerUI.MainBaseFont, text, rct.Location.ToVector2(), fontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);


                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)(2 * scale);
                    spriteBatch.DrawString(Manager.BaseFont, text, pos, fontColor, 0, Vector2.Zero, scale,
                        SpriteEffects.None, 0);
                }
                else
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)(rectangle.Width - measured.X - 2 * scale);
                    spriteBatch.DrawString(Manager.BaseFont, text, pos, fontColor, 0, Vector2.Zero, scale,
                        SpriteEffects.None, 0);
                }
            }
        }
    }
}