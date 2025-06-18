using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Shop;
using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class SpritesProvider
    {
        private readonly ISettingProvider _settingProvider;
        private readonly UserInventoryService _userInventoryService;

        public SpritesProvider(ISettingProvider settingProvider, UserInventoryService userInventoryService)
        {
            _settingProvider = settingProvider;
            _userInventoryService = userInventoryService;
        }
        
        public Sprite GetBallSprite(int id) => _settingProvider.Get<BallSkinsConfig>().Sprites[id];

        public (Sprite, Sprite) GetGameplaySprites()
        {
            var config = _settingProvider.Get<ShopConfig>();
            var fieldSet = config.FieldItems[_userInventoryService.GetSelectedFieldId()];
            return (fieldSet.Sprite, fieldSet.HeaderSprite);
        }
        
        public Sprite GetBackgroundSprite()
        {
            var config = _settingProvider.Get<ShopConfig>();
            return config.BackgroundItems[_userInventoryService.GetSelectedBackgroundId()].Sprite;
        }
    }
}