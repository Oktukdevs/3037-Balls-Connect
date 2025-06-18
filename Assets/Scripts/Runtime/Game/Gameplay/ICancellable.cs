using System.Threading;

namespace Runtime.Game.Gameplay
{
    public interface ICancellable
    {
        public void SetToken(CancellationToken token);
    }
}