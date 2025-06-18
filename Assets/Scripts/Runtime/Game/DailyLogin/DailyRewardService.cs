using System;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.DailyLogin
{
    public class DailyRewardService
    {
        private readonly UserDataService _userDataService;

        public DailyRewardService(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }
        
        public bool RewardAvailable()
        {
            var lastLoginDateString  = GetSavedTime();
            if (lastLoginDateString == String.Empty)
                return true;
         
            var lastLoginDate = Convert.ToDateTime(lastLoginDateString);
            return DateTime.Now.Date > lastLoginDate.Date;
        }

        public int GetLoginStreak() => GetLoginData().LoginStreak;
        
        public void RecordClaimDate()
        {
            var loginData = GetLoginData();
            loginData.LastLoginDate = DateTime.Now.ToString();
            loginData.LoginStreak++;
            _userDataService.SaveUserData();
        }

        private string GetSavedTime() => GetLoginData().LastLoginDate;
        private UserLoginData GetLoginData() => _userDataService.GetUserData().UserLoginData;
    }
}