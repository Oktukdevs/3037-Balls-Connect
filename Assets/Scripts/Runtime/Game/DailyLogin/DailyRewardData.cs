using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DailyRewardData
{
    public DailyRewardType Type;
    public Sprite Sprite;
    public int Amount;
}

public enum DailyRewardType
{
    Coins,
    Refresh,
    Swap
}