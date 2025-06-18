using System;
using System.Collections.Generic;

namespace Runtime.Game.Gameplay
{
    public class GameplayData
    {
        private int _score = 0;
        private float _timeLeft;

        public int GameLevelId;
        
        public int HighestValue { get; set; }
        public int HighestMatchCount { get; set; }

        public float TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
                OnTimeChanged?.Invoke(_timeLeft);
            }
        }
        
        public event Action<int> OnScoreChanged;
        public event Action<float> OnTimeChanged;


        public List<ClearProgress> ProgressData = new();

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnScoreChanged?.Invoke(value);
            }
        }
    }

    public class ClearProgress
    {
        public ClearCondition ClearCondition;
        public int Progress;
    }
}