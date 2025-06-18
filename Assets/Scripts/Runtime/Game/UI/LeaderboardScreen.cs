using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.GameStates.Game.Controllers;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class LeaderboardScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;

    private LeaderboardRecordFactory _leaderboardRecordFactory;
    private ProfileController _profileController;
    
    private List<GameObject> _records = new List<GameObject>();
    
    [Inject]
    private void Construct(LeaderboardRecordFactory leaderboardRecordFactory, ProfileController profileController)
    {
        _leaderboardRecordFactory = leaderboardRecordFactory;
        _profileController = profileController;
        
        _profileController.OnSaved += RecreateLeaderboard;
    }

    private void OnDestroy()
    {
        _profileController.OnSaved -= RecreateLeaderboard;
    }

    private void RecreateLeaderboard()
    {
        foreach (var record in _records)
            Object.Destroy(record.gameObject);
        
        _records.Clear();
        PopulateRecords();
    }

    private void Awake()
    {
        PopulateRecords();
    }

    private void PopulateRecords()
    {
        foreach (var record in _leaderboardRecordFactory.CreateRecords())
        {
            _records.Add(record.gameObject);
            record.transform.SetParent(_parent, false);
        }
    }
}
