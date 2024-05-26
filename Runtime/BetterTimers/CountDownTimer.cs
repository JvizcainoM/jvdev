using UnityEngine;

namespace JvDev.BetterTimers
{
    public class CountDownTimer : Timer
    {
        public CountDownTimer(float time) : base(time)
        {
        }

        public override void Tick()
        {
            if (!IsRunning) return;

            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0)
            {
                Stop();
            }
        }

        public override bool IsFinished => CurrentTime <= 0;
    }
}