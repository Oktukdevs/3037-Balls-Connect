using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class ModeSelectPopup : BasePopup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _campaignButton;
        [SerializeField] private Button _endlessButton;

        public event Action OnCampaignPressed;
        public event Action OnEndlessPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _closeButton.onClick.AddListener(DestroyPopup);
            _campaignButton.onClick.AddListener(() => OnCampaignPressed?.Invoke());
            _endlessButton.onClick.AddListener(() => OnEndlessPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }
    }
}