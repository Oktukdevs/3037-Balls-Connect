using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Gameplay;
using UnityEngine;

public class GameplayTimer
{
    private readonly GameplayData _data;

    public GameplayTimer(GameplayData data)
    {
        _data = data;
    }
    
    private float _timeLeft;
    
    public event Action OnTimeEnd;

    public async UniTaskVoid Start(float timeTotal, CancellationToken token)
    {
        _timeLeft = timeTotal;
        while (_timeLeft > 0 && !token.IsCancellationRequested)
        {
            _timeLeft -= Time.deltaTime;
            _data.TimeLeft = _timeLeft;
            await UniTask.NextFrame(cancellationToken: token);
        }
        
        OnTimeEnd?.Invoke();
    }
}
