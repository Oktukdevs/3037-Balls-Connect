using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.UI.Popup;
using Runtime.Game.Gameplay;
using Runtime.Game.GameStates.Game.Screens;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.Shop;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Popups
{
    public class WinPopupStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly RewardCalculator _rewardCalculator;
        private readonly GameplayData _gameplayData;
        private readonly UserProgressService _userProgressService;
        private readonly UserInventoryService _userInventoryService;
        private readonly ClearRatingCalculator _clearRatingCalculator;
        
        public WinPopupStateController(ILogger logger, IUiService uiService, GameplayData gameplayData,
            UserProgressService userProgressService, RewardCalculator rewardCalculator, 
            ClearRatingCalculator clearRatingCalculator, UserInventoryService userInventoryService) : base(logger)
        {
            _uiService = uiService;
            _userProgressService = userProgressService;
            _gameplayData = gameplayData;
            _rewardCalculator = rewardCalculator;
            _clearRatingCalculator = clearRatingCalculator;
            _userInventoryService = userInventoryService;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            Time.timeScale = 0;
            WinPopup popup = await _uiService.ShowPopup(ConstPopups.WinPopup) as WinPopup;

            int level = _gameplayData.GameLevelId;
            int stars = _clearRatingCalculator.GetStarsAmount();
            int reward = _rewardCalculator.CalculateWinReward();
            
            _userInventoryService.AddBalance(reward);
            
            _userProgressService.RecordLevelClear(level, stars, _gameplayData.TimeLeft);
            
            popup.SetData(level, reward, _gameplayData.TimeLeft, _userProgressService.GetBestTime(level), stars);
            
            popup.OnHomePressed += async () =>
            {
                Time.timeScale = 1;
                popup.DestroyPopup();
                await GoTo<MenuStateController>();
            };
            
            popup.OnNextPressed += async () =>
            {
                Time.timeScale = 1;
                popup.DestroyPopup();

                if (_userProgressService.NextLevelExists(_gameplayData.GameLevelId))
                    _gameplayData.GameLevelId++;
                
                await GoTo<LevelGameScreenStateController>();
            };
        }
    }
}