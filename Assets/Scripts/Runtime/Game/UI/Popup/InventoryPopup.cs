using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class InventoryPopup : BasePopup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private RectTransform _backgroundParent;
        [SerializeField] private RectTransform _fieldParent;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;

        private RectTransform _prevSection;
        
        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _closeButton.onClick.AddListener(DestroyPopup);
            
            _leftButton.onClick.AddListener(SwitchSections);
            _rightButton.onClick.AddListener(SwitchSections);
            
            _prevSection = _backgroundParent;
            
            return base.Show(data, cancellationToken);
        }

        private void SwitchSections()
        {
            if (_prevSection == _backgroundParent)
            {
                _backgroundParent.gameObject.SetActive(false);
                _fieldParent.gameObject.SetActive(true);
                _prevSection = _fieldParent;
            }
            else
            {
                _backgroundParent.gameObject.SetActive(true);
                _fieldParent.gameObject.SetActive(false);
                _prevSection = _backgroundParent;
            }
        }

        public void SetBackgrounds(List<InventoryItemDisplay> backgrounds)
        {
            foreach (var item in backgrounds)
            {
                item.transform.SetParent(_backgroundParent, false);
            }
        }
        
        public void SetFields(List<InventoryItemDisplay> fields)
        {
            foreach (var item in fields)
            {
                item.transform.SetParent(_fieldParent, false);
            }
        }
    }
}