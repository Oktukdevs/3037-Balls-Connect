using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class LevelGameScreen : UiScreen
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private ProgressDisplayController _progressDisplay;
        
        public event Action OnPausePressed;
        
        public void Initialize()
        {
            SubscribeEvents();
        }

        public void SetLevelConfig(LevelConfig levelConfig) => _progressDisplay.Initialize(levelConfig);
        
        private void SubscribeEvents()
        {
            _pauseButton.onClick.AddListener(() => OnPausePressed?.Invoke());
        }
    }
}