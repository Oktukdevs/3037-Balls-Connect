using Runtime.Core.Audio;
using Runtime.Core.Compressor;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.FileStorageService;
using Runtime.Core.Infrastructure.FileSystemService;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Core.Infrastructure.PersistantDataProvider;
using Runtime.Core.Infrastructure.Serializer;
using Runtime.Core.Infrastructure.ServerProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.ApplicationState;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.IAP;
using Runtime.Game.Services.NetworkConnection;
using Runtime.Game.Services.ScreenOrientation;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using UnityEngine;
using Zenject;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.Services
{
    [CreateAssetMenu(fileName = "ServicesInstaller", menuName = "Installers/ServicesInstaller")]
    public class ServicesInstaller : ScriptableObjectInstaller<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IUiService>().To<UiService>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IPersistentDataProvider>().To<PersistantDataProvider>().AsSingle();
            Container.Bind<ISettingProvider>().To<SettingsProvider.SettingsProvider>().AsSingle();
            Container.Bind<ILogger>().To<SimpleLogger>().AsSingle();
            Container.Bind<IFileStorageService>().To<PersistentFileStorageService>().AsSingle();
            Container.Bind<IFileCleaner>().To<FileCleaner>().AsSingle();
            Container.Bind<ISerializationProvider>().To<JsonSerializationProvider>().AsSingle();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
            Container.Bind<INetworkConnectionService>().To<NetworkConnectionService>().AsSingle();
            Container.Bind<BaseCompressor>().To<ZipCompressor>().AsSingle();
            Container.Bind<IIAPService>().To<IAPServiceMock>().AsSingle();
            Container.Bind<GameObjectFactory>().AsSingle();
            Container.Bind<ApplicationStateService>().AsSingle();
            Container.Bind<UserDataService>().AsSingle();
            Container.Bind<ServerProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScreenOrientationAlertController>().AsSingle();
        }
    }
}