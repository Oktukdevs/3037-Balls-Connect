using Runtime.Core.UI.Data;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.UI.Popup.Data
{
    public class SettingsPopupData : BasePopupData
    {
        private float _soundVolume;
        private float _musicVolume;

        public float SoundVolume => _soundVolume;
        public float MusicVolume => _musicVolume;

        public SettingsPopupData(float soundVolume, float musicVolume)
        {
            _soundVolume = soundVolume;
            _musicVolume = musicVolume;
        }
    }
}