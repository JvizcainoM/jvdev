using System;
using UnityEngine;

namespace JvDev.BetterTimers
{
    public abstract class Timer : IDisposable
    {
        public float CurrentTime { get; protected set; }
        public bool IsRunning { get; protected set; }

        public float InitialTime { get; protected set; }

        public float Progress => Mathf.Clamp01(CurrentTime / InitialTime);

        public event Action OnTimerStart = delegate { };
        public event Action OnTimerStop = delegate { };

        public Timer(float time)
        {
            InitialTime = time;
        }

        ~Timer()
        {
            Dispose(false);
        }

        public void Start()
        {
            CurrentTime = InitialTime;
            if (IsRunning) return;

            IsRunning = true;
            TimerManager.RegisterTimer(this);
            OnTimerStart();
        }

        public void Stop()
        {
            if (!IsRunning) return;

            IsRunning = false;
            TimerManager.UnregisterTimer(this);
            OnTimerStop();
        }

        public abstract void Tick();
        public abstract bool IsFinished { get; }

        public void Pause() => IsRunning = false;
        public void Resume() => IsRunning = true;

        public virtual void Reset() => CurrentTime = InitialTime;

        public virtual void Reset(float time)
        {
            InitialTime = time;
            Reset();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                TimerManager.UnregisterTimer(this);
            }

            _disposed = true;
        }
    }
}