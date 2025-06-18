using System;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class GameData
    {
        public int SessionNumber = 0;
        public bool IsAdb = false;
    }
}