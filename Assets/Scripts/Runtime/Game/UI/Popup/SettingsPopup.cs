using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Core.UI.Popup;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.UI.Popup.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Popup
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Slider _soundVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Button _privacyPolicyButton;
        [SerializeField] private Button _termsOfUseButton;
        
        public event Action<float> SoundVolumeChangeEvent;
        public event Action<float> MusicVolumeChangeEvent;
        
        public event Action OnPrivacyPolicyClick;
        public event Action OnTermsOfUseClick;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SettingsPopupData settingsPopupData = data as SettingsPopupData;

            _soundVolumeSlider.value = settingsPopupData.SoundVolume;
            _musicVolumeSlider.value = settingsPopupData.MusicVolume;

            _closeButton.onClick.AddListener(DestroyPopup);
            //_privacyPolicyButton.onClick.AddListener(() => OnPrivacyPolicyClick?.Invoke());
            //_termsOfUseButton.onClick.AddListener(() => OnTermsOfUseClick?.Invoke());

            _soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeToggleValueChanged);
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeToggleValueChanged);

            AudioService.PlaySound(ConstAudio.OpenPopupSound);

            return base.Show(data, cancellationToken);
        }

        public override void DestroyPopup()
        {
            AudioService.PlaySound(ConstAudio.PressButtonSound);
            Destroy(gameObject);
        }

        private void OnSoundVolumeToggleValueChanged(float value)
        {
            SoundVolumeChangeEvent?.Invoke(value);
        }

        private void OnMusicVolumeToggleValueChanged(float value)
        {
            MusicVolumeChangeEvent?.Invoke(value);
        }
    }
}