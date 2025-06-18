using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Gameplay;
using Runtime.Game.GameStates.Game.Popups;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Screens
{
    public class LevelGameScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly PausePopupStateController _pausePopupStateController;
        private readonly GameSetupController _gameSetupController;
        private readonly MergeController _mergeController;
        private readonly LevelProgressController _levelProgressController;
        private readonly ISettingProvider _settingProvider;
        private readonly GameplayData _gameplayData;
        private readonly GameplayTimer _gameplayTimer;
        private readonly WinPopupStateController _winPopupStateController;
        private readonly LosePopupStateController _losePopupStateController;
        private readonly IAudioService _audioService;
        
        private LevelGameScreen _screen;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        public LevelGameScreenStateController(ILogger logger, IUiService uiService, PausePopupStateController pausePopupStateController,
            GameSetupController gameSetupController, MergeController mergeController, LevelProgressController levelProgressController,
            ISettingProvider settingProvider, GameplayData gameplayData, GameplayTimer gameplayTimer,
            WinPopupStateController winPopupStateController, LosePopupStateController losePopupStateController, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _pausePopupStateController = pausePopupStateController;
            _gameSetupController = gameSetupController;
            _mergeController = mergeController;
            _levelProgressController = levelProgressController;
            _settingProvider = settingProvider;
            _gameplayData = gameplayData;
            _gameplayTimer = gameplayTimer;
            _winPopupStateController = winPopupStateController;
            _losePopupStateController = losePopupStateController;
            _audioService = audioService;
        }
        
        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            StartGame();
            return UniTask.CompletedTask;
        }
        
        public override async UniTask Exit()
        {
            EndGame();
            await _uiService.HideScreen(ConstScreens.LevelGameScreen);
        }
        
        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LevelGameScreen>(ConstScreens.LevelGameScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void StartGame()
        {
            _cancellationTokenSource = new();
            var levelConfig = _settingProvider.Get<GameConfig>().Levels[_gameplayData.GameLevelId];
            _gameSetupController.StartLevel(levelConfig, _cancellationTokenSource.Token);
            _screen.SetLevelConfig(levelConfig);
        }

        private void EndGame()
        {
            _gameSetupController.EndGame();
            _mergeController.OnPlayerLost -= ProcessLose;
            _gameplayTimer.OnTimeEnd -= ProcessLose;
            _mergeController.OnMergeComplete -= ProcessMerge;
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
        
        private void SubscribeToEvents()
        {
            _screen.OnPausePressed += () => _pausePopupStateController.Enter().Forget();
            _mergeController.OnPlayerLost += ProcessLose;
            _gameplayTimer.OnTimeEnd += ProcessLose;
            _mergeController.OnMergeComplete += ProcessMerge;
        }

        private void ProcessMerge()
        {
            for (int i = 0; i < _gameplayData.ProgressData.Count; i++)
            {
                var progressData = _gameplayData.ProgressData[i];
                if(progressData.Progress < progressData.ClearCondition.Matches)
                    return;
            }

            ShowWinPopup();
        }

        private void ProcessLose()
        {
            _audioService.PlaySound(ConstAudio.LoseSound);
            _losePopupStateController.Enter().Forget();
        }

        private void ShowWinPopup()
        {
            _audioService.PlaySound(ConstAudio.VictorySound);
            _winPopupStateController.Enter().Forget();
        }
    }
}