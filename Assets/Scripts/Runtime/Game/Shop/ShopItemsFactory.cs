using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.SettingsProvider;
using Runtime.Game.Services.UserData;
using UnityEngine;
using Zenject;

namespace Runtime.Game.Shop
{
    public class ShopItemsFactory : IInitializable
    {
        private readonly UserDataService _userDataService;
        private readonly ISettingProvider _settingProvider;
        private readonly IAssetProvider _assetProvider;
        private readonly GameObjectFactory _factory;

        private GameObject _backgroundShopItemPrefab;
        private GameObject _fieldShopItemPrefab;

        public ShopItemsFactory(UserDataService userDataService, ISettingProvider settingProvider,
            IAssetProvider assetProvider, GameObjectFactory factory)
        {
            _userDataService = userDataService;
            _settingProvider = settingProvider;
            _assetProvider = assetProvider;
            _factory = factory;
        }
        
        public async void Initialize()
        {
            _backgroundShopItemPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.BackgroundShopItem);
            _fieldShopItemPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.FieldShopItem);
        }

        public List<ShopItemDisplay> CreateBackgroundShopItems()
        {
            List<ShopItemDisplay> shopItems = new List<ShopItemDisplay>();
            
            var config = _settingProvider.Get<ShopConfig>();

            var purchasedBGs = _userDataService.GetUserData().UserInventoryData.PurchasedBackgroundIds;
            for (int i = 0; i < config.BackgroundItems.Count; i++)
            {
                if(purchasedBGs.Contains(i))
                    continue;

                var display = _factory.Create<ShopItemDisplay>(_backgroundShopItemPrefab);
                display.Initialize(config.BackgroundItems[i], i);
                shopItems.Add(display);
            }
            
            return shopItems;
        }
        
        public List<ShopItemDisplay> CreateFieldShopItems()
        {
            List<ShopItemDisplay> shopItems = new List<ShopItemDisplay>();
            
            var config = _settingProvider.Get<ShopConfig>();

            var purchasedBGs = _userDataService.GetUserData().UserInventoryData.PurchasedFieldIds;
            for (int i = 0; i < config.FieldItems.Count; i++)
            {
                if(purchasedBGs.Contains(i))
                    continue;

                var display = _factory.Create<ShopItemDisplay>(_fieldShopItemPrefab);
                display.Initialize(config.FieldItems[i], i);
                shopItems.Add(display);
            }
            
            return shopItems;
        }
    }
}