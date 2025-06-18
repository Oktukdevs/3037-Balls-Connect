using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Audio;
using Runtime.Core.UI.Data;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Runtime.Core.UI.Popup
{
    public class BasePopup : MonoBehaviour
    {
        private const float InAnimTime = 0.25f;
        private const string OpenPopupSound = "OpenPopup";
        private const string ClosePopupSound = "ClosePopupSound";

        private bool _isSoundEnable = true;

        [SerializeField] protected string _id;
        [SerializeField] private RectTransform _contentParent;
        
        protected IAudioService AudioService;

        public UnityEvent ShowEvent;
        public UnityEvent HideEvent;
        public UnityEvent HideImmediatelyEvent;

        public event Action DestroyPopupEvent;

        public string Id => _id;

        [Inject]
        public void Construct(IAudioService audioService)
        {
            AudioService = audioService;
        }

        public virtual UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            PlayAnim();
            TryPlaySound(OpenPopupSound);
            ShowEvent?.Invoke();
            return UniTask.CompletedTask;
        }

        public virtual void Hide()
        {
            HideEvent?.Invoke();
        }
        
        private void PlayAnim()
        {
            if(_contentParent == null)
                return;
            
            _contentParent.localScale = Vector3.zero;
            _contentParent.DOScale(Vector3.one, InAnimTime).SetEase(Ease.InCubic).SetLink(gameObject).SetUpdate(true);
        }

        public virtual void HideImmediately()
        {
            HideImmediatelyEvent?.Invoke();
        }

        public virtual void DestroyPopup()
        {
            DestroyPopupEvent?.Invoke();
            TryPlaySound(ClosePopupSound);
            Destroy(gameObject);
        }

        public void EnableSound(bool enable)
        {
            _isSoundEnable = enable;
        }

        protected void TryPlaySound(string soundName)
        {
            if(_isSoundEnable)
                AudioService.PlaySound(soundName);
        }
    }
}