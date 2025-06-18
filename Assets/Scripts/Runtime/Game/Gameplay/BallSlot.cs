namespace Runtime.Game.Gameplay
{
    public class BallSlot
    {
        public int Value;
        public GameBall GameBall;

        public void Initialize(int value, GameBall gameBall)
        {
            GameBall = gameBall;
            SetValue(value);
        }

        public void SetValue(int increase)
        {
            Value = increase;
            GameBall.SetValue(Value);
        }

        public void Clear()
        {
            Value = 0;
            GameBall = null;
        }

        public bool HasBall() => GameBall;
    }
}