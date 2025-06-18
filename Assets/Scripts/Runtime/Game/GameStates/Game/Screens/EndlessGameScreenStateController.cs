using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Gameplay;
using Runtime.Game.GameStates.Game.Popups;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Screens
{
    public class EndlessGameScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly GameSetupController _gameSetupController;
        private readonly MergeController _mergeController;
        private readonly EndlessGameOverPopupStateController _endlessGameOverPopupStateController; 
        private readonly PausePopupStateController _pausePopupStateController;
        private readonly IAudioService _audioService;
        
        private EndlessGameScreen _screen;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        public EndlessGameScreenStateController(ILogger logger, IUiService uiService, 
            GameSetupController gameSetupController, MergeController mergeController,
            EndlessGameOverPopupStateController endlessGameOverPopupStateController,
            PausePopupStateController pausePopupStateController, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _gameSetupController = gameSetupController;
            _mergeController = mergeController;
            _endlessGameOverPopupStateController = endlessGameOverPopupStateController;
            _pausePopupStateController = pausePopupStateController;
            _audioService = audioService;
        }
        
        public override UniTask Enter(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = new();
            
            CreateScreen();
            SubscribeToEvents();
            StartGame();
            return UniTask.CompletedTask;
        }
        
        public override async UniTask Exit()
        {
            EndGame();
            await _uiService.HideScreen(ConstScreens.EndlessGameScreen);
        }
        
        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<EndlessGameScreen>(ConstScreens.EndlessGameScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }
        
        private void SubscribeToEvents()
        {
            _mergeController.OnPlayerLost += ProcessLose;
            _screen.OnPausePressed += () => _pausePopupStateController.Enter().Forget();
        }

        private void StartGame()
        {
            _gameSetupController.StartEndlessGame(_cancellationTokenSource.Token);
        }

        private void EndGame()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            
            _gameSetupController.EndGame();
            _mergeController.OnPlayerLost -= ProcessLose;
        }

        private void ProcessLose()
        {
            _audioService.PlaySound(ConstAudio.LoseSound);
            _endlessGameOverPopupStateController.Enter().Forget();
        }
    }
}