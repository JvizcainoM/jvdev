using UnityEngine;

namespace Workshop
{
    public sealed class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        private static bool HasInstance => _instance != null;

        public static T TryGetInstance => HasInstance ? _instance : null;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindFirstObjectByType<T>();
                if (_instance != null) return _instance;

                var obj = new GameObject { name = $"{typeof(T).Name} (Auto)" };
                _instance = obj.AddComponent<T>();

                return _instance;
            }
        }

        private void Awake() => Initialize();
        private void OnDestroy() => _instance = null;

        private void Initialize()
        {
            if (!Application.isPlaying) return;

            _instance = this as T;
        }
    }
}