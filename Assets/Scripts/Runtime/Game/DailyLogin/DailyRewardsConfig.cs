using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyRewardsConfig", menuName = "Config/DailyRewardsConfig")]
public class DailyRewardsConfig : BaseSettings
{
    public List<DailyRewardData> DailyRewards = new List<DailyRewardData>();
    public Sprite ClaimedSprite;
    public Sprite UnclaimedSprite;
    public Sprite LockedSprite;
}
