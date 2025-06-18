using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Runtime.Game.Gameplay
{
    public class ComboDisplay : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _comboSprites;
        [SerializeField] private float baseDuration = 0.8f;
        [SerializeField] private float baseScale = 1f;

        private Tween _currentTween;

        private PathDrawController _pathDrawController;
        private ComboCalculator _comboCalculator;

        [Inject]
        private void Construct(PathDrawController pathDrawController, ComboCalculator comboCalculator)
        {
            _pathDrawController = pathDrawController;
            _comboCalculator = comboCalculator;
            
            _pathDrawController.OnMerged += ProcessMerge;
        }

        private void OnDestroy()
        {
            _pathDrawController.OnMerged -= ProcessMerge;
        }

        private void ProcessMerge(List<GameBall> path)
        {
            int combo = _comboCalculator.CalculateCombo(path);
            
            if(combo == 0)
                return;
            
            ShowCombo(combo);
        }

        private void ShowCombo(int combo)
        {
            combo = Mathf.Clamp(combo, 1, 5);
            _spriteRenderer.sprite = _comboSprites[combo - 1];

            float flashiness = combo / 5f;

            _spriteRenderer.DOKill();
            transform.DOKill();
            _spriteRenderer.color = new Color(1, 1, 1, 1);
            transform.localScale = Vector3.one * baseScale;

            Sequence seq = DOTween.Sequence();

            float punch = 0.2f + 0.3f * flashiness;
            float duration = baseDuration - 0.3f * flashiness;

            seq.Append(transform.DOPunchScale(Vector3.one * punch, 0.4f, vibrato: 4, elasticity: 0.8f));

            if (combo >= 3)
                seq.Join(transform.DOShakePosition(0.4f, strength: 0.2f * flashiness, vibrato: 10));

            seq.AppendInterval(duration);
            seq.Append(_spriteRenderer.DOFade(0f, 0.3f));
        }
    }
}