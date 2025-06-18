using Cysharp.Threading.Tasks;

namespace Runtime.Core.Factory
{
    public abstract class BaseAsyncFactory<T>
    {
        public abstract UniTask<T> CreateAsync();
    }
}