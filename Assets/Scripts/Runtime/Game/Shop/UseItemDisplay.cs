using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseItemDisplay : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _priceText;
    
    
    [SerializeField] private int _price;

    public event Action<int> OnPurchasePressed;

    private void Awake()
    {
        _priceText.text = _price.ToString();
        _buyButton.onClick.AddListener(() => OnPurchasePressed?.Invoke(_price));
    }
}
