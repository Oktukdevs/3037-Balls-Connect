using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class LosePopup : BasePopup
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _replayButton;

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private TextMeshProUGUI _failText;

        public event Action OnHomePressed;
        public event Action OnReplayPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _homeButton.onClick.AddListener(() => OnHomePressed?.Invoke());
            _replayButton.onClick.AddListener(() => OnReplayPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetData(int level, int reward, string fail)
        {
            _levelText.text = $"Level: {level + 1}";
            _rewardText.text = reward.ToString();
            _failText.text = fail;
        }
    }
}