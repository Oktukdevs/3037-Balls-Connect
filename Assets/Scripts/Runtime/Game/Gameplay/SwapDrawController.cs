using System;
using UnityEngine;
using Zenject;

namespace Runtime.Game.Gameplay
{
    public class SwapDrawController : ITickable
    {
        private const float SelectRadius = 3;
    
        private readonly UserInputService _userInputService;
        private readonly BallBoard _board;
        private readonly LineDrawer _lineDrawer;
        
        public event Action<GameBall, GameBall> OnSwapped;
        
        private bool _enabled = false;
        
        private GameBall _firstBall;
        private GameBall _secondBall;

        public SwapDrawController(UserInputService userInputService, BallBoard board, LineDrawer lineDrawer)
        {
            _userInputService = userInputService;
            _board = board;
            _lineDrawer = lineDrawer;
        }
        
        public void Tick()
        {
            if(!_enabled)
                return;

            if (!_userInputService.IsTouching)
            {
                if (_firstBall && _secondBall)
                {
                    OnSwapped?.Invoke(_firstBall, _secondBall);
                    _firstBall = null;
                    _secondBall = null;
                }
                
                _lineDrawer.ClearLine();
                return;
            }
            
            Vector2 touchPos = _userInputService.GetTouchWorldPos();

            if (!_firstBall)
            {
                var foundBall = FindClosestBall(touchPos);
                if (foundBall)
                {
                    _firstBall = foundBall;
                    _lineDrawer.AddPathPoint(1, _firstBall.transform.position);
                }
                return;
            }
            

            var otherBall = FindClosestBall(touchPos);
            
            if (otherBall && otherBall != _firstBall)
            {
                _secondBall = otherBall;
                _lineDrawer.AddPathPoint(2, _secondBall.transform.position);
            }
            else
                _lineDrawer.AddPathPoint(2, touchPos);
        }

        public void SetEnabled(bool enabled) => _enabled = enabled;

        public void Reset()
        {
            _enabled = false;
            
            _firstBall = null;
            _secondBall = null;
            
            _lineDrawer.ClearLine();
        }

        private GameBall FindClosestBall(Vector2 lastPos)
        {
            for (int i = 0; i < BallBoard.FieldSize; i++)
            {
                for (int j = 0; j < BallBoard.FieldSize; j++)
                {
                    var ball = _board.Board[i, j].GameBall;
                
                    if(!ball)
                        continue;
                
                    var pos = ball.transform.position;
                
                    if (Vector2.Distance(lastPos, pos) <= SelectRadius)
                        return ball;
                }
            }

            return null;
        }
    }
}