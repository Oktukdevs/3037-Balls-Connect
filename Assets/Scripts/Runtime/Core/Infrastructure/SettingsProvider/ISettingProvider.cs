using Cysharp.Threading.Tasks;

namespace Runtime.Core.Infrastructure.SettingsProvider
{
    public interface ISettingProvider
    {
        UniTask Initialize();
        T Get<T>() where T : BaseSettings;
        void Set(BaseSettings config);
    }
}