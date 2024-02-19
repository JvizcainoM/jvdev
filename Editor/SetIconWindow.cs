using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SetIconWindow : EditorWindow
{
    private const string k_menuPath = "Assets/Set Icon";
    private const string k_filter = "t:texture2d l:ScriptIcon";

    private List<Texture2D> m_icons;
    private int m_selectedIcon;

    [MenuItem(k_menuPath, priority = 0)]
    public static void ShowMenuItem()
    {
        var window = GetWindow<SetIconWindow>();
        window.titleContent = new GUIContent("Set Icon");
        window.Show();
    }

    [MenuItem(k_menuPath, validate = true)]
    public static bool ShowMenuItemValidation()
    {
        return Selection.objects.All(asset => asset.GetType() == typeof(MonoScript));
    }

    private void OnGUI()
    {
        if (m_icons == null)
        {
            m_icons = new List<Texture2D>();
            var assetsGuids = AssetDatabase.FindAssets(k_filter);
            foreach (var guid in assetsGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                m_icons.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(path));
            }
        }

        if (m_icons.Count == 0)
        {
            GUILayout.Label("No icons found. Please create some icons first.");
            if (GUILayout.Button("Close", GUILayout.Width(100)))
                Close();
            return;
        }

        var textureArray = m_icons.Cast<Texture>().ToArray();
        m_selectedIcon = GUILayout.SelectionGrid(m_selectedIcon, textureArray, 5);


        if (GUILayout.Button("Apply", GUILayout.Width(100)))
        {
            ApplyIcon(m_icons[m_selectedIcon]);
            Close();
        }

        if (Event.current == null) return;

        if (Event.current.isKey)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.Return or KeyCode.KeypadEnter:
                    ApplyIcon(m_icons[m_selectedIcon]);
                    Close();
                    break;
                case KeyCode.Escape:
                    Close();
                    break;
            }
        }
        else if (Event.current.button == 0 && Event.current.clickCount == 2)
        {
            ApplyIcon(m_icons[m_selectedIcon]);
            Close();
        }
    }

    private void ApplyIcon(Texture2D icon)
    {
        AssetDatabase.StartAssetEditing();
        foreach (var asset in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            var importer = AssetImporter.GetAtPath(path) as MonoImporter;
            importer!.SetIcon(icon);
            AssetDatabase.ImportAsset(path);
        }

        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }
}