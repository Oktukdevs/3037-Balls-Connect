using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserProgressData
    {
        public int HighestScore;
        public List<ClearData> ClearData = new();
    }

    [Serializable]
    public class ClearData
    {
        public int Stars;
        public float BestTime;
    }
}