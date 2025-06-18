using System.Diagnostics;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.UserData;

namespace Runtime.Game.Shop
{
    public class ShopService
    {
        private readonly UserDataService _userDataService;
        private readonly UserInventoryService _userInventoryService;
        private readonly ISettingProvider _settingProvider;

        public ShopService(UserDataService userDataService, UserInventoryService userInventoryService,
            ISettingProvider settingProvider)
        {
            _userDataService = userDataService;
            _userInventoryService = userInventoryService;
            _settingProvider = settingProvider;
        }
    }
}