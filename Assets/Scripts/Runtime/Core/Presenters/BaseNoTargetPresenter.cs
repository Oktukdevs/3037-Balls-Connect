using Cysharp.Threading.Tasks;

namespace Runtime.Core.Presenters
{
    public abstract class BaseNoTargetPresenter : BasePresenter
    {
        public abstract UniTask Show();
        public abstract UniTask Hide();
    }
}