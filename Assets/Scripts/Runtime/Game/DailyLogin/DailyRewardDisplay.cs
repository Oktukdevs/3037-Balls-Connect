using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardDisplay : MonoBehaviour
{
    [SerializeField] private Image _statusImage;
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private Image _rewardImage;
    [SerializeField] private TextMeshProUGUI _rewardText;
    
    public void Initialize(Sprite sprite, int day, DailyRewardData dailyRewardData)
    {
        _statusImage.sprite = sprite;
        _dayText.text = "Day " + day;

        _rewardImage.sprite = dailyRewardData.Sprite;
        _rewardText.text = dailyRewardData.Amount + "x";
    }
    
    public void UpdateStatus(Sprite sprite) => _statusImage.sprite = sprite;
}
