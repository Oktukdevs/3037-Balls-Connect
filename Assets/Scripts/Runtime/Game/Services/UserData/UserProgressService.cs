using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Gameplay;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Services.UserData
{
    public class UserProgressService
    {
        private readonly UserDataService _userDataService;
        private readonly ISettingProvider _settingProvider;

        public UserProgressService(UserDataService userDataService, ISettingProvider settingProvider)
        {
            _userDataService = userDataService;
            _settingProvider = settingProvider;
        }

        public void RecordScore(int score)
        {
            int prevScore = GetBestScore();
            
            if(score > prevScore)
                GetUserProgressData().HighestScore = score;
        }

        public bool NextLevelExists(int levelId)
        {
            return levelId + 1 < _settingProvider.Get<GameConfig>().Levels.Count;
        }
        
        public int GetLastUnlockedLevel() => GetUserProgressData().ClearData.Count;

        public float GetBestTime(int levelId)
        {
            var progressData = GetUserProgressData();
            return progressData.ClearData[levelId].BestTime;
        }
        
        public void RecordLevelClear(int levelId, int stars, float time)
        {
            var progressData = GetUserProgressData();
            if (progressData.ClearData.Count == levelId)
            {
                progressData.ClearData.Add(new()
                {
                    Stars = stars,
                    BestTime = time
                });
                return;
            }
            
            var clearData = progressData.ClearData[levelId];
            
            if (stars > clearData.Stars)
                progressData.ClearData[levelId].Stars = stars;
            
            if(time > clearData.BestTime)
                progressData.ClearData[levelId].BestTime = time;
        }

        public int GetLevelClearStars(int levelId)
        {
            if (levelId >= GetLastUnlockedLevel())
                return 0;

            return GetUserProgressData().ClearData[levelId].Stars;
        }

        public int GetBestScore() => GetUserProgressData().HighestScore;
        
        private UserProgressData GetUserProgressData() => _userDataService.GetUserData().UserProgressData;
    }
}