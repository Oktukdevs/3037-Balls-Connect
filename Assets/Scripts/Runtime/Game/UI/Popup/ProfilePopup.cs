using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class ProfilePopup : BasePopup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _avatarButton;
        [SerializeField] private Image _avatarImage;
        [SerializeField] private TMP_InputField _nameField;

        public event Action OnSavePressed;
        public event Action OnAvatarSelectPressed;
        public event Action<string> OnNameFieldChanged;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _closeButton.onClick.AddListener(DestroyPopup);
            _saveButton.onClick.AddListener(() => OnSavePressed?.Invoke());
            _avatarButton.onClick.AddListener(() => OnAvatarSelectPressed?.Invoke());
            _nameField.onEndEdit.AddListener((value) => OnNameFieldChanged?.Invoke(value));
            
            return base.Show(data, cancellationToken);
        }
        
        public void SetName(string name) => _nameField.text = name;
        public void SetAvatar(Sprite avatar)
        {
            if(avatar != null)
                _avatarImage.sprite = avatar;
        }
    }
}