using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Controllers;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Core.UI.Popup;
using Runtime.Game.DailyLogin;
using Runtime.Game.Services.UI;
using Runtime.Game.Shop;
using UnityEngine;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public class DailyRewardController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly DailyRewardService _dailyRewardService;
        private readonly ISettingProvider _settingProvider;
        private readonly UserInventoryService _userInventoryService;

        public DailyRewardController(IUiService uiService, DailyRewardService dailyRewardService, 
            ISettingProvider settingProvider, UserInventoryService userInventoryService)
        {
            _uiService = uiService;
            _dailyRewardService = dailyRewardService;
            _settingProvider = settingProvider;
            _userInventoryService = userInventoryService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);

            DailyRewardPopup popup = _uiService.GetPopup<DailyRewardPopup>(ConstPopups.DailyRewardPopup);
            
            popup.Show(null, cancellationToken).Forget();
            
            popup.SetData(GetRewards(), _dailyRewardService.RewardAvailable());
            
            popup.OnClaimPressed += () =>
            {
                var config = _settingProvider.Get<DailyRewardsConfig>();
                var streak = _dailyRewardService.GetLoginStreak();

                if (streak >= 0 && streak < config.DailyRewards.Count)
                {
                    var rewardData = config.DailyRewards[streak];
                    if(rewardData.Type == DailyRewardType.Coins)
                        _userInventoryService.AddBalance(rewardData.Amount);
                    if(rewardData.Type == DailyRewardType.Refresh)
                        _userInventoryService.AddRandomize(rewardData.Amount);
                    if(rewardData.Type == DailyRewardType.Swap)
                        _userInventoryService.AddSwaps(rewardData.Amount);
                    
                    popup.UpdateStatus(streak, config.ClaimedSprite);
                    _dailyRewardService.RecordClaimDate();
                }
            };

            CurrentState = ControllerState.Complete;
            return UniTask.CompletedTask;
        }

        private List<RewardDisplayData> GetRewards()
        {
            var config = _settingProvider.Get<DailyRewardsConfig>();
            
            List<RewardDisplayData> rewards = new List<RewardDisplayData>();

            for (int i = 0; i < config.DailyRewards.Count; i++)
            {
                var rewardData = config.DailyRewards[i];

                RewardDisplayData data = new();
                data.DailyRewardData = rewardData;
                data.StatusSprite = GetRewardSprite(i);
                
                rewards.Add(data);
            }
            
            return rewards;
        }

        private Sprite GetRewardSprite(int day)
        {
            int streak = _dailyRewardService.GetLoginStreak();
            var config = _settingProvider.Get<DailyRewardsConfig>();

            if (streak > day)
                return config.ClaimedSprite;

            if (streak < day)
                return config.LockedSprite;

            if (streak == day && _dailyRewardService.RewardAvailable())
                return config.UnclaimedSprite;

            return config.LockedSprite;
        }
    }

    public class RewardDisplayData
    {
        public DailyRewardData DailyRewardData;
        public Sprite StatusSprite;
    }
}