using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class RewardCalculator
    {
        private readonly GameplayData _data;
        private readonly ISettingProvider _settingProvider;
        private readonly ClearRatingCalculator _clearRatingCalculator;

        public RewardCalculator(GameplayData data, ISettingProvider settingProvider, ClearRatingCalculator clearRatingCalculator)
        {
            _data = data;
            _settingProvider = settingProvider;
            _clearRatingCalculator = clearRatingCalculator;
        }
        
        public int CalculateEndlessGameReward()
        {
            int score = _data.Score;
            int coinsForScore = Mathf.CeilToInt(score * 0.1f);
            
            int highestValue = _data.HighestValue;
            int highestMatchCount = _data.HighestMatchCount;
            
            return Mathf.RoundToInt(coinsForScore * (highestValue / 2f + 1) * (highestMatchCount / 2f + 1));
        }

        public int CalculateLoseReward()
        {
            var levelConfig = GetLevelConfig();
            return levelConfig.Reward / 5;
        }
        
        public int CalculateWinReward()
        {
            var levelConfig = GetLevelConfig();
            
            return levelConfig.Reward * _clearRatingCalculator.GetStarsAmount();
        }

        private LevelConfig GetLevelConfig()
        {
            var gameConfig = _settingProvider.Get<GameConfig>();
            var levelConfig = gameConfig.Levels[_data.GameLevelId];
            return levelConfig;
        }
    }
}