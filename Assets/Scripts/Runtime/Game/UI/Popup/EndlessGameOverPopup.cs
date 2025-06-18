using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.UI.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class EndlessGameOverPopup : BasePopup
    {
        private const float TextAnimationDuration = 0.2f;
        
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private TextMeshProUGUI _rewardText;
        
        public event Action OnHomeButtonPressed;
        public event Action OnRestartButtonPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _homeButton.onClick.AddListener(() => OnHomeButtonPressed?.Invoke());
            _restartButton.onClick.AddListener(() => OnRestartButtonPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetData(int score, int bestScore, int reward)
        {
            AnimateNumberChange(_scoreText, score);
            AnimateNumberChange(_bestScoreText, bestScore);
            AnimateNumberChange(_rewardText, reward);
        }
        
        private void AnimateNumberChange(TextMeshProUGUI text, int targetScore)
        {
            int from = 0;
            DOTween.To(() => from, x =>
            {
                from = x;
                text.text = x.ToString();
            }, targetScore, TextAnimationDuration).SetEase(Ease.OutQuad).SetLink(gameObject);
        }
    }
}