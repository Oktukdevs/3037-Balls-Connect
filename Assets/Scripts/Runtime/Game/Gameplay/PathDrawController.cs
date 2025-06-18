using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PathDrawController : ITickable
{
    private const float SelectRadius = 3;
    
    private readonly UserInputService _userInputService;
    private readonly BallBoard _board;
    private readonly LineDrawer _lineDrawer;

    private List<GameBall> _path = new();
    
    private bool _enabled = false;

    public event Action<List<GameBall>> OnMerged;

    public PathDrawController(UserInputService userInputService, BallBoard board, LineDrawer lineDrawer)
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
            Merge();
            return;
        }

        Vector2 touchPos = _userInputService.GetTouchWorldPos();

        GameBall closest = FindClosestBall(touchPos);

        if (!closest || _path.Contains(closest))
            return;

        if (_path.Count == 0)
            AddBallToPath(closest);
        else
        {
            GameBall last = _path[^1];

            if (last.Value == closest.Value && AreAdjacent(closest, last)) 
                AddBallToPath(closest);
        }
    }

    public void SetEnabled(bool enabled) => _enabled = enabled;

    public void Reset()
    {
        _enabled = false;
        ClearPath();
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

    private bool AreAdjacent(GameBall a, GameBall b)
    {
        var aPos = _board.BallToPosition[a];
        var bPos = _board.BallToPosition[b];

        int dx = Mathf.Abs(aPos.x - bPos.x);
        int dy = Mathf.Abs(aPos.y - bPos.y);

        return dx <= 1 && dy <= 1 && (dx != 0 || dy != 0);
    }

    private void AddBallToPath(GameBall ball)
    {
        _path.Add(ball);
        _lineDrawer.AddPathPoint(_path.Count, ball.transform.position);
    }
    
    private void Merge()
    {
        if(_path.Count == 0)
            return;
        
        OnMerged?.Invoke(new List<GameBall>(_path));
        ClearPath();
    }

    private void ClearPath()
    {
        _path.Clear();
        _lineDrawer.ClearLine();
    }
}
