using UnityEngine;

namespace Runtime.Game.Services.NetworkConnection
{
    public class NetworkConnectionService : INetworkConnectionService
    {
        bool INetworkConnectionService.IsInternetReachable()
        {
            return UnityEngine.Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}