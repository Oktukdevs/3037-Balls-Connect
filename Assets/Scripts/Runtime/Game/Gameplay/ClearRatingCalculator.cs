using Runtime.Core.Infrastructure.SettingsProvider;

namespace Runtime.Game.Gameplay
{
    public class ClearRatingCalculator
    {
        private readonly GameplayData _gameplayData;
        private readonly ISettingProvider _settingProvider;

        public ClearRatingCalculator(GameplayData gameplayData, ISettingProvider settingProvider)
        {
            _gameplayData = gameplayData;
            _settingProvider = settingProvider;
        }
        
        public int GetStarsAmount()
        {
            var levelConfig = GetLevelConfig();
            float timeTotal = levelConfig.TimeTotal;
            float timeLeft = _gameplayData.TimeLeft;

            return timeLeft > 0.66 * timeTotal ? 3 :
                timeLeft > 0.33 * timeTotal ? 2 : 1;
        }
        
        private LevelConfig GetLevelConfig()
        {
            var gameConfig = _settingProvider.Get<GameConfig>();
            var levelConfig = gameConfig.Levels[_gameplayData.GameLevelId];
            return levelConfig;
        }
    }
}