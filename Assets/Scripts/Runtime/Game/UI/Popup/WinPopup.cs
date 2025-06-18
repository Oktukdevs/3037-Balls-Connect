using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Game.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class WinPopup : BasePopup
    {
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _nextButton;

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _rewardText;
        
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _bestTimeText;

        [SerializeField] private Image[] _stars;
        [SerializeField] private Sprite _activeStar;

        public event Action OnHomePressed;
        public event Action OnNextPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _homeButton.onClick.AddListener(() => OnHomePressed?.Invoke());
            _nextButton.onClick.AddListener(() => OnNextPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetData(int level, int reward, float time, float bestTime, int stars)
        {
            _levelText.text = $"Level: {level + 1}";
            _rewardText.text = reward.ToString();
            
            _timeText.text = Tools.FormatTime(time);
            _bestTimeText.text = Tools.FormatTime(bestTime);
            
            for(int i = 0; i < stars; i++)
                _stars[i].sprite = _activeStar;
        }
    }
}