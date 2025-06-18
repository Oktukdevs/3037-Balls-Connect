using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class MergeController : ICancellable
    {
        private readonly PathDrawController _pathDrawController;
        private readonly BallBoard _board;
        private readonly BallsPreviewController _previewController;
        private readonly BallsAnimationController _animationController;
        private readonly GameplayData _gameplayData;
        private readonly ComboCalculator _comboCalculator;
        private readonly IAudioService _audioService;

        private CancellationToken _token;
        
        public event Action OnPlayerLost;

        public event Action OnMergeComplete;

        public MergeController(PathDrawController pathDrawController, BallBoard ballBoard,
            BallsPreviewController previewController,
            BallsAnimationController ballsAnimationController, 
            GameplayData gameplayData, ComboCalculator comboCalculator, IAudioService audioService)
        {
            _pathDrawController = pathDrawController;
            _board = ballBoard;
            _previewController = previewController;
            _animationController = ballsAnimationController;
            _gameplayData = gameplayData;
            _comboCalculator = comboCalculator;
            _audioService = audioService;
            
            _pathDrawController.OnMerged += ProcessMerge;
        }

        public void SetToken(CancellationToken token) => _token = token;

        private void ProcessMerge(List<GameBall> path)
        {
            PlayMergeAnimation(path).Forget();
        }
        
        private async UniTaskVoid PlayMergeAnimation(List<GameBall> path)
        {
            int increase = _comboCalculator.CalculateCombo(path);
            
            if(increase == 0)
                return;

            _token.ThrowIfCancellationRequested();
            
            _audioService.PlaySound(ConstAudio.MatchSound);
            
            UpdateGameplayData(path);

            _pathDrawController.SetEnabled(false);

            var matchedBalls = path.GetRange(0, path.Count - 1);
            
            await _animationController.PlayMatchAnim(matchedBalls);
            _token.ThrowIfCancellationRequested();

            _board.DropBalls(matchedBalls);

            _board.UpdateValue(path[^1], increase);
            
            await _animationController.PlayDropBallsAnimation();
            _token.ThrowIfCancellationRequested();

            var newBalls = _previewController.CreateMissingBallsToColumns();
            
            await _animationController.PlayAddNewBallsAnimation(newBalls);
            _token.ThrowIfCancellationRequested();

            await _animationController.PlayAppearAnimation(_previewController.CreateRowPreview());
            _token.ThrowIfCancellationRequested();

            _pathDrawController.SetEnabled(true);

            if (!_board.MatchesPossible())
                OnPlayerLost?.Invoke();
            
            OnMergeComplete?.Invoke();
        }

        private void UpdateGameplayData(List<GameBall> path)
        {
            int value = path[0].Value;
            int count = path.Count;
            
            if(value > _gameplayData.HighestValue)
                _gameplayData.HighestValue = value;
            
            if(count > _gameplayData.HighestMatchCount)
                _gameplayData.HighestMatchCount = value;
        }
    }
}