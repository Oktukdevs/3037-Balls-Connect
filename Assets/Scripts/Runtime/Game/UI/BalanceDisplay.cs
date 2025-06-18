using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Shop;
using TMPro;
using UnityEngine;
using Zenject;

public class BalanceDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;
    
    private UserInventoryService _userInventoryService;

    [Inject]
    private void Construct(UserInventoryService userInventoryService)
    {
        _userInventoryService = userInventoryService;
        
        _userInventoryService.OnBalanceChanged += UpdateBalance;
        _balanceText.text = _userInventoryService.GetBalance().ToString();
    }

    private void OnDestroy()
    {
        _userInventoryService.OnBalanceChanged -= UpdateBalance;
    }

    private void UpdateBalance(int balance) => _balanceText.text = balance.ToString();
}
