using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DangerousD.GameCore.Graphics
{
    [Serializable]
    public class AnimationContainer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("textureName")]
        public string TextureName { get; set; }
        [JsonProperty("startSpriteRectangle")]
        public Rectangle StartSpriteRectangle { get; set; }
        [JsonProperty("frameSecond")]
        public List<Tuple<int, int>> FrameTime { get; set; }
        [JsonProperty("textureFrameInterval")]
        public int TextureFrameInterval { get; set; }
        [JsonProperty("framesCount")]
        public int FramesCount { get; set; }

    }
}
