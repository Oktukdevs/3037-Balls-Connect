using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class BallsPool
    {
        private readonly BallsFactory _factory;
        private readonly SpritesProvider _spritesProvider;
        
        private List<GameBall> _pool = new();

        public BallsPool(BallsFactory factory, SpritesProvider spritesProvider)
        {
            _factory = factory;
            _spritesProvider = spritesProvider;
        }
        
        public GameBall GetBall(int value)
        {
            GameBall result = null;
            
            if (_pool.Count > 0)
            {
                result = _pool[^1];
                _pool.RemoveAt(_pool.Count - 1);
            }
            else
            {
                result = _factory.CreateBall();
            }
            
            result.gameObject.SetActive(true);
            result.transform.DOKill();
            result.transform.localScale = Vector3.one;
            result.SetSprite(_spritesProvider.GetBallSprite(value - 1));
            result.SetValue(value);
            
            return result;
        }
        
        public void ReturnBall(GameBall ball)
        {
            ball.transform.DOKill();
            ball.gameObject.SetActive(false);
            _pool.Add(ball);
        }

        public void ReturnAll()
        {
            GameBall[] balls = Object.FindObjectsOfType<GameBall>(false);
            for(int i = 0; i < balls.Length; i++)
                ReturnBall(balls[i]);
        }
    }
}