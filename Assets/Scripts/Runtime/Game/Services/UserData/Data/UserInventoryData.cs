using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserInventoryData
    { 
        public int Balance = 0;
        public int Randomizes = 0;
        public int Swaps = 0;

        public int UsedBackgroundId = 0;
        public List<int> PurchasedBackgroundIds = new List<int>(){0};
        
        public int UsedFieldId = 0;
        public List<int> PurchasedFieldIds = new List<int>(){0};
    }
}