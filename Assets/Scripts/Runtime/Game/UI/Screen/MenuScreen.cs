using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class MenuScreen : UiScreen
    {
        private const float AnimTime = 0.25f;
        private const float OpenPosY = 50;
        private const float ClosePosY = 0;
            
        [SerializeField] private MenuButton _shopButton;
        [SerializeField] private MenuButton _homeButton;
        [SerializeField] private MenuButton _leadButton;
        
        private MenuButton _prevMenu;
        
        public void Initialize()
        {
            _prevMenu = _homeButton;
            
            _shopButton.Button.onClick.AddListener(() => SwitchMenu(_shopButton));
            _homeButton.Button.onClick.AddListener(() => SwitchMenu(_homeButton));
            _leadButton.Button.onClick.AddListener(() => SwitchMenu(_leadButton));
        }

        private void SwitchMenu(MenuButton nextMenu)
        {
            if (_prevMenu != nextMenu)
            {
                CloseMenu(_prevMenu);
            }
            
            _prevMenu = nextMenu;
            OpenMenu(nextMenu);
        }

        private void CloseMenu(MenuButton menu)
        {
            menu.Canvas.interactable = false;
            menu.Canvas.blocksRaycasts = false;
            AnimateMenuButton(menu, ClosePosY, OpenPosY, 0);
        }

        private void OpenMenu(MenuButton menu)
        {
            menu.Canvas.interactable = true;
            menu.Canvas.blocksRaycasts = true;
            AnimateMenuButton(menu, OpenPosY, ClosePosY, 1);
        }

        private void AnimateMenuButton(MenuButton menu, float buttonY, float textY, float targetAlpha)
        {
            menu.Canvas.DOFade(targetAlpha, AnimTime).SetLink(gameObject);
            menu.Text.rectTransform.DOAnchorPosY(textY, AnimTime).SetLink(gameObject);
            menu.Button.GetComponent<RectTransform>().DOAnchorPosY(buttonY, AnimTime).SetLink(gameObject);
        }
    }

    [Serializable]
    public class MenuButton
    {
        public Button Button;
        public TextMeshProUGUI Text;
        public CanvasGroup Canvas;
    }
}