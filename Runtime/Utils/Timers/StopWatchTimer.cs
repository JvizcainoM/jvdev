namespace JV.Utils.Timers
{
    public class StopWatchTimer : Timer
    {
        public StopWatchTimer(float time) : base(time)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (IsRunning)
                Time += deltaTime;
        }

        public void Reset() => Time = 0;
        public float GetTime() => Time;
    }
}