using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig", order = 0)]
public class LevelConfig : ScriptableObject
{
    public List<ClearCondition> ClearConditions = new List<ClearCondition>();
    public int Reward;
    public float TimeTotal = 30;
}

[Serializable]
public class ClearCondition
{
    public int Value;
    public int Matches;
}
