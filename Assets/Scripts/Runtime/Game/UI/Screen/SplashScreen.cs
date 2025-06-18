using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class SplashScreen : UiScreen
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private float _durationMin;
        [SerializeField] private float _durationMax;
        
        public override async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await PlayLoadingAnim(cancellationToken);
            await base.HideAsync(cancellationToken);
        }

        private async UniTask PlayLoadingAnim(CancellationToken cancellationToken)
        {
            float duration = Random.Range(_durationMin, _durationMax);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();

                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);
                _slider.value = progress;
                _progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            _slider.value = 1f;
            _progressText.text = "100%";
        }
    }
}