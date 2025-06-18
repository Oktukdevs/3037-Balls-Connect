using System.Collections.Generic;
using System.Linq;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Services.SettingsProvider;
using Runtime.Game.Services.UserData;
using UnityEngine;
using Zenject;

public class LeaderboardRecordFactory : IInitializable
{
    private readonly IAssetProvider _assetProvider;
    private readonly GameObjectFactory _factory;
    private readonly UserDataService _userDataService;

    private GameObject _prefab;
    private Sprite _top1Sprite;
    private Sprite _top2Sprite;
    private Sprite _top3Sprite;
    private Sprite _top4Sprite;
    
    public LeaderboardRecordFactory(IAssetProvider assetProvider, GameObjectFactory factory,
        UserDataService userDataService)
    {
        _assetProvider = assetProvider;
        _factory = factory;
        _userDataService = userDataService;
    }
    
    public async void Initialize()
    {
        _prefab = await _assetProvider.Load<GameObject>(ConstPrefabs.RecordPrefab);
        _top1Sprite = await _assetProvider.Load<Sprite>(ConstSprites.Top1Sprite);
        _top2Sprite = await _assetProvider.Load<Sprite>(ConstSprites.Top2Sprite);
        _top3Sprite = await _assetProvider.Load<Sprite>(ConstSprites.Top3Sprite);
        _top4Sprite = await _assetProvider.Load<Sprite>(ConstSprites.Top4Sprite);
    }

    public List<RecordDisplay> CreateRecords()
    {
        int place = 1;

        List<RecordDisplay> records = new List<RecordDisplay>();
        
        foreach (var data in GetRecordData())
        {
            var display = _factory.Create<RecordDisplay>(_prefab);
            display.Initialize(GetPlaceSprite(place), place, data.Name, data.Score);
            records.Add(display);
            place++;
        }
        
        return records;
    }

    private Sprite GetPlaceSprite(int place)
    {
        if(place == 1)
            return _top1Sprite;
        
        if(place == 2)
            return _top2Sprite;
        
        if(place == 3)
            return _top3Sprite;
        
        return _top4Sprite;
    }

    private List<RecordData> GetRecordData()
    {
        List<RecordData> recordData = new List<RecordData>
        {
            new() { Name = "Marge", Score = 201 },
            new() { Name = "Bob", Score = 5123 },
            new() { Name = "Marry", Score = 56123 },
            new() { Name = "Steve", Score = 125 },
            new() { Name = "Stephen", Score = 1623 },
            new() { Name = "Sandy", Score = 85623 },
            new() { Name = "Clad", Score = 84234 },
            new() { Name = "George", Score = 6123 },
            new() { Name = "Max", Score = 645 },
            new() { Name = "Dorethy", Score = 7451 },
            new() { Name = "Peter", Score = 12743 },
            new() { Name = "Rob", Score = 748561 },
            new() { Name = "Susie", Score = 63213 },
            new() { Name = "Daniel", Score = 67457 },
            new() { Name = "Mark", Score = 1246 },
            new() { Name = "Arthur", Score = 652 },
            new() { Name = "Denis", Score = 17623 },
            new() { Name = "Andrew", Score = 217123 },
            new() { Name = "Lois", Score = 176712 },
            new() { Name = "Ivan", Score = 8923 },
            new() { Name = "Lamar", Score = 902341 },
            new() { Name = "Jane", Score = 94134 },
            new() { Name = "Lucy", Score = 6523 },
        };
        
        recordData.Add(new()
        {
            Name = _userDataService.GetUserData().UserProfileData.Name,
            Score = _userDataService.GetUserData().UserProgressData.HighestScore,
        });

        recordData = recordData.OrderByDescending(x => x.Score).ToList();
        
        return recordData;
    }
    
    private class RecordData
    {
        public string Name;
        public int Score;
    }
}
