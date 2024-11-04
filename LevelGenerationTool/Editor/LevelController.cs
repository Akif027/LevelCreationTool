namespace LevelEditorPlugin.Editor
{
    using UnityEditor;
    using LevelEditorPlugin.Runtime;
    using UnityEngine;

    public class LevelController
    {
        public LevelData CreateNewLevelData(string path)
        {
            LevelData newLevelData = ScriptableObject.CreateInstance<LevelData>();
            AssetDatabase.CreateAsset(newLevelData, path);
            AssetDatabase.SaveAssets();
            return newLevelData;
        }

        public LevelData LoadLevelData(int levelIndex)
        {
            var allLevels = Resources.LoadAll<LevelData>("");
            foreach (var levelData in allLevels)
            {
                if (levelData.levelNum == levelIndex)
                {
                    return levelData;
                }
            }
            return null;
        }
    }
}
