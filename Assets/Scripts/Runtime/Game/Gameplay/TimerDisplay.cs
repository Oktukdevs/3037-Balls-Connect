
using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Gameplay;
using Runtime.Game.Tools;
using TMPro;
using UnityEngine;
using Zenject;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    
    private GameplayData _data;

    [Inject]
    private void Construct(GameplayData data)
    {
        _data = data;
        _data.OnTimeChanged += UpdateTime;
    }

    private void OnDestroy()
    {
        _data.OnTimeChanged -= UpdateTime;
    }

    private void UpdateTime(float time)
    {
        _timeText.text = Tools.FormatTime(time);
    }
}
