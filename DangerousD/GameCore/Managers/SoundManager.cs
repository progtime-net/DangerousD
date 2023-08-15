using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace DangerousD.GameCore
{
    public class SoundManager
    {
        public Dictionary<string, SoundEffectInstance> Sounds = new Dictionary<string, SoundEffectInstance>(); // словарь со звуками где строка - название файла
        public List<Sound> PlayingSounds = new List<Sound>(); // список со всеми звуками, которые проигрываются
        public string SoundDirectory = "Sounds"; // папка со звуками там где exe
        public float MaxSoundDistance = 1500; // максимальная дальность звука

        public void LoadSounds(ContentManager content) // метод для загрузки звуков из папки
        {
            var soundFiles = Directory.GetFiles(SoundDirectory);
            foreach (var soundFile in soundFiles)
            {
                Sounds.Add(soundFile, content.Load<SoundEffectInstance>(soundFile));
            }
        }

        public void StartSound(string soundName, bool isMusic) // запустить звук у которого нет позиции
        {
            var sound = new Sound(Sounds[soundName]);
            sound.SoundEffect.IsLooped = false;
            if (isMusic)
                sound.SoundEffect.IsLooped = true;
            sound.SoundEffect.Play();
            PlayingSounds.Add(sound);
        }

        public void StartSound(string soundName, Vector2 soundPos, Vector2 playerPos) // запустить звук у которого есть позиция
        {
            var sound = new Sound(Sounds[soundName], soundPos);
            sound.SoundEffect.IsLooped = false;
            sound.SoundEffect.Volume = (float)sound.GetDistance(playerPos) / MaxSoundDistance;
            sound.SoundEffect.Play();
            PlayingSounds.Add(sound);
        }//GameManager.SendSound
        public void StopAllSounds() // остановка всех звуков
        {
            foreach (var sound in PlayingSounds)
                sound.SoundEffect.Stop();
            PlayingSounds.Clear();
        }

        public void Update(Vector2 playerPos) // апдейт, тут происходит изменение громкости
        {
            foreach (var sound in PlayingSounds)
            {
                if (!sound.isAmbient)
                    sound.SoundEffect.Volume = (float)sound.GetDistance(playerPos) / MaxSoundDistance;
                if (sound.SoundEffect.State == SoundState.Stopped)
                    PlayingSounds.Remove(sound);
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