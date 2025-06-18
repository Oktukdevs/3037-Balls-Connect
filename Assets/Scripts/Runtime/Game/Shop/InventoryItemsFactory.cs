using System;
using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.SettingsProvider;
using Runtime.Game.Services.UserData;
using UnityEngine;
using Zenject;

public class InventoryItemsFactory : IInitializable
{
    public const string SelectedStatus = "Selected";
    public const string NotSelectedStatus = "Select";
    
    private readonly IAssetProvider _assetProvider;
    private readonly GameObjectFactory _gameObjectFactory;
    private readonly UserDataService _userDataService;
    private readonly ISettingProvider _settingProvider;
    
    private GameObject _backgroundPrefab;
    private GameObject _fieldPrefab;

    private Sprite _selectedSprite;
    private Sprite _notSelectedSprite;
    
    public Sprite SelectedSprite => _selectedSprite;
    public Sprite NotSelectedSprite => _notSelectedSprite;

    public InventoryItemsFactory(IAssetProvider assetProvider, UserDataService userDataService,
        ISettingProvider settingProvider, GameObjectFactory gameObjectFactory)
    {
        _assetProvider = assetProvider;
        _userDataService = userDataService;
        _settingProvider = settingProvider;
        _gameObjectFactory = gameObjectFactory;
    }
    
    public async void Initialize()
    {
        _backgroundPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.InventoryDisplayItem);
        _fieldPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.FieldInventoryDisplayItem);
    }

    public List<InventoryItemDisplay> GetInventoryBackgrounds()
    {
        _selectedSprite = _settingProvider.Get<ShopConfig>().SelectedItemSprite;
        _notSelectedSprite = _settingProvider.Get<ShopConfig>().NotSelectedItemSprite;
        
        List<InventoryItemDisplay> result = new ();

        var usedIds = _userDataService.GetUserData().UserInventoryData.PurchasedBackgroundIds;

        foreach (var id in usedIds)
        {
            var display = _gameObjectFactory.Create<InventoryItemDisplay>(_backgroundPrefab);
            display.Initialize(id, GetStatusSprite(id, IsBackgroundSelected), GetStatusText(id, IsBackgroundSelected), GetBackgroundItemSprite(id));
            result.Add(display);
        }
        
        return result;
    }
    
    public List<InventoryItemDisplay> GetInventoryFields()
    {
        List<InventoryItemDisplay> result = new ();

        var usedIds = _userDataService.GetUserData().UserInventoryData.PurchasedFieldIds;

        foreach (var id in usedIds)
        {
            var display = _gameObjectFactory.Create<InventoryItemDisplay>(_fieldPrefab);
            display.Initialize(id, GetStatusSprite(id, IsFieldSelected), GetStatusText(id, IsFieldSelected), GetFieldItemSprite(id), GetFieldHeaderSprite(id));
            result.Add(display);
        }
        
        return result;
    }

    private Sprite GetBackgroundItemSprite(int id) => _settingProvider.Get<ShopConfig>().BackgroundItems[id].Sprite;
    private Sprite GetFieldItemSprite(int id) => _settingProvider.Get<ShopConfig>().FieldItems[id].Sprite;
    private Sprite GetFieldHeaderSprite(int id) => _settingProvider.Get<ShopConfig>().FieldItems[id].HeaderSprite;

    private string GetStatusText(int id, Func<int, bool> isSelected)
    {
        if(isSelected(id))
            return SelectedStatus;
        
        return NotSelectedStatus;
    }

    private Sprite GetStatusSprite(int id, Func<int, bool> isSelected)
    {
        if (isSelected(id))
            return SelectedSprite;
        
        return NotSelectedSprite;
    }

    private bool IsBackgroundSelected(int id) => _userDataService.GetUserData().UserInventoryData.UsedBackgroundId == id;
    private bool IsFieldSelected(int id) => _userDataService.GetUserData().UserInventoryData.UsedFieldId == id;
}
