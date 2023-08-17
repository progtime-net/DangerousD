using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DangerousD.GameCore.Graphics
{
    public class GraphicsComponent
    {
        private List<AnimationContainer> animations;
        private List<Texture2D> textures;
        private List<string> texturesNames;
        private AnimationContainer currentAnimation;
        public AnimationContainer CurrentAnimation
        {
            get
            {
                return currentAnimation;
            }
        }
        public string LastAnimation { get; set; }
        public string GetCurrentAnimation
        {
            get { return currentAnimation.Id; }
        }

        private AnimationContainer neitralAnimation;
        //private SpriteBatch _spriteBatch;

        private int currentFrame;
        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }
        }
        private int interval;
        private int lastInterval;
        private Rectangle sourceRectangle;

        public GraphicsComponent(List<string> animationsId, string neitralAnimationId)
        {
            //this._spriteBatch = _spriteBatch;
            currentFrame = 0;
            lastInterval = 1;

            LoadAnimations(animationsId, neitralAnimationId);
            currentAnimation = neitralAnimation;
            SetInterval();
            buildSourceRectangle();
        }

        public GraphicsComponent(string textureName)
        {
            animations = new List<AnimationContainer>();
            textures = new List<Texture2D>();
            var texture = AppManager.Instance.Content.Load<Texture2D>(textureName);
            textures.Add(texture);
            AnimationContainer animationContainer = new AnimationContainer();
            animationContainer.StartSpriteRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            animationContainer.TextureFrameInterval = 0;
            animationContainer.TextureName = texture.Name;
            animationContainer.IsCycle = true;
            animationContainer.FramesCount = 1;
            animationContainer.FrameTime = new List<Tuple<int, int>>() { new Tuple<int, int>(0, 10) };
            animationContainer.Id = texture.Name;
            currentAnimation = animationContainer;
            neitralAnimation = animationContainer;
            animations.Add(animationContainer);
        }

        private void LoadAnimations(List<string> animationsId, string neitralAnimationId)
        {
            animations = new List<AnimationContainer>();
            foreach (var id in animationsId)
            {
                animations.Add(AppManager.Instance.AnimationBuilder.Animations.Find(x => x.Id == id));
                if (id == neitralAnimationId)
                {
                    neitralAnimation = animations.Last();
                }
            }
        }

        public void LoadContent()
        {
            textures = new List<Texture2D>();
            texturesNames = new List<string>();

            foreach (var animation in animations)
            {
                if (!texturesNames.Contains(animation.TextureName))
                {
                    texturesNames.Add(animation.TextureName);
                    textures.Add(AppManager.Instance.Content.Load<Texture2D>(animation.TextureName));
                }
            }
        }

        public void StartAnimation(string startedanimationId)
        {
            currentFrame = 0;
            currentAnimation = animations.Find(x => x.Id == startedanimationId);

            buildSourceRectangle();
            SetInterval();
        }

        public void StopAnimation()
        {
            currentFrame = 0;
            interval = 0;
            currentAnimation = neitralAnimation;
            buildSourceRectangle();
            SetInterval();
        }

        public void Update()
        {
            if (interval == 0)
            {
                currentFrame++;
                if (currentAnimation.FramesCount <= currentFrame)
                {
                    if (!currentAnimation.IsCycle)
                    {
                        currentAnimation = neitralAnimation;
                    }

                    currentFrame = 0;
                }

                buildSourceRectangle();
                SetInterval();
            }

            interval--;
        }

        public void DrawAnimation(Rectangle destinationRectangle, SpriteBatch _spriteBatch)
        {
            Texture2D texture = textures[texturesNames.FindIndex(x => x == currentAnimation.TextureName)];
            float scale;
            if (currentAnimation.Offset.X!=0)
            {
                destinationRectangle.X -= (int)currentAnimation.Offset.X;
                scale=destinationRectangle.Height/sourceRectangle.Height;
                destinationRectangle.Width = (int)(sourceRectangle.Width * scale);
                
            }
            else if (currentAnimation.Offset.Y != 0)
            {
                destinationRectangle.Y -= (int)currentAnimation.Offset.Y;
                scale = destinationRectangle.Width / sourceRectangle.Width;
                destinationRectangle.Height = (int)(sourceRectangle.Height * scale);
            }
           
            
            _spriteBatch.Draw(texture,
                destinationRectangle, sourceRectangle, Color.White);
        }

        private void buildSourceRectangle()
        {
            sourceRectangle = new Rectangle();
            sourceRectangle.X = currentAnimation.StartSpriteRectangle.X + currentFrame *
                (currentAnimation.StartSpriteRectangle.Width + currentAnimation.TextureFrameInterval);
            sourceRectangle.Y = currentAnimation.StartSpriteRectangle.Y;
            sourceRectangle.Height = currentAnimation.StartSpriteRectangle.Height;
            sourceRectangle.Width = currentAnimation.StartSpriteRectangle.Width;
        }

        private void SetInterval()
        {
            Tuple<int, int> i = currentAnimation.FrameTime.Find(x => x.Item1 == currentFrame);
            if (i != null)
            {
                interval = i.Item2;
                lastInterval = interval;
            }
            else
            {
                interval = lastInterval;
            }
        }
    }
}