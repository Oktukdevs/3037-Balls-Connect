using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Gameplay;
using Runtime.Game.Services.SettingsProvider;
using UnityEngine;
using Zenject;

public class BallsFactory : IInitializable
{
    private readonly GameObjectFactory _factory;
    private readonly IAssetProvider _assetProvider;
    
    private GameObject _ballPrefab;

    public BallsFactory(GameObjectFactory factory, IAssetProvider assetProvider)
    {
        _factory = factory;
        _assetProvider = assetProvider;
    }
    
    public async void Initialize()
    {
        _ballPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.BallPrefab);
    }
    
    public GameBall CreateBall()
    {
        GameBall ball = _factory.Create<GameBall>(_ballPrefab);
        return ball;
    }
}
