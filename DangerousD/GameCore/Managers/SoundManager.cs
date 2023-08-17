using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using DangerousD.GameCore.Graphics;
using Newtonsoft.Json;

namespace DangerousD.GameCore
{
    public class SoundManager
    {
        public Dictionary<string, SoundEffectInstance> Sounds = new Dictionary<string, SoundEffectInstance>(); // словарь со звуками где строка - название файла
        public List<Sound> PlayingSounds = new List<Sound>(); // список со всеми звуками, которые проигрываются
        public float MaxSoundDistance = 1500; // максимальная дальность звука

        public void LoadSounds() // метод для загрузки звуков из папки
        {
            var k = Directory.GetFiles("../../..//Content").Where(x => x.EndsWith("mp3"));
            if (k.Count()>0)
            {

                string[] soundFiles = k.Select(x => x.Split("\\").Last().Split("/").Last().Replace(".mp3", "")).ToArray();// папка со звуками там где exe 
                foreach (var soundFile in soundFiles)
                {
                    Sounds.Add(soundFile, AppManager.Instance.Content.Load<SoundEffect>(soundFile).CreateInstance());
                }

            }
        }

        public void StartAmbientSound(string soundName) // запустить звук у которого нет позиции
        {
            var sound = new Sound(Sounds[soundName]);
            sound.SoundEffect.IsLooped = false;
            sound.SoundEffect.Play();
            PlayingSounds.Add(sound);
            if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host)
            {
                AppManager.Instance.NetworkManager.SendMsg(new Network.NetworkTask(Vector2.Zero, soundName));
            }
        }
        public void StartSound(string soundName, Vector2 soundPos, Vector2 playerPos) // запустить звук у которого есть позиция
        {
            var sound = new Sound(Sounds[soundName], soundPos);
            sound.SoundEffect.IsLooped = false;
            sound.SoundEffect.Volume = (float)sound.GetDistance(playerPos) / MaxSoundDistance;
            sound.SoundEffect.Play();
            PlayingSounds.Add(sound);
            if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host) 
            {
                AppManager.Instance.NetworkManager.SendMsg(new Network.NetworkTask(soundPos, soundName));
            }
        } 
        public void StopAllSounds() // остановка всех звуков
        {
            foreach (var sound in PlayingSounds)
                sound.SoundEffect.Stop();
            PlayingSounds.Clear();
        }


        public void Update() // апдейт, тут происходит изменение громкости
        {

            
            var player = AppManager.Instance.GameManager.GetPlayer1;
            if (player != null)
            {
                for (int i = 0; i < PlayingSounds.Count; i++)
                { 
                    if (!PlayingSounds[i].isAmbient)
                        PlayingSounds[i].SoundEffect.Volume = (float)PlayingSounds[i].GetDistance(player.Pos) / MaxSoundDistance;
                    if (PlayingSounds[i].SoundEffect.State == SoundState.Stopped)
                        PlayingSounds.Remove(PlayingSounds[i]);
                }
            }
        }
    }

    public class Sound
    {
        public SoundEffectInstance SoundEffect { get; set; }
        public Vector2 Position { get; set; } // позиция для эффекта
        public bool isAmbient { get; }

        public Sound(SoundEffectInstance soundEffect) // конструктор для музыки и эмбиента
        {
            SoundEffect = soundEffect;
            isAmbient = true;
        }

        public Sound(SoundEffectInstance soundEffect, Vector2 position) // конструктор для эффектов по типу криков зомби
        {
            isAmbient = false;
            SoundEffect = soundEffect;
            Position = position;
        }

        public double GetDistance(Vector2 playerPos) // получение дистанции до объедка от игрока
        {
            return Math.Sqrt(Math.Pow(Position.X - playerPos.X, 2) + Math.Pow(Position.Y - playerPos.Y, 2));
        }
    }
}