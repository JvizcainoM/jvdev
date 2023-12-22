using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

namespace Workshop.Tools
{
    public static class Setup
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            Folders.Dir("_Project", "Animation",
                "Art",
                "Audio",
                "Materials",
                "Music",
                "Prefabs",
                "Settings",
                "Scripts",
                "Scenes",
                "Shaders",
                "Textures");
            Refresh();
        }

        [MenuItem("Tools/Setup/Import Useful Assets")]
        public static void ImportUsefulAssets()
        {
            Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/Editor ExtensionsAnimation");
        }

        [MenuItem("Tools/Setup/Import Prototyping Assets")]
        public static void ImportPrototypingAssets()
        {
            Assets.ImportAsset("FREE Casual Game SFX Pack.unitypackage", "Dustyroom/AudioSound FX");
            Assets.ImportAsset("Gridbox Prototype Materials.unitypackage", "Ciathyza/Textures Materials");
        }

        private static class Folders
        {
            public static void Dir(string root, params string[] folders)
            {
                var fullPath = Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var path = Combine(fullPath, folder);
                    if (!Exists(path))
                    {
                        CreateDirectory(path);
                    }
                }
            }
        }

        private static class Assets
        {
            public static void ImportAsset(string asset, string subfolder,
                string folder = "C:/Users/jiscv/AppData/Roaming/Unity/Asset Store-5.x")
            {
                var path = Combine(folder, subfolder, asset);
                ImportPackage(path, false);
            }
        }
    }
}