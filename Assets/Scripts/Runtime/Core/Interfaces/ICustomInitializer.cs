using Cysharp.Threading.Tasks;

namespace Runtime.Core.Interfaces
{
    public interface ICustomInitializer
    {
        UniTask Initialize();
    }
    
    public interface ICustomInitializer<P>
    {
        UniTask Initialize(P param);
    }
}