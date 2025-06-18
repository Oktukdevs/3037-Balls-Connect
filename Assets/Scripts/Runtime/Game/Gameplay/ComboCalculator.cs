using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Game.Gameplay
{
    public class ComboCalculator
    {
        private const int MinMatch = 3;
        private const int MaxCombo = 5;
        
        public int CalculateCombo(List<GameBall> path)
        {
            return  Mathf.Clamp(path.Count / MinMatch, 0, MaxCombo); 
        }
    }
}