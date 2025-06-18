using System;
using System.Collections.Generic;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Services.UserData
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new List<GameSessionData>();
        public SettingsData SettingsData = new SettingsData();
        public GameData GameData = new GameData();
        public UserProfileData UserProfileData = new UserProfileData();
        public UserProgressData UserProgressData = new UserProgressData();
        public UserInventoryData UserInventoryData = new UserInventoryData();
        public UserLoginData UserLoginData = new UserLoginData();
    }
}