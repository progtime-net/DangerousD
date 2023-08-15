using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace DangerousD.GameCore.Graphics
{
    public class AnimationBuilder
    {
        public List<AnimationContainer> Animations { get; private set; }
        public void LoadAnimations()
        {
            Animations = new List<AnimationContainer>();
            string[] animationFilesNames = Directory.GetFiles("../../../Content/animations");

            StreamReader reader;
            foreach (var fileName in animationFilesNames)
            {
                reader = new StreamReader(fileName);
                string json = reader.ReadToEnd();
                AnimationContainer animation = JsonConvert.DeserializeObject<AnimationContainer>(json);
                Animations.Add(animation);
                reader.Close();
            }
            
        }
    }
}
