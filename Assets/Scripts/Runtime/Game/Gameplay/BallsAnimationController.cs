using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class BallsAnimationController : ICancellable
    {
        private const float DropDuration = 0.25f;
        private const float MatchDuration = 0.4f;
        private const float AppearDuration = 0.33f;
        private const float MoveDuration = 0.33f;

        private readonly BallBoard _board;
        private readonly BallsTransformController _ballsTransformController;

        private CancellationToken _token;

        public BallsAnimationController(BallBoard board, BallsTransformController ballsTransformController)
        {
            _board = board;
            _ballsTransformController = ballsTransformController;
        }

        public void SetToken(CancellationToken token)
        {
            _token = token;
        }

        public async UniTask AnimateBoardToCurrentPositions(BallBoard board)
        {
            List<UniTask> tasks = new();
            var slots = board.Board;

            for (var row = 0; row < BallBoard.FieldSize; row++)
            for (var col = 0; col < BallBoard.FieldSize; col++)
            {
                _token.ThrowIfCancellationRequested();

                var slot = slots[row, col];
                if (!slot.HasBall()) continue;

                var ball = slot.GameBall;
                var targetPos = _ballsTransformController.GetWorldPosition(col, row);

                if (Vector3.Distance(ball.transform.position, targetPos) > 0.01f)
                {
                    var tween = ball.transform.DOMove(targetPos, MoveDuration)
                        .SetEase(Ease.InOutQuad)
                        .SetLink(ball.gameObject);

                    tasks.Add(AwaitTweenWithCancellation(tween, _token));
                }
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask AnimateSwap(GameBall ball1, GameBall ball2)
        {
            _token.ThrowIfCancellationRequested();

            var pos1 = ball1.transform.position;
            var pos2 = ball2.transform.position;

            var t1 = ball1.transform.DOMove(pos2, 0.2f).SetEase(Ease.InOutQuad).SetLink(ball1.gameObject);
            var t2 = ball2.transform.DOMove(pos1, 0.2f).SetEase(Ease.InOutQuad).SetLink(ball2.gameObject);

            await UniTask.WhenAll(
                AwaitTweenWithCancellation(t1, _token),
                AwaitTweenWithCancellation(t2, _token)
            );
        }

        public async UniTask PlayMatchAnim(List<GameBall> newBalls)
        {
            List<UniTask> animations = new();

            foreach (var ball in newBalls)
            {
                _token.ThrowIfCancellationRequested();

                var tween = ball.transform.DOScale(Vector3.zero, MatchDuration).SetLink(ball.gameObject);
                animations.Add(AwaitTweenWithCancellation(tween, _token));
            }

            await UniTask.WhenAll(animations);
        }

        public async UniTask PlayAppearAnimation(List<GameBall> newBalls)
        {
            List<UniTask> animations = new();

            foreach (var ball in newBalls)
            {
                _token.ThrowIfCancellationRequested();

                var targetPos = new Vector2(ball.transform.position.x, BallsPreviewController.PreviewYPos);
                var moveTween = ball.transform.DOMove(targetPos, AppearDuration).SetLink(ball.gameObject);
                animations.Add(AwaitTweenWithCancellation(moveTween, _token));
            }

            await UniTask.WhenAll(animations);
        }

        public async UniTask PlayDropBallsAnimation()
        {
            List<UniTask> animations = new();

            for (var y = 0; y < BallBoard.FieldSize; y++)
            for (var x = 0; x < BallBoard.FieldSize; x++)
            {
                _token.ThrowIfCancellationRequested();

                var slot = _board.Board[y, x];
                if (!slot.HasBall()) continue;

                var ball = slot.GameBall;
                var targetPos = _ballsTransformController.GetWorldPosition(x, y);

                if (Vector3.Distance(ball.transform.position, targetPos) > 0.01f)
                {
                    var tween = ball.transform.DOMove(targetPos, DropDuration)
                        .SetEase(Ease.InQuad)
                        .SetLink(ball.gameObject);

                    animations.Add(AwaitTweenWithCancellation(tween, _token));
                }
            }

            await UniTask.WhenAll(animations);
        }

        public async UniTask PlayAddNewBallsAnimation(List<List<GameBall>> newBalls)
        {
            List<UniTask> animations = new();

            for (var i = 0; i < newBalls.Count; i++) PlayNewBallsDropAnimation(newBalls[i], i, animations);

            await UniTask.WhenAll(animations);
        }

        private void PlayNewBallsDropAnimation(List<GameBall> ballColumn, int column, List<UniTask> animations)
        {
            for (var row = ballColumn.Count - 1; row >= 0; row--)
            {
                _token.ThrowIfCancellationRequested();

                var ball = ballColumn[row];
                var targetPos = _ballsTransformController.GetWorldPosition(column, row, false);

                if (Vector3.Distance(ball.transform.position, targetPos) > 0.01f)
                {
                    var moveTween = ball.transform.DOMove(targetPos, DropDuration)
                        .SetEase(Ease.InQuad)
                        .SetLink(ball.gameObject);
                    
                    var scaleTween = ball.transform.DOScale(Vector3.one, DropDuration)
                        .SetEase(Ease.InQuad)
                        .SetLink(ball.gameObject);

                    animations.Add(AwaitTweenWithCancellation(moveTween, _token));
                    animations.Add(AwaitTweenWithCancellation(scaleTween, _token));
                }
            }
        }

        private async UniTask AwaitTweenWithCancellation(Tween tween, CancellationToken token)
        {
            try
            {
                await UniTask.WaitUntil(() => !tween.IsActive() || tween.IsComplete(), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                tween.Kill();
                throw;
            }
        }
    }
}