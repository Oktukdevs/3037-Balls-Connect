using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Gameplay;
using UnityEngine;
using Zenject;

public class ProgressDisplayController : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;
    [SerializeField] private GameObject _prefab;

    private SpritesProvider _spritesProvider;
    private LevelProgressController _levelProgressController;
    private GameplayData _gameplayData;
    
    private List<ProgressDisplay> _progressDisplayList;
    
    [Inject]
    private void Construct(SpritesProvider spritesProvider, LevelProgressController levelProgressController,
        GameplayData gameplayData)
    {
        _spritesProvider = spritesProvider;
        _levelProgressController = levelProgressController;
        _gameplayData = gameplayData;
        
        _levelProgressController.OnProgressChanged += UpdateProgress;
    }

    private void OnDestroy()
    {
        _levelProgressController.OnProgressChanged -= UpdateProgress;
    }

    private void UpdateProgress(int id)
    {
        _progressDisplayList[id].UpdateProgress(_gameplayData.ProgressData[id].Progress,
            _gameplayData.ProgressData[id].ClearCondition.Matches);
    }

    public void Initialize(LevelConfig levelConfig)
    {
        _progressDisplayList = new List<ProgressDisplay>();
        foreach (var clearCondition in levelConfig.ClearConditions)
        {
            var display = Instantiate(_prefab, _parent).GetComponent<ProgressDisplay>();
            display.Initialize(_spritesProvider.GetBallSprite(clearCondition.Value - 1), clearCondition.Matches);
            display.transform.SetParent(_parent, false);
            
            _progressDisplayList.Add(display);
        }
    }
}
