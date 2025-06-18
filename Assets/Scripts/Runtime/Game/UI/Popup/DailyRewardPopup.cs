using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Game.GameStates.Game.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class DailyRewardPopup : BasePopup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _claimButton;
        [SerializeField] private List<DailyRewardDisplay> _dailyRewardDisplay;
        
        public event Action OnClaimPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _closeButton.onClick.AddListener(DestroyPopup);
            _claimButton.onClick.AddListener(() => OnClaimPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetData(List<RewardDisplayData> data, bool canClaim)
        {
            for (int i = 0; i < data.Count; i++) 
                _dailyRewardDisplay[i].Initialize(data[i].StatusSprite, i + 1, data[i].DailyRewardData);
            _claimButton.interactable = canClaim;
        }

        public void UpdateStatus(int id, Sprite claimedSprite)
        {
            _dailyRewardDisplay[id].UpdateStatus(claimedSprite);
            _claimButton.interactable = false;
        }
    }
}