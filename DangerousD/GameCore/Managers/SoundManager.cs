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
using Microsoft.Xna.Framework.Media;
using DangerousD.GameCore.GameObjects.LivingEntities;

namespace DangerousD.GameCore
{
    public class SoundManager
    {
        public Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>(); // словарь со звуками где строка - название файла
        public List<Sound> PlayingSounds = new List<Sound>(); // список со всеми звуками, которые проигрываются
        public static float MaxSoundDistance = 100; // максимальная дальность звука

        public void LoadSounds() // метод для загрузки звуков из папки
        {
            var k = Directory.GetFiles("../../..//Content//sounds").Where(x => x.EndsWith("mp3"));

            if (k.Count() > 0)
            {

                string[] soundFiles = k.Select(x => x.Split("\\").Last().Split("/").Last().Replace(".mp3", "")).ToArray();// папка со звуками там где exe 
                foreach (var soundFile in soundFiles)
                {
                    Sounds.Add(soundFile, AppManager.Instance.Content.Load<SoundEffect>("sounds//" + soundFile));
                }

            }

            if (k.Count()>0)
            {

            }
        }

        public void StartAmbientSound(string soundName) // запустить звук у которого нет позиции
        {
            var sound = new Sound(Sounds[soundName].CreateInstance(), Vector2.One, true);
            sound.SoundEffect.Volume = sound.baseVolume * AppManager.Instance.SettingsManager.MusicVolume * AppManager.Instance.SettingsManager.MainVolume;
            sound.SoundEffect.IsLooped = false;

            sound.SoundEffect.Play();
            PlayingSounds.Add(sound);

            if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host)
            {
                AppManager.Instance.NetworkTasks.Add(new Network.NetworkTask(Vector2.Zero, soundName));
            }
        }
        public void StartSound(string soundName, Vector2 soundPos, Vector2 playerPos, float baseVolume = 1, float pitch = 0) // запустить звук у которого есть позиция
        {
            var sound = new Sound(Sounds[soundName].CreateInstance(), soundPos, false) { baseVolume = baseVolume, basePich = pitch };
            sound.SoundEffect.IsLooped = false;
            sound.SoundEffect.Volume = sound.baseVolume * AppManager.Instance.SettingsManager.SoundEffectsVolume * AppManager.Instance.SettingsManager.MainVolume;
            sound.SoundEffect.Pitch = pitch;
            sound.SoundEffect.Play();
            PlayingSounds.Add(sound);

            if (AppManager.Instance.multiPlayerStatus == MultiPlayerStatus.Host) 
            {
                AppManager.Instance.NetworkTasks.Add(new Network.NetworkTask(soundPos, soundName));
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
            for (int i = 0; i < PlayingSounds.Count; i++)
            {
                PlayingSounds[i].UpdateVolume(Vector2.Zero);
                if (PlayingSounds[i].SoundEffect.State == SoundState.Stopped)
                {
                    PlayingSounds.Remove(PlayingSounds[i]);
                    i--;
                }
            }
            return;
            
            var player = AppManager.Instance.GameManager.GetPlayer1;
            if (player != null)
            {
                for (int i = 0; i < PlayingSounds.Count; i++)
                { 
                    if (!PlayingSounds[i].isAmbient)
                        PlayingSounds[i].SoundEffect.Volume = (float)(MaxSoundDistance - PlayingSounds[i].GetDistanceVol(player.Pos)) / MaxSoundDistance;
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
        public float baseVolume { get; set; } = 1;
        public float basePich { get; set; } = 0;
        public Sound(SoundEffectInstance soundEffect, Vector2 position, bool isAmbient) // конструктор для эффектов по типу криков зомби
        {
            this.isAmbient = isAmbient; 
            SoundEffect = soundEffect;
            Position = position;
        }
        public void UpdateVolume(Vector2 playerPos)
        {
            if (isAmbient)
                SoundEffect.Volume = baseVolume * AppManager.Instance.SettingsManager.MusicVolume * AppManager.Instance.SettingsManager.MainVolume;
            else
                SoundEffect.Volume = baseVolume * AppManager.Instance.SettingsManager.SoundEffectsVolume * AppManager.Instance.SettingsManager.MainVolume;// * (float)Math.Clamp(1 - GetDistanceVol(playerPos),0,1);

        }

        public double GetDistanceVol(Vector2 playerPos) // получение дистанции до объедка от игрока
        {
            return Math.Sqrt(Math.Pow(Position.X - playerPos.X, 2) + Math.Pow(Position.Y - playerPos.Y, 2))-SoundManager.MaxSoundDistance;
        } 
    }
}