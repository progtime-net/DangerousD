using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using MonoGame.Extended.Serialization;
using Newtonsoft.Json;

namespace DangerousD.GameCore.Graphics
{
    public class AnimationBuilder
    {
        public List<AnimationContainer> Animations { get; private set; }
        public void LoadAnimations()
        {
            Animations = new List<AnimationContainer>();
            List<string> animations = AppManager.Instance.Content.Load<List<string>>("animations/index");
            foreach (var animation in animations)
            {
                Animations.Add(AppManager.Instance.Content.Load<AnimationContainer>("animations/" + animation));
            }
        }
    }
    
}
