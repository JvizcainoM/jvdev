using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Playmex.Editor
{
    [InitializeOnLoad]
    public static class HierarchyIconDisplay
    {
        private static bool _hierarchyHasFocus;
        private static EditorWindow _hierarchyWindow;

        static HierarchyIconDisplay()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
            EditorApplication.update += OnEditorUpdate;
        }

        private static void OnEditorUpdate()
        {
            if (_hierarchyWindow == null)
                _hierarchyWindow =
                    EditorWindow.GetWindow(System.Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor"));

            _hierarchyHasFocus = EditorWindow.focusedWindow != null
                                 && EditorWindow.focusedWindow == _hierarchyWindow;
        }

        private static void DrawActivateToggle(Rect selectionRect, GameObject go)
        {
            var toggleRect = new Rect(selectionRect);
            toggleRect.x -= 27f;
            toggleRect.width = 13f;

            var isActive = go.activeSelf;
            var newIsActive = EditorGUI.Toggle(toggleRect, isActive);
            if (newIsActive == isActive) return;
            Undo.RecordObject(go, "Toggle active state");
            go.SetActive(newIsActive);
            EditorSceneManager.MarkSceneDirty(go.scene);
        }

        private static bool HasMissingComponentsInHierarchy(GameObject go)
        {
            var components = go.GetComponents<Component>();
            if (components != null && components.Any(c => c == null))
                return true;
            
            foreach (Transform child in go.transform)
            {
                if (HasMissingComponentsInHierarchy(child.gameObject))
                    return true;
            }

            return false;
        }

        private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go == null) return;
            DrawActivateToggle(selectionRect, go);

            
            if (HasMissingComponentsInHierarchy(go))
            {
                var warningIcon = EditorGUIUtility.IconContent("console.warnicon");
                var iconRect = new Rect(selectionRect);
                iconRect.x -= 44f;
                iconRect.width = 16f;
                GUI.Label(iconRect, warningIcon);
            }

            var components = go.GetComponents<Component>();
            if (components == null || components.Length == 0) return;

            var component = components.Length > 1 ? components[1] ?? components[0] : components[0];

            var type = component.GetType();
            var content = EditorGUIUtility.ObjectContent(component, type);
            content.text = null;
            content.tooltip = type.Name;

            if (content.image == null) return;

            var isSelected = Selection.instanceIDs.Contains(instanceID);
            var isHovering = selectionRect.Contains(Event.current.mousePosition);

            var color = UnityEditorBackgroundColor.Get(isSelected, isHovering, _hierarchyHasFocus); ;
            var backgroundRect = selectionRect;
            backgroundRect.width = 18.5f;
            EditorGUI.DrawRect(backgroundRect, color);

            EditorGUI.LabelField(selectionRect, content);
        }
    }
}