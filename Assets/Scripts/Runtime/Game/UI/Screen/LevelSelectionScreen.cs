using System;
using Runtime.Game.Services.UserData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Game.UI.Screen
{
    public class LevelSelectionScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private LevelButton[] _levelButtons;

        [SerializeField] private Sprite _clearedSprite;
        [SerializeField] private Sprite _lastSprite;
        [SerializeField] private Sprite _lockedSprite;
        
        
        private UserProgressService _userProgressService;
        
        public event Action OnBackPressed;
        public event Action<int> OnLevelSelected;

        [Inject]
        private void Construct(UserProgressService userProgressService)
        {
            _userProgressService = userProgressService;
        }
        
        public void Initialize()
        {
            SubscribeEvents();
            InitializeButtons();
        }
        
        private void SubscribeEvents()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
        }

        private void InitializeButtons()
        {
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                bool locked = i > _userProgressService.GetLastUnlockedLevel();

                var button = _levelButtons[i];
                button.Initialize(SelectSprite(i), i, 
                    _userProgressService.GetLevelClearStars(i), locked);
                
                button.OnLevelSelected += ProcessLevelSelect;
            }
        }

        private void ProcessLevelSelect(int levelId) => OnLevelSelected?.Invoke(levelId);

        private Sprite SelectSprite(int i)
        {
            bool last = i == _userProgressService.GetLastUnlockedLevel();

            if (last)
                return _lastSprite;
            
            bool locked = i > _userProgressService.GetLastUnlockedLevel();
            
            if(locked)
                return _lockedSprite;

            return _clearedSprite;
        }
    }
}