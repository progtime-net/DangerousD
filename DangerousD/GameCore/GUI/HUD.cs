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

namespace DangerousD.GameCore.GUI
{
    public class HUD : IDrawableObject
    {  
        int wigth = AppManager.Instance.inGameResolution.X;
        int height = AppManager.Instance.inGameResolution.Y;
        float scaler = AppManager.Instance.resolution.Y / (float)AppManager.Instance.inGameHUDHelperResolution.Y;
        Texture2D texture;
        SpriteFont spriteFont;

        public void Draw(SpriteBatch spriteBatch)
        { 
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(wigth / 35 - 2, height / 35 - 2, 120 + 2, 70 + 2), Color.DarkRed);
            spriteBatch.DrawString(spriteFont, "AMMO", new Vector2(wigth / 34 + 4, height / 30 - 6), Color.Gray, 0, Vector2.Zero, 1.8f, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, "AMMO", new Vector2(wigth / 34 + 1, height / 30 - 6), Color.White, 0, Vector2.Zero, 1.8f, SpriteEffects.None, 0);
            for (int i = 0; i < 5; i++)
            {
                if (i <= AppManager.Instance.GameManager.players[0].Bullets)
                {
                    spriteBatch.Draw(texture, new Rectangle(wigth / 30 + i * 13 + 2, height / 17 + 4, 5, 20), new Color(0.8f, 0.8f, 0, 1f));
                    spriteBatch.Draw(texture, new Rectangle(wigth / 30 + i * 13, height / 17 + 4, 5, 20), Color.Yellow);
                }
                else
                { 
                    spriteBatch.Draw(texture, new Rectangle(wigth / 30 + i * 13, height / 17 + 4, 7, 20), new Color(0.2f, 0.2f, 0, 1f));
                }
            }
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

