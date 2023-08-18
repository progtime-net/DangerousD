using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameLibrary.UI.Elements;
using MonogameLibrary.UI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames; 

namespace MonogameLibrary.UI.Base
{
    public enum InputState { GamePad, Keyboard, Mouse }
    public class UIManager
    {
        Dictionary<int, List<DrawableUIElement>> layerCollection = new();
        public GraphicsDevice GraphicsDevice { get; private set; }
        public SpriteFont BaseFont { get; private set; }
        public InputState inputState = InputState.Mouse;
        public void Initialize(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            
            for (int i = -10; i < 11; i++)
            {
                layerCollection.Add(i, new List<DrawableUIElement>());
            }
        }
        public KeyboardState GetKeyboardState { get { return keyboardState; } }
        static MouseState mouseState, prevmouseState;
        static KeyboardState keyboardState;
        public static Point resolutionInGame, resolution;
        

        public void LoadContent(ContentManager content, string font)
        {
            
            try
            {
                BaseFont = content.Load<SpriteFont>(font);
            }
            catch
            {
            }
            foreach (var collection in layerCollection)
            {
                foreach (var item in collection.Value)
                {
                    item.LoadTexture(content);
                }
            }
        }
        public GameTime gameTime;
        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            try
            {
                keyboardState = Keyboard.GetState();
                mouseState = Mouse.GetState();
                mouseState = new MouseState((int)(mouseState.X*(float)resolutionInGame.X/resolution.X),
                    (int)(mouseState.Y * (float)resolutionInGame.Y / resolution.Y), mouseState.ScrollWheelValue, mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton, mouseState.XButton1, mouseState.XButton2);
            }
            catch
            {

            }
            bool hasInteracted = false;
            foreach (var collection in layerCollection)
            {
                foreach (var item in collection.Value)
                {
                    if (item is IInteractable)
                    {
                        if (!hasInteracted)
                        {
                            hasInteracted = (item as IInteractable).InteractUpdate(mouseState, prevmouseState);
                        }
                    }
                }
            }
            prevmouseState = mouseState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var collection in layerCollection)
            {
                foreach (var item in collection.Value)
                {
                    item.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }
        public void Register(DrawableUIElement drawableUiElement, int layerIndex)
        {
            if (!layerCollection.ContainsKey(layerIndex))
                layerCollection.Add(layerIndex, new List<DrawableUIElement>());

            layerCollection[layerIndex].Add(drawableUiElement);
        }
    }
}
