﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace DangerousD.GameCore.Graphics
{
    public class AnimationBuilder
    {
        public List<AnimationContainer> animations;
        void LoadAnimations(string nameOfMainFile)
        {
            animations = new List<AnimationContainer>();
            List<string> animationFilesNames = new List<string>();
            StreamReader reader = new StreamReader(nameOfMainFile);
            while (reader.Peek() != -1)
            {
                animationFilesNames.Add(reader.ReadLine());
            }
            reader.Close();
            foreach (var fileName in animationFilesNames)
            {
                reader = new StreamReader(fileName);
                string json = reader.ReadToEnd();
                AnimationContainer animation = JsonConvert.DeserializeObject<AnimationContainer>(json);
                animations.Add(animation);
            }
        }
    }
}
