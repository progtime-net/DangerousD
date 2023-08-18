using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameLibrary.UI.Elements;
using static System.String;

namespace DangerousD.GameCore.GUI
{
    public class DebugHUD : IDrawableObject
    {
        private SpriteFont _spriteFont;
        private Dictionary<string, string> _text = new();
        private List<string> _log = new();

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            _spriteFont = AppManager.Instance.Content.Load<SpriteFont>("Font_12");
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /*var keysString = Join("\n", _text.Select(el => el.Key + ": " + el.Value).ToList());
            spriteBatch.Begin();
            spriteBatch.DrawString(
                _spriteFont,
                keysString,
                new Vector2(10, 10),
                Color.Cyan,
                0,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0
            );
            spriteBatch.DrawString(
                _spriteFont,
                Join("\n", _log),
                new Vector2(10, 10 + _spriteFont.MeasureString(keysString).Y),
                Color.Green,
                0,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                0
            );
            spriteBatch.End();*/
        }

        public void Set(string key, string value)
        {
            _text[key] = value;
        }

        public void Log(string value)
        {
            _log.Add(value);
            if (_log.Count > 30)
            {
                _log.RemoveAt(0);
            }
        }
    }
}