using System.Collections.Generic;
using Runtime.Game.Gameplay;
using UnityEngine;

public class BallBoard
{
    public const int FieldSize = 5;
    
    private const int MaxBallValue = 13;

    private readonly BallsPool _pool;
    private readonly BallsTransformController _ballsTransformController;
    private readonly SpritesProvider _spritesProvider;
    
    private BallSlot[,] _board = new BallSlot[FieldSize, FieldSize];
    public BallSlot[,] Board => _board;
    
    private Dictionary<GameBall, Vector2Int> _ballToPosition;
    
    public Dictionary<GameBall, Vector2Int> BallToPosition => _ballToPosition;

    public BallBoard(BallsPool ballsPool, BallsTransformController ballsTransformController,
        SpritesProvider spritesProvider)
    {
        _pool = ballsPool;
        _ballsTransformController = ballsTransformController;
        _spritesProvider = spritesProvider;
    }
    
    public void Initialize()
    {
        do
        {
            _ballToPosition = new();
            List<int> values = GenerateBalancedValues(FieldSize * FieldSize, 1, 3, 5);
            
            Shuffle(values);

            int i = 0;
            for (int y = 0; y < FieldSize; y++)
            {
                for (int x = 0; x < FieldSize; x++)
                {
                    int value = values[i++];
                    GameBall ball = _pool.GetBall(value);
                    ball.transform.position = _ballsTransformController.GetWorldPosition(x, y);

                    _board[y, x] = new BallSlot();
                    _board[y, x].Initialize(value, ball);

                    _ballToPosition.Add(ball, new Vector2Int(x, y));
                }
            }
        } while (!MatchesPossible());
    }
    
    private List<int> GenerateBalancedValues(int totalCount, int minValue, int maxValue, int minEach)
    {
        List<int> values = new List<int>();
        
        for (int v = minValue; v <= maxValue; v++)
        {
            for (int i = 0; i < minEach; i++)
            {
                values.Add(v);
            }
        }

        while (values.Count < totalCount)
            values.Add(Random.Range(minValue, maxValue + 1));

        return values;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void UpdateValue(GameBall ball, int valueIncrease)
    {
        if(!_ballToPosition.TryGetValue(ball, out var position))
            return;

        var slot = _board[position.y, position.x];
        
        int newValue = Mathf.Clamp(slot.Value + valueIncrease, 0, MaxBallValue);
        slot.SetValue(newValue);
        slot.GameBall.SetSprite(_spritesProvider.GetBallSprite(newValue - 1));
    }

    public void DropBalls(List<GameBall> ballsToRemove)
    {
        RemoveMatchedBalls(ballsToRemove);
        DropFloatingBalls();
    }

    public void AssignNewBall(GameBall ball, Vector2Int position)
    {
        _board[position.y, position.x] = new BallSlot()
        {
            Value = ball.Value,
            GameBall = ball,
        };
        
        _ballToPosition.Add(ball, position);
    }

    public void SwapBalls(GameBall ball1, GameBall ball2)
    {
        Vector2Int pos1 = _ballToPosition[ball1];
        Vector2Int pos2 = _ballToPosition[ball2];

        var cell1 = _board[pos1.y, pos1.x];
        var cell2 = _board[pos2.y, pos2.x];

        cell1.Initialize(ball2.Value, ball2);
        cell2.Initialize(ball1.Value, ball1);

        _ballToPosition[ball1] = pos2;
        _ballToPosition[ball2] = pos1;
    }
    
    public void RandomizeBoard()
    {
        List<GameBall> balls = new();
        _ballToPosition.Clear();

        for (int row = 0; row < FieldSize; row++)
        {
            for (int col = 0; col < FieldSize; col++)
            {
                if (_board[row, col].HasBall())
                {
                    balls.Add(_board[row, col].GameBall);
                    _board[row, col].Clear();
                }
            }
        }

        System.Random rng = new();
        for (int i = balls.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (balls[i], balls[j]) = (balls[j], balls[i]);
        }

        int index = 0;
        for (int row = 0; row < FieldSize; row++)
        {
            for (int col = 0; col < FieldSize; col++)
            {
                if (index < balls.Count)
                {
                    GameBall ball = balls[index];
                    _board[row, col].Initialize(ball.Value, ball);
                    _ballToPosition[ball] = new Vector2Int(col, row);
                    index++;
                }
            }
        }
    }
    
    private void RemoveMatchedBalls(List<GameBall> ballsToRemove)
    {
        foreach (var ball in ballsToRemove)
        {
            if (!_ballToPosition.ContainsKey(ball))
                continue;

            var pos = _ballToPosition[ball];
            _board[pos.y, pos.x].Clear();
            _ballToPosition.Remove(ball);

            _pool.ReturnBall(ball);
        }
    }

    private void DropFloatingBalls()
    {
        for (int x = 0; x < FieldSize; x++)
        {
            int targetY = 0;

            for (int y = 0; y < FieldSize; y++)
            {
                var slot = _board[y, x];
                
                if (!slot.HasBall())
                    continue;

                if (targetY != y)
                {
                    GameBall ball = slot.GameBall;

                    _board[targetY, x].Initialize(ball.Value, ball);
                    _ballToPosition[ball] = new Vector2Int(x, targetY);

                    slot.Clear();
                }

                targetY++;
            }

            for (int y = targetY; y < FieldSize; y++) 
                _board[y, x].Clear();
        }
    }

    public bool MatchesPossible()
    {
        var directions = new (int dx, int dy)[]
        {
            (-1,  0), (1, 0),   
            (0, -1), (0, 1),   
            (-1, -1), (1, 1),   
            (-1, 1),  (1, -1)   
        };

        for (int y = 0; y < FieldSize; y++)
        {
            for (int x = 0; x < FieldSize; x++)
            {
                var currentCell = _board[y, x];
                if (!currentCell.HasBall()) continue;

                int value = currentCell.Value;

                int sameValueNeighbors = 0;

                foreach (var (dx, dy) in directions)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (IsInBounds(nx, ny))
                    {
                        var neighbor = _board[ny, nx];
                        if (neighbor.HasBall() && neighbor.Value == value)
                        {
                            sameValueNeighbors++;
                            if (sameValueNeighbors >= 2)
                                return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private bool IsInBounds(int x, int y) => x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;

    public void DebugBoard()
    {
        for (int y = FieldSize - 1; y >= 0; y--)  // top to bottom
        {
            string row = "";
            for (int x = 0; x < FieldSize; x++)
            {
                row += _board[y, x].HasBall() ? _board[y, x].Value.ToString() : "-";
            }
        
            Debug.Log(row);
        }

        Debug.Log("-----------------------------");
    }
}
