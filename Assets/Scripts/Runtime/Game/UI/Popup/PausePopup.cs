using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class PausePopup : BasePopup
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _leaveButton;
        
        public event Action OnContinueButtonPressed;
        public event Action OnLeaveButtonPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _continueButton.onClick.AddListener(() => OnContinueButtonPressed?.Invoke());
            _leaveButton.onClick.AddListener(() => OnLeaveButtonPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }
    }
}