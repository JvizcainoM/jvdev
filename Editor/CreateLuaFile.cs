using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateLuaFile : Editor
{
    private const string DefaultLuaCode = "print(\"Hello from Lua!\")";

    [MenuItem("Assets/Create/Lua Script", false, 80)]
    public static void CreateLuaScript()
    {
        var folderPath = "Assets";
        if (Selection.activeObject != null && AssetDatabase.Contains(Selection.activeObject))
        {
            folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                folderPath = Path.GetDirectoryName(folderPath);
            }
        }

        var fullPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/NewLuaScript.lua");

        File.WriteAllText(fullPath, DefaultLuaCode);
        AssetDatabase.Refresh();
        var obj = AssetDatabase.LoadAssetAtPath<Object>(fullPath);
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);
        EditorUtility.FocusProjectWindow();
        EditorGUIUtility.PingObject(obj);
    }
}