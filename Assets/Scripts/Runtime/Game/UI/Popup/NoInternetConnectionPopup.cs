using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Core.UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Popup
{
    public class NoInternetConnectionPopup : BasePopup
    {
        [SerializeField] private Button _okButton;

        private UniTaskCompletionSource _completionSource;

        public override async UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _completionSource = new UniTaskCompletionSource();
            _okButton.onClick.AddListener(Hide);

            await _completionSource.Task;

            DestroyPopup();
        }

        public override void Hide()
        {
            _completionSource?.TrySetResult();
        }
    }
}