using DangerousD.GameCore.GameObjects.LivingEntities;
using DangerousD.GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DangerousD.GameCore.GameObjects.Entities.Items
{
    internal class ScoreText : Entity
    { 
        SpriteFont spriteFont;
        string textStr;
        Vector2  strSize;
        public ScoreText(Vector2 position, string text) : base(position)
        {
            Width = 32;
            Height = 32;
            textStr = text;

            GraphicsComponent.StartAnimation("score100");
            //GraphicsComponent.actionOfAnimationEnd += (a) => { AppManager.Instance.GameManager.Remove(this); };


            spriteFont = AppManager.Instance.Content.Load<SpriteFont>("PixelFont");
            strSize = spriteFont.MeasureString(textStr);
        }
        public override void Initialize()
        {
        }
        float dPosLeft = 70;
        float dPosLeftMax = 70; 
        public override void Update(GameTime gameTime)
        {
            float dy = (float)(gameTime.ElapsedGameTime.TotalSeconds * dPosLeft * (dPosLeft / dPosLeftMax) * 0.98f);
            _pos.Y -= dy;
            dPosLeft -= dy;
            if (gameTime.ElapsedGameTime.TotalSeconds * dPosLeft * 0.95f < 0.1) { AppManager.Instance.GameManager.Remove(this); }
            scoreSizeScaler = 2.5f  * (dPosLeft / dPosLeftMax);
            base.Update(gameTime); 
        }

        protected override GraphicsComponent GraphicsComponent { get; } = new GraphicsComponent(new List<string>() { "score100", "score600" }, "score100");

        float scoreSizeScaler = 2.5f;
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, textStr,
                _pos*GraphicsComponent.scaling - GraphicsComponent.CameraPosition.ToVector2() * GraphicsComponent.scaling
                - scoreSizeScaler*strSize / 2, new Color(1.1f, 1.1f,0.1f), 0, Vector2.Zero,
                scoreSizeScaler, SpriteEffects.None, 0);
            //base.Draw(spriteBatch);
        }
    }
}
