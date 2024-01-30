using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Workshop
{
    /// <summary>
    /// Utility class for handling EventBus instances.
    /// </summary>
    public static class EventBusUtil
    {
        /// <summary>
        /// List of all event types in the application.
        /// </summary>
        public static IReadOnlyList<Type> EventTypes { get; set; }

        /// <summary>
        /// List of all EventBus types in the application.
        /// </summary>
        public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
        /// <summary>
        /// Current state of the Unity Editor's play mode.
        /// </summary>
        public static PlayModeStateChange PlayerModeStateChange { get; set; }

        /// <summary>
        /// Method called when the Unity Editor loads.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void InitializeEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// Method called when the Unity Editor's play mode state changes.
        /// </summary>
        /// <param name="state">The new state of the Unity Editor's play mode.</param>
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            PlayerModeStateChange = state;
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ClearAllBusses();
            }
        }
#endif

        /// <summary>
        /// Method called when the application loads.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
            EventBusTypes = InitializeAllBusses();
        }

        /// <summary>
        /// Initializes all EventBus instances in the application.
        /// </summary>
        /// <returns>A list of all EventBus types in the application.</returns>
        private static IReadOnlyList<Type> InitializeAllBusses()
        {
            var eventBusTypes = new List<Type>();
            var typeDefinition = typeof(EventBus<>);

            foreach (var eventType in EventTypes)
            {
                var busType = typeDefinition.MakeGenericType(eventType);
                eventBusTypes.Add(busType);
                Debug.Log($"Initialized EventBus<{eventType.Name}>");
            }

            return eventBusTypes;
        }

        /// <summary>
        /// Clears all EventBus instances in the application.
        /// </summary>
        public static void ClearAllBusses()
        {
            foreach (var busType in EventBusTypes)
            {
                var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
                clearMethod!.Invoke(null, null);
            }
        }
    }
}