using LevelEditorPlugin.Runtime;
using UnityEditor;
using UnityEngine;

namespace LevelEditorPlugin.Editor
{
    public class EnvironmentUIController
    {
        private readonly EnvironmentManager environmentManager;

        public EnvironmentUIController(EnvironmentManager manager)
        {
            environmentManager = manager;
        }

        public void DrawEnvironmentUI(LevelData levelData)
        {
            GUILayout.Label("Environment Management", EditorStyles.boldLabel);

            foreach (var environment in levelData.environmentOptions)
            {
                if (environment == null || environment.prefab == null)
                {
                    Debug.LogWarning("Environment or prefab is missing in levelData.environmentOptions.");
                    continue;
                }

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button($"Add {environment.environmentName}"))
                {
                    environmentManager.InstantiateEnvironment(environment, levelData);
                }

                if (Selection.activeGameObject != null && GUILayout.Button($"Replace with {environment.environmentName}"))
                {
                    environmentManager.ReplaceEnvironment(Selection.activeGameObject, environment, levelData);
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Delete Selected Environment") && Selection.activeGameObject != null)
            {
                environmentManager.DeleteEnvironment(Selection.activeGameObject, levelData);
            }

            if (GUILayout.Button("Delete All Environments"))
            {
                environmentManager.DeleteAllEnvironments(levelData);
            }
        }
    }
}
