using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.Controllers;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI.Popup;
using Runtime.Game.UI.Popup.Data;
using UnityEngine;
using AudioType = Runtime.Core.Audio.AudioType;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public sealed class SettingsPopupController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;

        public SettingsPopupController(IUiService uiService, UserDataService userDataService, IAudioService audioService)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);

            SettingsPopup settingsPopup = _uiService.GetPopup<SettingsPopup>(ConstPopups.SettingsPopup);

            settingsPopup.SoundVolumeChangeEvent += OnChangeSoundVolume;
            settingsPopup.MusicVolumeChangeEvent += OnChangeMusicVolume;

            settingsPopup.OnPrivacyPolicyClick += async () =>
            {
                settingsPopup.DestroyPopup();
                await _uiService.ShowPopup(ConstPopups.PrivacyPolicyPopup);
            };
            
            settingsPopup.OnTermsOfUseClick += async () =>
            {
                settingsPopup.DestroyPopup();
                await _uiService.ShowPopup(ConstPopups.TermsOfUsePopup);
            };

            var userData = _userDataService.GetUserData();

            var soundVolume = userData.SettingsData.SoundVolume;
            var musicVolume = userData.SettingsData.MusicVolume;

            settingsPopup.Show(new SettingsPopupData(soundVolume, musicVolume), cancellationToken).Forget();
            CurrentState = ControllerState.Complete;
            return UniTask.CompletedTask;
        }
        
        private void OnChangeSoundVolume(float volume)
        {
            _audioService.SetVolume(AudioType.Sound, volume);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.SoundVolume = volume;
        }

        private void OnChangeMusicVolume(float volume)
        {
            _audioService.SetVolume(AudioType.Music, volume);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.MusicVolume = volume;
        }
    }
}