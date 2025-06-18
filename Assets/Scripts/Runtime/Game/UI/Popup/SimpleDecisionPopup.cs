using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Core.UI.Popup;
using Runtime.Game.UI.Popup.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Popup
{
    public class SimpleDecisionPopup : BasePopup
    {
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private TextMeshProUGUI _message;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SimpleDecisionPopupData simpleDecisionPopupData = data as SimpleDecisionPopupData;

            _message.text = simpleDecisionPopupData.Message;
            _confirmButton.onClick.AddListener(() =>
            {
                simpleDecisionPopupData?.PressOkEvent?.Invoke();
                Hide();
            });

            _cancelButton.onClick.AddListener(Hide);

            return base.Show(data, cancellationToken);
        }
    }
}