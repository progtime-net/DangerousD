using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.Xna;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DangerousD.GameCore.Managers
{
    public class SettingsManager
    {
        private SettingsContainer settingsContainer= new SettingsContainer(); 
        public bool IsFullScreen { get => settingsContainer.IsFullScreen; } 
        public float MainVolume { get => settingsContainer.MainVolume; }
        public float MusicVolume { get => settingsContainer.MusicVolume; }
        public float SoundEffectsVolume { get => settingsContainer.SoundEffectsVolume; }
        public Point Resolution { get => settingsContainer.Resolution; }
        public void SetResolution(Point resolution)
        {
            settingsContainer.Resolution = resolution;
        }
        public void SetMainVolume(float volume)
        {
            settingsContainer.MainVolume = MainVolume;

        }
        public void SetMusicVolume(float volume)
        {
            settingsContainer.MusicVolume = MainVolume;

        }
        public void SetSoundEffectsVolume(float volume)
        {
            settingsContainer.SoundEffectsVolume = MainVolume;

        }
        public void SetIsFullScreen(bool isFullScreen)
        {
            settingsContainer.IsFullScreen = isFullScreen;
        }
        public void LoadSettings()
        {
            if (!File.Exists("GameSettings.txt"))
            {
                File.Create("GameSettings.txt");
                SaveSettings();
                return;
            }
                
            var serializedObject = JsonConvert.DeserializeObject<SettingsContainer>(File.ReadAllText("GameSettings.txt")); 
            
        }
        public void SaveSettings()
        {
            if (!File.Exists("GameSettings.txt"))
                File.Create("GameSettings.txt");
            File.WriteAllText("GameSettings.txt", JsonConvert.SerializeObject(settingsContainer));
        }

    }
    public class SettingsContainer
    {
        [JsonProperty("IsFullScreen")]
        public bool IsFullScreen { get; set; } = false;
        [JsonProperty("MainVolume")]
        public float MainVolume { get; set; } = 1;
        [JsonProperty("MusicVolume")]
        public float MusicVolume { get; set; } = 1;
        [JsonProperty("SoundEffectsVolume")]
        public float SoundEffectsVolume { get; set; } = 1;
        [JsonProperty("Resolution")]
        public Point Resolution { get; set; } = new Point(1920,1080);
    }
}
