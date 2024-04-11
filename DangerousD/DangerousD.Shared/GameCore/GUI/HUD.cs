using System;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System.Xml.Linq;
using DangerousD.GameCore.Managers;
using DangerousD.GameCore;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Serialization;

namespace DangerousD.GameCore.GUI
{
    public class HUD : IDrawableObject
    {  
        int wigth = AppManager.Instance.inGameResolution.X;
        int height = AppManager.Instance.inGameResolution.Y;
        float scaler = AppManager.Instance.inGameResolution.Y / (float)AppManager.Instance.inGameHUDHelperResolution.Y;
        Texture2D texture;
        SpriteFont spriteFont;

        public void Draw(SpriteBatch spriteBatch)
        { 
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(
                (int)((wigth / 35 - 2) * scaler), 
                (int)((height / 35 - 2) * scaler), 
                (int)((120 + 2) * scaler), 
                (int)((70 + 2) * scaler)), Color.DarkRed);
            spriteBatch.DrawString(spriteFont, "AMMO", new Vector2((wigth / 34 + 4)* scaler, height / 30 - 6), Color.Gray, 0, Vector2.Zero, 1.8f, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, "AMMO", new Vector2((wigth / 34 + 1 )* scaler, height / 30 - 6), Color.White, 0, Vector2.Zero, 1.8f, SpriteEffects.None, 0);
            for (int i = 1; i < 6; i++)
            {
                if (i <= AppManager.Instance.GameManager.players[0].Bullets)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)((wigth / 30 + i * 13 + 2) * scaler), (int)((height / 17 + 4) * scaler), (int)((5) * scaler), (int)((20) * scaler)), new Color(0.8f, 0.8f, 0, 1f));
                    spriteBatch.Draw(texture, new Rectangle((int)((wigth / 30 + i * 13) * scaler), (int)((height / 17 + 4) * scaler), (int)((5) * scaler), (int)((20) * scaler)), Color.Yellow);
                }
                else
                { 
                    spriteBatch.Draw(texture, new Rectangle(wigth / 30 + i * 13, height / 17 + 4, 7, 20), new Color(0.2f, 0.2f, 0, 1f));
                }
            }
            spriteBatch.End();

            spriteBatch.Begin();

            #region Timer
            float gameTimeSeconds = (float)AppManager.Instance.gameTime.TotalGameTime.TotalSeconds;
            TimeSpan ts = TimeSpan.FromSeconds(AppManager.Instance.GameManager.GetTimeOfPlaythrough); ;
            string timestr = $"{ts.ToString("hh\\:mm\\:ss\\.fff")}";

            float timerSizeScaler = 2f;
            float timerTimeScaler = 1.5f;
            float timerAmplitudeScaler = 1.5f;
            Vector2 StringStandartSize = spriteFont.MeasureString(timestr);
            float temp_scaler =  timerAmplitudeScaler * 0.05f * (float)Math.Sin(timerTimeScaler * gameTimeSeconds * 3);
            Vector2 timerPosition = new Vector2((wigth / 2) * scaler, height / 30 + 6);


            spriteBatch.DrawString(spriteFont, timestr,
                timerPosition - timerSizeScaler * (timerSizeScaler * 1.46f + temp_scaler) * StringStandartSize / 4, new Color(0
                , 0f,0), 0, Vector2.Zero,
                timerSizeScaler * 1.46f + temp_scaler, SpriteEffects.None, 0);

            spriteBatch.DrawString(spriteFont, timestr,
                timerPosition - timerSizeScaler * (timerSizeScaler * 1.44f + temp_scaler) * StringStandartSize / 4, new Color(-0.1f
                + 0.3f * (float)Math.Sin(timerTimeScaler * gameTimeSeconds), 0f
                + 0.3f * (float)Math.Sin(timerTimeScaler * gameTimeSeconds),
                0.3f * (float)Math.Sin(timerTimeScaler * gameTimeSeconds)), 0, Vector2.Zero,
                timerSizeScaler * 1.44f + temp_scaler, SpriteEffects.None, 0);

            spriteBatch.DrawString(spriteFont, timestr,
                timerPosition - timerSizeScaler * (timerSizeScaler * 1.4f + temp_scaler) * StringStandartSize / 4, new Color(1.1f
                + 0.3f * (float)Math.Sin(timerTimeScaler * gameTimeSeconds), 1.1f
                + 0.3f * (float)Math.Sin(timerTimeScaler * gameTimeSeconds),
                0.1f+0.3f * (float)Math.Sin(timerTimeScaler * gameTimeSeconds)), 0, Vector2.Zero,
                timerSizeScaler * 1.4f + temp_scaler, SpriteEffects.None, 0);
            #endregion

            #region Score

            string scoreStr = AppManager.Instance.GameManager.GetPlayer1.score.ToString();
            StringStandartSize = spriteFont.MeasureString(scoreStr);

            Vector2 scorePosition = new Vector2((0.95f*wigth) * scaler, height / 30 + 6);

            float scoreSizeScaler = 2.5f;
            float scoreTimeScaler = 0.3f;


            spriteBatch.DrawString(spriteFont, scoreStr,
                scorePosition - scoreSizeScaler * new Vector2(StringStandartSize.X, StringStandartSize.Y / 2) + new Vector2(10, 0), new Color(0.3f
                + 0.3f * (float)Math.Sin(scoreTimeScaler * gameTimeSeconds), 0.3f
                + 0.3f * (float)Math.Sin(scoreTimeScaler * gameTimeSeconds),
                0.1f + 0.3f * (float)Math.Sin(scoreTimeScaler * gameTimeSeconds)), 0, Vector2.Zero,
                scoreSizeScaler, SpriteEffects.None, 0);

            spriteBatch.DrawString(spriteFont, scoreStr,
                scorePosition - scoreSizeScaler * new Vector2(StringStandartSize.X, StringStandartSize.Y / 2), new Color(1.1f
                + 0.3f * (float)Math.Sin(scoreTimeScaler * gameTimeSeconds), 1.1f
                + 0.3f * (float)Math.Sin(scoreTimeScaler * gameTimeSeconds),
                0.1f + 0.3f * (float)Math.Sin(scoreTimeScaler * gameTimeSeconds)), 0, Vector2.Zero,
                scoreSizeScaler, SpriteEffects.None, 0);

            #endregion
            spriteBatch.End();
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            texture  = new Texture2D(AppManager.Instance.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            spriteFont = AppManager.Instance.Content.Load<SpriteFont>("PixelFont");
        }

        public void Update(GameTime gameTime)
        {
        }
    }
    //public class HUD1 : AbstractGui
    //{
    //    
    //    protected override void CreateUI()
    //    {
    //        DrawableUIElement background = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, wigth, height), mainColor = Color.Transparent };
    //        Elements.Add(background);
    //        Rect rect = new Rect(Manager) { rectangle = new Rectangle(wigth / 35, height / 35, 120, 70), mainColor = Color.DarkRed };
    //        Elements.Add(rect);
    //        Label label = new Label(Manager) { rectangle = new Rectangle(wigth / 34, height / 30, 120, 20), text = "ammout", fontName = "font2", scale = 0.2f, mainColor = Color.Transparent, fontColor = Color.Black };
    //        Elements.Add(label);

    //    }
    //    public override void Update(GameTime gameTime)
    //    {
    //        
    //        rects.Clear();
    //        for (int i = 0; i < ammout; i++)
    //        {
    //           rects.Add(new Rect(Manager) { rectangle = new Rectangle(wigth / 29 + i * 13, height / 17, 5, 20), mainColor = Color.Yellow });
    //           rects[i].LoadTexture(AppManager.Instance.Content);
    //        }
    //        base.Update(gameTime);
    //    }
    //}
}

