using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

namespace Runtime.Game.Services.IAP
{
    [CreateAssetMenu(fileName = "IAPConfig", menuName = "Config/IAPConfig")]
    public class IAPConfig : BaseSettings
    {
        [SerializeField] private List<ProductData> _products;

        public List<ProductData> Products => _products;
    }
}