using System;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserLoginData
    {
        public string LastLoginDate = string.Empty;
        public int LoginStreak = 0;
    }
}