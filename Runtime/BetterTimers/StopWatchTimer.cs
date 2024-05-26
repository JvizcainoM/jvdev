using UnityEngine;

namespace JvDev.BetterTimers
{
    public class StopWatchTimer : Timer
    {
        public StopWatchTimer(float time) : base(time)
        {
        }

        public override void Tick()
        {
            if (!IsRunning) return;

            CurrentTime += Time.deltaTime;
        }

        public override bool IsFinished => false;
        public override void Reset() => CurrentTime = 0;
    }
}