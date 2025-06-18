using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.UI.Popup;
using Runtime.Game.Gameplay;
using Runtime.Game.GameStates.Game.Screens;
using Runtime.Game.Services.UI;
using Runtime.Game.Shop;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Popups
{
    public class LosePopupStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly RewardCalculator _rewardCalculator;
        private readonly GameplayData _gameplayData;
        private readonly UserInventoryService _userInventoryService;
        
        public LosePopupStateController(ILogger logger, IUiService uiService, RewardCalculator rewardCalculator, 
            GameplayData gameplayData, UserInventoryService userInventoryService) : base(logger)
        {
            _uiService = uiService;
            _rewardCalculator = rewardCalculator;
            _gameplayData = gameplayData;
            _userInventoryService = userInventoryService;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            Time.timeScale = 0;
            LosePopup popup = await _uiService.ShowPopup(ConstPopups.LosePopup) as LosePopup;

            int levelId = _gameplayData.GameLevelId;
            int reward = _rewardCalculator.CalculateLoseReward();
            string failText = _gameplayData.TimeLeft <= 0 ? "Time ran out..." : "Level failed...";
            
            _userInventoryService.AddBalance(reward);
            
            popup.SetData(levelId, reward, failText);
            
            popup.OnHomePressed += async () =>
            {
                Time.timeScale = 1;
                popup.DestroyPopup();
                await GoTo<MenuStateController>();
            };
            
            popup.OnReplayPressed += async () =>
            {
                Time.timeScale = 1;
                popup.DestroyPopup();
                await GoTo<LevelGameScreenStateController>();
            };
        }
    }
}