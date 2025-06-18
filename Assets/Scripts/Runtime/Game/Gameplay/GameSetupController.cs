using System.Threading;
using Runtime.Core.Infrastructure.SettingsProvider;

namespace Runtime.Game.Gameplay
{
    public class GameSetupController
    {
        private readonly BallBoard _ballBoard;
        private readonly PathDrawController _pathDrawController;
        private readonly SwapDrawController _swapDrawController;
        private readonly BallsPreviewController _ballPreviewController;
        private readonly BallsAnimationController _animationController;
        private readonly BackgroundActivator _backgroundActivator;
        private readonly SpritesProvider _spritesProvider;
        private readonly BallsPool _ballPool;
        private readonly GameplayData _gameplayData;
        private readonly GameplayTimer _gameplayTimer;
        private readonly MergeController _mergeController;
        
        public GameSetupController(BallBoard ballBoard, PathDrawController pathDrawController, BallsPreviewController ballsPreviewController,
            BallsAnimationController ballsAnimationController, BackgroundActivator backgroundActivator, SpritesProvider spritesProvider,
            SwapDrawController swapDrawController, BallsPool ballsPool, GameplayData gameplayData, GameplayTimer gameplayTimer,
            MergeController mergeController)
        {
            _ballBoard = ballBoard;
            _pathDrawController = pathDrawController;
            _ballPreviewController = ballsPreviewController;
            _animationController = ballsAnimationController;
            _backgroundActivator = backgroundActivator;
            _spritesProvider = spritesProvider;
            _swapDrawController = swapDrawController;
            _ballPool = ballsPool;
            _gameplayData = gameplayData;
            _gameplayTimer = gameplayTimer;
            _mergeController = mergeController;
        }

        public async void StartEndlessGame(CancellationToken token)
        {
            _animationController.SetToken(token);
            _mergeController.SetToken(token);
            
            ResetGameplayData();
            EnableBackground();
            _ballBoard.Initialize();
            
            await _animationController.PlayAppearAnimation(_ballPreviewController.CreateRowPreview());
            token.ThrowIfCancellationRequested();
            
            _pathDrawController.SetEnabled(true);
        }

        public void StartLevel(LevelConfig levelConfig, CancellationToken token)
        {
            _gameplayTimer.Start(levelConfig.TimeTotal, token).Forget();
            
            StartEndlessGame(token);
            
            _gameplayData.ProgressData = new();
            for (int i = 0; i < levelConfig.ClearConditions.Count; i++)
            {
                _gameplayData.ProgressData.Add(new()
                {
                    ClearCondition = levelConfig.ClearConditions[i],
                    Progress = 0
                });
            }
        }

        public void EndGame()
        {
            _backgroundActivator.Enable(false);
            
            _pathDrawController.Reset();
            _swapDrawController.Reset();
            
            _ballPool.ReturnAll();
            _ballPreviewController.Cleanup();
        }

        private void EnableBackground()
        {
            _backgroundActivator.Enable(true);

            _backgroundActivator.SetBackgroundSprite(_spritesProvider.GetBackgroundSprite());
            _backgroundActivator.SetFieldSprite(_spritesProvider.GetGameplaySprites());
        }

        private void ResetGameplayData()
        {
            _gameplayData.Score = 0;
            _gameplayData.HighestValue = 0;
            _gameplayData.HighestMatchCount = 0;
            _gameplayData.TimeLeft = 0;
        }
    }
}