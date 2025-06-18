using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Services.SettingsProvider;
using UnityEngine;
using Zenject;

public class LineDrawer : IInitializable
{
    private readonly IAssetProvider _assetProvider;

    private LineRenderer _lineRenderer;
    
    public LineDrawer(IAssetProvider assetProvider)
    {
        _assetProvider = assetProvider;
    }

    public async void Initialize()
    {
        var go = await _assetProvider.Load<GameObject>(ConstPrefabs.LineRendererPrefab);
        _lineRenderer = Object.Instantiate(go).GetComponent<LineRenderer>();
    }
    
    public void AddPathPoint(int count, Vector2 pos)
    {
        _lineRenderer.positionCount = count;
        _lineRenderer.SetPosition(count - 1, pos);
    }

    public void ClearLine()
    {
        _lineRenderer.positionCount = 0;
    }
}
