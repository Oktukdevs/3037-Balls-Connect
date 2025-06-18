using System.Collections.Generic;

namespace Runtime.Game.Gameplay
{
    public class ScoreCalculator
    {
        private readonly GameplayData _gameplayData;
        private readonly PathDrawController _pathDrawController;
        private readonly ComboCalculator _comboCalculator;

        public ScoreCalculator(GameplayData gameplayData, PathDrawController pathDrawController, ComboCalculator comboCalculator)
        {
            _gameplayData = gameplayData;
            _pathDrawController = pathDrawController;
            _comboCalculator = comboCalculator;
            
            _pathDrawController.OnMerged += CalculateScore;
        }

        private void CalculateScore(List<GameBall> matchedBalls)
        {
            int combo = _comboCalculator.CalculateCombo(matchedBalls);
            
            if(combo == 0)
                return;
            
            int value = matchedBalls[0].Value;
            int count = matchedBalls.Count;
            
            int scoreGained = count * value * combo;

            _gameplayData.Score += scoreGained;
        }
    }
}