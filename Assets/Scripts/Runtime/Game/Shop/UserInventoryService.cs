using System;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Shop
{
    public class UserInventoryService
    {
        private readonly UserDataService _userDataService;

        public event Action<int> OnBalanceChanged;
        public event Action<int> OnRandomizeChanged;
        public event Action<int> OnSwapChanged;
        
        public UserInventoryService(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public void SetUsedBackground(int id) => GetData().UsedBackgroundId = id;
        public void SetUsedField(int id) => GetData().UsedFieldId = id;
        
        public void AddBackground(int id)
        {
            GetData().PurchasedBackgroundIds.Add(id);
            SetUsedBackground(id);
        }
        
        public void AddField(int id)
        {
            GetData().PurchasedFieldIds.Add(id);
            SetUsedField(id);
        }
        
        public void AddBalance(int amount)
        {
            var balance = GetBalance();
            balance += amount;
            GetData().Balance = balance;
            OnBalanceChanged?.Invoke(balance);
        }

        public int GetBalance() => GetData().Balance;

        public void AddRandomize(int amount)
        {
            var balance = GetRandomizes();
            balance += amount;
            GetData().Randomizes = balance;
            OnRandomizeChanged?.Invoke(balance);
        }

        public int GetRandomizes() => GetData().Randomizes;
        
        public void AddSwaps(int amount)
        {
            var balance = GetSwaps();
            balance += amount;
            GetData().Swaps = balance;
            OnSwapChanged?.Invoke(balance);
        }

        public int GetSwaps() => GetData().Swaps;

        public int GetSelectedBackgroundId() => GetData().UsedBackgroundId;
        public int GetSelectedFieldId() => GetData().UsedFieldId;

        private UserInventoryData GetData() => _userDataService.GetUserData().UserInventoryData;
    }
}