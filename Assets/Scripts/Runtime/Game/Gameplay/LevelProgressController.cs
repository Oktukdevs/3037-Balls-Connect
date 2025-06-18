using System;
using System.Collections.Generic;
using Runtime.Game.Gameplay;
using UnityEngine;

public class LevelProgressController
{
    private readonly GameplayData _gameplayData;
    private readonly PathDrawController _pathDrawController;
    
    public event Action<int> OnProgressChanged;

    public LevelProgressController(GameplayData gameplayData, PathDrawController pathDrawController)
    {
        _gameplayData = gameplayData;
        _pathDrawController = pathDrawController;
        
        _pathDrawController.OnMerged += ProcessMerge;
    }

    private void ProcessMerge(List<GameBall> matchList)
    {
        if(matchList.Count < 3)
            return;

        int value = matchList[0].Value;
        var clearProgressList = _gameplayData.ProgressData;

        for (int i = 0; i < clearProgressList.Count; i++)
        {
            var clearProgress = clearProgressList[i];
            if(clearProgress.ClearCondition.Value != value)
                continue;

            clearProgress.Progress = Mathf.Clamp(clearProgress.Progress + matchList.Count,0,  clearProgress.ClearCondition.Matches);
            OnProgressChanged?.Invoke(i);
        }
    }
}
