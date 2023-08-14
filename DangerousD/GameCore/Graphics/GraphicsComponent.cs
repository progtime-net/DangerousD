using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore.Graphics
{
    class GraphicsComponent
    {
        private List<AnimationContainer> animations;
        private List<Texture2D> textures;
        private List<string> texturesNames;
        private AnimationContainer currentAnimation;
        private SpriteBatch _spriteBatch;
        private string lastAnimationId;
        private int currentFrame;
        private int interval;
        private int lastInterval;
        private Rectangle sourceRectangle;
        public GraphicsComponent(List<string> animationsId, SpriteBatch _spriteBatch)
        {
            this._spriteBatch = _spriteBatch;
            currentFrame = 0;
            lastInterval = 1;
            lastAnimationId = null;
            LoadAnimations(animationsId);
            LoadTextures();

        }
        private void LoadAnimations(List<string> animationsId)
        {
            animations = new List<AnimationContainer>();
            foreach (var id in animationsId)
            {
                animations.Add( GameManager.builder.animations.Find(x => x.Id == id));
            }
        }
        private void LoadTextures()
        {
            textures = new List<Texture2D>();
            texturesNames = new List<string>();

            foreach (var animation in animations)
            {
                if (!texturesNames.Contains(animation.TextureName))
                {
                    texturesNames.Add(animation.TextureName);
                    textures.Add(TextureManager.GetTexture(animation.TextureName));

                }
            }
        }
        public void DrawAnimation(Rectangle destinationRectangle, string animationId)
        {

            if (animationId != lastAnimationId)
            {
                currentFrame = 0;
                currentAnimation = animations.Find(x => x.Id == animationId);
                buildSourceRectangle();
                SetInterval();
            }

            if (interval == 0)
            {
                currentFrame++;
                if (currentAnimation.FramesCount - 1 <= currentFrame)
                {
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
