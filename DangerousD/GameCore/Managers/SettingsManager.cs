using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.Xna;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Serialization;

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
            AppManager.Instance.resolution = resolution;
        }
        public void SetMainVolume(float volume)
        {
            settingsContainer.MainVolume = MainVolume;
            //AppManager.Instance.SoundManager.

        }
        public void SetMusicVolume(float volume)
        {
            settingsContainer.MusicVolume = MainVolume;
            SaveSettings();

        }
        public void SetSoundEffectsVolume(float volume)
        {
            settingsContainer.SoundEffectsVolume = MainVolume;
            SaveSettings();

        }
        public void SetIsFullScreen(bool isFullScreen)
        {
            settingsContainer.IsFullScreen = isFullScreen;
            AppManager.Instance.SetIsFullScreen(isFullScreen);
            SaveSettings();
        }
        public void LoadSettings()
        {
            if (!File.Exists("GameSettings.txt"))
            {
                SaveSettings();
                return;
            }

            settingsContainer = JsonConvert.DeserializeObject<SettingsContainer>(File.ReadAllText("GameSettings.txt"));
            SetIsFullScreen(settingsContainer.IsFullScreen);
            SetMainVolume(settingsContainer.MainVolume);
            SetMusicVolume(settingsContainer.MusicVolume);
            SetResolution(settingsContainer.Resolution);
            SetSoundEffectsVolume(settingsContainer.SoundEffectsVolume);


        }
        public void SaveSettings()
        {
            using (StreamWriter streamWriter = new StreamWriter("GameSettings.txt"))
            {
                string _str = JsonConvert.SerializeObject(settingsContainer);
                streamWriter.Write(_str);
            } 
        }

    }
    [Serializable]
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
        public Point Resolution { get; set; } = new Point(1366,768);
    }
}
