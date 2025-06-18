using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _headerImage;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _priceText;

    public event Action<GameObject, int, int> OnPurchasePressed;
    
    public void Initialize(ShopItemData shopItemData, int itemId)
    {
        _image.sprite = shopItemData.Sprite;
        _priceText.text = shopItemData.Price.ToString();
        
        _buyButton.onClick.AddListener(() => OnPurchasePressed?.Invoke(gameObject, itemId, shopItemData.Price));
    }
    
    public void Initialize(FieldItemData shopItemData, int itemId)
    {
        _image.sprite = shopItemData.Sprite;
        _priceText.text = shopItemData.Price.ToString();
        _headerImage.sprite = shopItemData.HeaderSprite;
        
        _buyButton.onClick.AddListener(() => OnPurchasePressed?.Invoke(gameObject, itemId, shopItemData.Price));
    }
}
