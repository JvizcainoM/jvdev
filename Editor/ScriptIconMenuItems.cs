using UnityEditor;
using UnityEngine;

public static class ScriptIconMenuItems
{
    private const string k_label = "ScriptIcon";

    [MenuItem("Tools/Script Icons/Assing Label")]
    private static void AssignScriptIconMenuItem()
    {
        var objects = Selection.objects;
        if (objects == null) return;

        foreach (var obj in objects)
        {
            //AssetDatabase.SetLabels(obj, new[] { k_label });
            var labels = AssetDatabase.GetLabels(obj);
            if (ArrayUtility.Contains(labels, k_label)) continue;
            ArrayUtility.Add(ref labels, k_label);
            AssetDatabase.SetLabels(obj, labels);
        }
    }

    [MenuItem("Tools/Script Icons/Remove Label")]
    private static void RemoveScriptIconMenuItem()
    {
        var objects = Selection.objects;
        if (objects == null) return;

        foreach (var obj in objects)
        {
            // AssetDatabase.ClearLabels(obj);
            var labels = AssetDatabase.GetLabels(obj);
            ArrayUtility.Remove(ref labels, k_label);
            AssetDatabase.SetLabels(obj, labels);
        }
    }

    [MenuItem("Tools/Script Icons/Find")]
    private static void FindScriptIconMenuItem()
    {
        var guids = AssetDatabase.FindAssets($"t:texture2d l:{k_label}");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log(path);
        }
    }
}