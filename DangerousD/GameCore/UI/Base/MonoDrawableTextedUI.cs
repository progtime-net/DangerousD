using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameLibrary.UI.Base
{
    public class MonoDrawableTextedUI : MonoDrawableUI
    {
        protected SpriteFont spriteFont;
        public string text = "";
        public float scale = 0.5f;
        public Color fontColor = Color.Black;
        public TextAligment textAligment = TextAligment.Center;
        public MonoDrawableTextedUI(MonoClassManagerUI MyUIManager = null, int layerIndex = 0) : base(MyUIManager,layerIndex)
        {
        }
        public virtual void LoadTexture(string textureName = "", string font = "")
        {
            base.LoadTexture(textureName);
            if (font != "")
            {
                try
                {
                    spriteFont = MonoClassManagerUI.MainContent.Load<SpriteFont>(font);
                }
                catch
                {
                }
            }

        }
        public virtual void DrawText(SpriteBatch _spriteBatch)
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
                    _spriteBatch.DrawString(spriteFont, text, pos, fontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else if (textAligment == TextAligment.Center)
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)(2 * scale);
                    _spriteBatch.DrawString(spriteFont, text, pos, fontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)(rectangle.Width - measured.X - 2 * scale);
                    _spriteBatch.DrawString(spriteFont, text, pos, fontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
            }
            else
            { 
                var measured = MonoClassManagerUI.MainBaseFont.MeasureString(text) * scale;
                measured.X -= measured.X % 10;
                //measured.Y *= -1;
                if (textAligment == TextAligment.Center)
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)((rectangle.Width - measured.X) / 2);
                    _spriteBatch.DrawString(MonoClassManagerUI.MainBaseFont, text, pos, fontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
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
                    _spriteBatch.DrawString(MonoClassManagerUI.MainBaseFont, text, pos, fontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else
                {
                    Vector2 pos = rectangle.Location.ToVector2();
                    pos.Y += (int)((rectangle.Height - measured.Y) / 2);
                    pos.X += (int)(rectangle.Width - measured.X - 2 * scale);
                    _spriteBatch.DrawString(MonoClassManagerUI.MainBaseFont, text, pos, fontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
            }

        }
    }
}
