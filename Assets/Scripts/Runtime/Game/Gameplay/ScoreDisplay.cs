using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.Game.Gameplay
{
    public class ScoreDisplay : MonoBehaviour
    {
        private const float TextAnimationDuration = 0.2f;
        
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private int _prevScore = 0;

        private GameplayData _gameplayData;
        
        [Inject]
        private void Construct(GameplayData gameplayData)
        {
            _gameplayData = gameplayData;
            _gameplayData.OnScoreChanged += AnimateNumberChange;
        }

        private void OnDestroy()
        {
            _gameplayData.OnScoreChanged -= AnimateNumberChange;
        }

        private void AnimateNumberChange(int targetScore)
        {
            DOTween.To(() => _prevScore, x =>
            {
                _prevScore = x;
                _scoreText.text = x.ToString();
            }, targetScore, TextAnimationDuration).SetEase(Ease.OutQuad).SetLink(gameObject);
        }
    }
}