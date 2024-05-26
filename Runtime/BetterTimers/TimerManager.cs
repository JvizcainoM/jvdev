using System.Collections.Generic;

namespace JvDev.BetterTimers
{
    public static class TimerManager
    {
        private static readonly List<Timer> Timers = new();

        public static void RegisterTimer(Timer timer) => Timers.Add(timer);
        public static void UnregisterTimer(Timer timer) => Timers.Remove(timer);

        public static void UpdateTimers()
        {
            foreach (var timer in new List<Timer>(Timers))
            {
                timer.Tick();
            }
        }

        public static void Clear() => Timers.Clear();
    }
}