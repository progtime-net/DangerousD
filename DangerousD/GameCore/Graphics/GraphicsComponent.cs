using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DangerousD.GameCore.Graphics
{
    class GraphicsComponent
    {
        private List<AnimationContainer> animations;
        private List<Texture2D> textures;
        private List<string> texturesNames;
        private AnimationContainer currentAnimation;
        private AnimationContainer neitralAnimation;
        //private SpriteBatch _spriteBatch;
        
        private int currentFrame;
        private int interval;
        private int lastInterval;
        private Rectangle sourceRectangle;
        public GraphicsComponent(List<string> animationsId, string neitralAnimationId)
        {
            //this._spriteBatch = _spriteBatch;
            currentFrame = 0;
            lastInterval = 1;
            
            LoadAnimations(animationsId,neitralAnimationId);
            

        }
        private void LoadAnimations(List<string> animationsId, string neitralAnimationId)
        {
            animations = new List<AnimationContainer>();
            foreach (var id in animationsId)
            {
                animations.Add( GameManager.builder.animations.Find(x => x.Id == id));
                if (id==neitralAnimationId)
                {
                    neitralAnimation = animations.Last();
                }
            }
        }
        public void LoadContent(ContentManager content)
        {
            textures = new List<Texture2D>();
            texturesNames = new List<string>();

            foreach (var animation in animations)
            {
                if (!texturesNames.Contains(animation.TextureName))
                {
                    texturesNames.Add(animation.TextureName);
                    textures.Add(content.Load<Texture2D>(animation.TextureName));

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
        public void DrawAnimation(Rectangle destinationRectangle,  SpriteBatch _spriteBatch)
        {

            
                
            

            if (interval == 0)
            {
                currentFrame++;
                if (currentAnimation.FramesCount - 1 <= currentFrame)
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

            _spriteBatch.Draw(textures[texturesNames.FindIndex(x => x == currentAnimation.TextureName)], destinationRectangle, sourceRectangle, Color.White);
        }
        private void buildSourceRectangle()
        {
            sourceRectangle = new Rectangle();
            sourceRectangle.X = currentAnimation.StartSpriteRectangle.X + currentFrame * (currentAnimation.StartSpriteRectangle.Width + currentAnimation.TextureFrameInterval);
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
