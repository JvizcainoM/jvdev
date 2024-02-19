using System.Linq;
using UnityEditor;
using UnityEngine;


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

    private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null) return;

        var components = go.GetComponents<Component>();
        if (components == null || components.Length == 0) return;

        var component = components.Length > 1 ? components[1] : components[0];
        var type = component.GetType();
        var content = EditorGUIUtility.ObjectContent(component, type);
        content.text = null;
        content.tooltip = type.Name;

        if (content.image == null) return;

        var isSelected = Selection.instanceIDs.Contains(instanceID);
        var isHovering = selectionRect.Contains(Event.current.mousePosition);

        var color = UnityEditorBackgroundColor.Get(isSelected, isHovering, _hierarchyHasFocus);
        // var color = UnityEditorBackgroundColor.SimpleColor;
        var backgroundRect = selectionRect;
        backgroundRect.width = 18.5f;
        EditorGUI.DrawRect(backgroundRect, color);

        EditorGUI.LabelField(selectionRect, content);
    }
}