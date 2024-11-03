using LevelEditorPlugin.Runtime;
using UnityEditor;
using UnityEngine;

namespace LevelEditorPlugin.Editor
{
    /// <summary>
    /// Manages environment-related actions in the Level Editor, such as adding,
    /// replacing, and deleting environment objects.
    /// </summary>
    public class EnvironmentManager
    {
        private const string EnvironmentContainerName = "EnvironmentContainer";

        /// <summary>
        /// Draws the environment management UI, allowing users to add, replace,
        /// and delete environment objects.
        /// </summary>
        /// <param name="levelData">The LevelData asset being edited.</param>
        public void DrawEnvironmentManagement(LevelData levelData)
        {
            GUILayout.Label("Environment Management", EditorStyles.boldLabel);

            // List all available environment options
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
                    InstantiateEnvironment(environment, levelData);
                }

                // Button to replace the selected object with the chosen environment object
                if (Selection.activeGameObject != null && GUILayout.Button($"Replace with {environment.environmentName}"))
                {
                    ReplaceEnvironment(Selection.activeGameObject, environment, levelData);
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            // Button to delete the selected environment object
            if (GUILayout.Button("Delete Selected Environment") && Selection.activeGameObject != null)
            {
                DeleteEnvironment(Selection.activeGameObject, levelData);
            }

            // Button to delete all environment objects in the scene
            if (GUILayout.Button("Delete All Environments"))
            {
                DeleteAllEnvironments(levelData);
            }
        }

        /// <summary>
        /// Instantiates a new environment object in the scene and adds it to the
        /// environment container.
        /// </summary>
        /// <param name="environmentData">The EnvironmentData asset for the environment object.</param>
        /// <param name="levelData">The LevelData asset to store the environment instance data.</param>
        private void InstantiateEnvironment(EnvironmentData environmentData, LevelData levelData)
        {
            Vector3 defaultPosition = GetSceneCenterInWorld();
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(environmentData.prefab);
            if (instance != null)
            {
                instance.transform.position = defaultPosition;
                instance.transform.SetParent(GetEnvironmentContainer().transform);
                Selection.activeGameObject = instance;

                levelData.placedEnvironments.Add(new LevelData.PlacedEnvironment
                {
                    environmentData = environmentData,
                    position = defaultPosition,
                    rotation = instance.transform.rotation
                });
            }
        }

        /// <summary>
        /// Replaces an existing environment object in the scene with a new environment object.
        /// </summary>
        /// <param name="oldEnvironment">The currently selected GameObject to replace.</param>
        /// <param name="newEnvironmentData">The EnvironmentData asset for the replacement environment.</param>
        /// <param name="levelData">The LevelData asset to update the environment instance data.</param>
        private void ReplaceEnvironment(GameObject oldEnvironment, EnvironmentData newEnvironmentData, LevelData levelData)
        {
            Vector3 position = oldEnvironment.transform.position;
            Quaternion rotation = oldEnvironment.transform.rotation;
            Object.DestroyImmediate(oldEnvironment);

            GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(newEnvironmentData.prefab);
            if (newInstance != null)
            {
                newInstance.transform.position = position;
                newInstance.transform.rotation = rotation;
                newInstance.transform.SetParent(GetEnvironmentContainer().transform);
                Selection.activeGameObject = newInstance;

                var existingEnvironment = levelData.placedEnvironments.Find(e => e.position == position);
                if (existingEnvironment != null)
                {
                    existingEnvironment.environmentData = newEnvironmentData;
                    existingEnvironment.rotation = rotation;
                }
                else
                {
                    levelData.placedEnvironments.Add(new LevelData.PlacedEnvironment
                    {
                        environmentData = newEnvironmentData,
                        position = position,
                        rotation = rotation
                    });
                }
            }
        }

        /// <summary>
        /// Deletes the selected environment object from the scene and removes its reference from LevelData.
        /// </summary>
        /// <param name="environment">The selected environment GameObject to delete.</param>
        /// <param name="levelData">The LevelData asset to update after deletion.</param>
        private void DeleteEnvironment(GameObject environment, LevelData levelData)
        {
            Vector3 position = environment.transform.position;
            Object.DestroyImmediate(environment);
            levelData.placedEnvironments.RemoveAll(e => e.position == position);
        }

        /// <summary>
        /// Deletes all environment objects in the scene and clears the references in LevelData.
        /// </summary>
        /// <param name="levelData">The LevelData asset to update after deletion.</param>
        private void DeleteAllEnvironments(LevelData levelData)
        {
            var environmentContainer = GameObject.Find(EnvironmentContainerName);
            if (environmentContainer != null)
            {
                Object.DestroyImmediate(environmentContainer);
            }
            levelData.placedEnvironments.Clear();
        }

        /// <summary>
        /// Gets the container GameObject for all environment objects in the scene,
        /// creating it if it doesn't exist.
        /// </summary>
        /// <returns>The GameObject that holds all environment objects.</returns>
        private GameObject GetEnvironmentContainer()
        {
            GameObject container = GameObject.Find(EnvironmentContainerName);
            if (container == null)
            {
                container = new GameObject(EnvironmentContainerName);
            }
            return container;
        }

        /// <summary>
        /// Calculates the default position to place new environment objects in the scene.
        /// </summary>
        /// <returns>The calculated Vector3 position in world space.</returns>
        private Vector3 GetSceneCenterInWorld()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            return sceneView != null ? sceneView.camera.transform.position + sceneView.camera.transform.forward * 10f : Vector3.zero;
        }
    }
}
