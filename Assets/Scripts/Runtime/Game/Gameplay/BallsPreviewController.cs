using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class BallsPreviewController
    {
        public const float PreviewYPos = 24;
        
        private readonly BallsPool _pool;
        private readonly BallBoard _board;
        private readonly BallsTransformController _transformController;

        private List<GameBall> _rowBalls = new()
        {
            null,
            null,
            null,
            null,
            null,
        };
        
        public BallsPreviewController(BallsPool ballsPool, BallBoard board, BallsTransformController ballsTransformController)
        {
            _pool = ballsPool;
            _board = board;
            _transformController = ballsTransformController;
        }

        public List<GameBall> CreateRowPreview()
        {
            int highestValue = FindHighestValueBall();

            List<GameBall> newBalls = new();
            
            for (int i = 0; i < BallBoard.FieldSize; i++)
            {
                if (!_rowBalls[i])
                {
                    GameBall ball = _pool.GetBall(Random.Range(1, highestValue));
                    
                    ball.transform.position =
                        new Vector3(_transformController.GetWorldPositionX(i),
                            PreviewYPos + BallsTransformController.OffsetDistance,
                            0);

                    ball.transform.localScale = Vector3.one * 0.6f;
                    
                    _rowBalls[i] = ball;
                    newBalls.Add(ball);
                }
            }

            return newBalls;
        }

        public void Cleanup()
        {
            for(int i = 0; i < _rowBalls.Count; i++)
                _rowBalls[i] = null;
        }
        
        public List<List<GameBall>> CreateMissingBallsToColumns()
        {
            List<List<GameBall>> result = new();

            var board = _board.Board;

            int highestValue = FindHighestValueBall();
            
            for (int col = 0; col < BallBoard.FieldSize; col++)
            {
                List<GameBall> newBalls = new();
                
                for (int row = 0; row < BallBoard.FieldSize; row++)
                {
                    if (board[row, col].HasBall())
                        continue;

                    GameBall ball = null;
                    
                    if (_rowBalls[col] != null)
                    {
                        ball = _rowBalls[col];
                        _rowBalls[col] = null;
                    }
                    else
                        ball = _pool.GetBall(Random.Range(1, highestValue + 1));
                    
                    ball.transform.position =
                        new Vector3(_transformController.GetWorldPositionX(col),
                            PreviewYPos + BallsTransformController.OffsetDistance * newBalls.Count,
                            0);
                    
                    newBalls.Add(ball);
                    _board.AssignNewBall(ball, new Vector2Int(col, row));
                }

                newBalls.Reverse();
                result.Add(newBalls);
            }
            
            return result;
        }

        private int FindHighestValueBall()
        {
            int highestValue = 0;
            
            for (int row = 0; row < BallBoard.FieldSize; row++)
            {
                for (int col = 0; col < BallBoard.FieldSize; col++)
                {
                    if (_board.Board[row, col].HasBall())
                    {
                        if(_board.Board[row, col].Value > highestValue)
                            highestValue = _board.Board[row, col].Value;
                    }
                }
            }
            
            return highestValue;
        }
    }
}