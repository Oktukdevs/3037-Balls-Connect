using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.Controllers;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.UserData;
using UnityEngine;
using AudioType = Runtime.Core.Audio.AudioType;

namespace Runtime.Game.GameStates.Bootstrap.Controllers
{
    public class AudioSettingsBootstrapController : BaseController
    {
        private readonly IAudioService _audioService;
        private readonly ISettingProvider _staticSettingsService;
        private readonly UserDataService _userDataService;

        private CancellationTokenSource _cancellationTokenSource;

        public AudioSettingsBootstrapController(IAudioService audioService, ISettingProvider staticSettingsService, UserDataService userDataService)
        {
            _audioService = audioService;
            _staticSettingsService = staticSettingsService;
            _userDataService = userDataService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);

            _cancellationTokenSource = new CancellationTokenSource();
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);

            SetVolume();
            PlayMusic(linkedTokenSource.Token).Forget();
            return UniTask.CompletedTask;
        }

        public override UniTask Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            return base.Stop();
        }

        private async UniTask PlayMusic(CancellationToken cancellationToken)
        {
            var audioSettings = _staticSettingsService.Get<AudioConfig>();
            var allMusicAudioData = audioSettings.Audio.FindAll(x => x.AudioType == AudioType.Music);
            var allMusicClips = new List<AudioClip>(allMusicAudioData.Count);

            foreach (var audioData in allMusicAudioData)
                allMusicClips.Add(audioData.Clip);

            var clipsCount = allMusicClips.Count;

            var clipIndex = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                var clipDuration = (int)allMusicClips[clipIndex].length * 1000 + 1000;
                _audioService.PlayMusic(allMusicClips[clipIndex]);
                await UniTask.Delay(clipDuration, cancellationToken: cancellationToken);
                clipIndex++;
                if (clipIndex >= clipsCount)
                    clipIndex = 0;
            }
        }

        private void SetVolume()
        {
            var soundVolume = _userDataService.GetUserData().SettingsData.SoundVolume;
            _audioService.SetVolume(AudioType.Sound, soundVolume);

            var musicVolume = _userDataService.GetUserData().SettingsData.MusicVolume;
            _audioService.SetVolume(AudioType.Music, musicVolume);
        }
    }
}