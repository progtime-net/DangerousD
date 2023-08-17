using System;
using Microsoft.Xna.Framework;
using MonogameLibrary.UI.Base;
using MonogameLibrary.UI.Elements;
using System.Xml.Linq;
using DangerousD.GameCore.Managers;
using DangerousD.GameCore;
using System.Collections.Generic;

namespace DangerousD.GameCore.GUI
{
    public class HUD : AbstractGui
    {
        Button[] buttons = new Button[2];
        int ammout = 0;
        List<Rect> rects = new List<Rect> { };
        int wigth = AppManager.Instance.inGameResolution.X;
        int height = AppManager.Instance.inGameResolution.Y;
        protected override void CreateUI()
        {
            DrawableUIElement background = new DrawableUIElement(Manager) { rectangle = new Rectangle(0, 0, wigth, height), mainColor = Color.Transparent };
            Elements.Add(background);
            Rect rect = new Rect(Manager) { rectangle = new Rectangle(wigth / 35, height / 35, 120, 70), mainColor = Color.DarkRed };
            Elements.Add(rect);
            Label label = new Label(Manager) { rectangle = new Rectangle(wigth / 34, height / 30, 120, 20), text = "ammout", fontName = "font2", scale = 0.2f, mainColor = Color.Transparent, fontColor = Color.Black };
            Elements.Add(label);
            buttons[0] = new Button(Manager) { rectangle = new Rectangle(300, 300, 50, 50), text = ammout.ToString() };
            Elements.Add(buttons[0]);
            buttons[1] = new Button(Manager) { rectangle = new Rectangle(300, 400, 50, 50) };
            Elements.Add(buttons[1]);

        }
        public override void Update(GameTime gameTime)
        {
            
            rects.Clear();
            for (int i = 0; i < ammout; i++)
            {   ammout += 1;
                ammout %= 8;
                rects.Add(new Rect(Manager) { rectangle = new Rectangle(wigth / 29 + i * 13, height / 17, 5, 20), mainColor = Color.Yellow });
                rects[i].LoadTexture(AppManager.Instance.Content);
            }
            base.Update(gameTime);
        }
    }
}

