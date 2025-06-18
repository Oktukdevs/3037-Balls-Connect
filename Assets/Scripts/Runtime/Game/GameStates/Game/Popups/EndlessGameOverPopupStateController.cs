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
    public class EndlessGameOverPopupStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly RewardCalculator _rewardCalculator;
        private readonly UserProgressService _userProgressService;
        private readonly UserInventoryService _userInventoryService;
        private readonly GameplayData _gameplayData;
        
        public EndlessGameOverPopupStateController(ILogger logger, IUiService uiService, 
            UserProgressService userProgressService, UserInventoryService userInventoryService,
            GameplayData gameplayData, RewardCalculator rewardCalculator) : base(logger)
        {
            _uiService = uiService;
            _userProgressService = userProgressService;
            _userInventoryService = userInventoryService;
            _gameplayData = gameplayData;
            _rewardCalculator = rewardCalculator;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            int reward = _rewardCalculator.CalculateEndlessGameReward();
            int score = _gameplayData.Score;
            
            _userProgressService.RecordScore(score);
            _userInventoryService.AddBalance(reward);

            int bestScore = _userProgressService.GetBestScore();

            EndlessGameOverPopup popup = await _uiService.ShowPopup(ConstPopups.EndlessGameOverPopup) as EndlessGameOverPopup;
            popup.SetData(score, bestScore, reward);

            popup.OnHomeButtonPressed += async () =>
            {
                popup.DestroyPopup();
                await GoTo<MenuStateController>();
            };

            popup.OnRestartButtonPressed += async () =>
            {
                popup.DestroyPopup();
                await GoTo<EndlessGameScreenStateController>();
            };
        }
    }
}