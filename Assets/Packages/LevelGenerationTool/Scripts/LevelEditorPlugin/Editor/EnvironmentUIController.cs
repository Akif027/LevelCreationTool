namespace LevelEditorPlugin.Editor
{
    using UnityEditor;
    using UnityEngine;
    using LevelEditorPlugin.Runtime;

    public class EnvironmentUIController
    {
        private EnvironmentManager environmentManager;

        public EnvironmentUIController(EnvironmentManager environmentManager)
        {
            this.environmentManager = environmentManager;
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

                // Button to add environment object
                if (GUILayout.Button($"Add {environment.environmentName}"))
                {
                    environmentManager.InstantiateEnvironment(environment, Vector3.zero);
                }

                // Button to replace the selected object with the chosen environment object
                if (Selection.activeGameObject != null && GUILayout.Button($"Replace with {environment.environmentName}"))
                {
                    environmentManager.ReplaceEnvironment(Selection.activeGameObject, environment);
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            // Button to delete the selected environment object
            if (GUILayout.Button("Delete Selected Environment") && Selection.activeGameObject != null)
            {
                environmentManager.DeleteEnvironment(Selection.activeGameObject);
            }

            // Button to delete all environment objects in the scene
            if (GUILayout.Button("Delete All Environments"))
            {
                environmentManager.DeleteAllEnvironments();
            }
        }
    }
}
