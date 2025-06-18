using Cysharp.Threading.Tasks;
using Runtime.Core.Interfaces;
using UnityEngine;

namespace Runtime.Core.Infrastructure.AssetProvider
{
    public interface IAssetProvider : ICustomInitializer
    {
        UniTask<T> Load<T>(string address) where T : class;
        UniTask<GameObject> Instantiate(string address, Transform under);
        UniTask<GameObject> Instantiate(string address, Vector3 at);
        UniTask<GameObject> Instantiate(string address);
        void Dispose();
    }
}