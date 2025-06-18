using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class EndlessGameScreen : UiScreen
    {
        [SerializeField] private Button _pauseButton;
        
        public event Action OnPausePressed;
        
        public void Initialize()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _pauseButton.onClick.AddListener(() => OnPausePressed?.Invoke());
        }
    }
}