using UnityEditor;
using UnityEngine;

public class EnvironmentManager
{
    private const string environmentContainerName = "EnvironmentContainer";

    public void DrawEnvironmentManagement(LevelData levelData)
    {
        GUILayout.Label("Environment Management", EditorStyles.boldLabel);

        foreach (var environment in levelData.environmentOptions)
        {
            if (environment == null || environment.prefab == null)
            {
                Debug.LogWarning("Environment or prefab is missing in levelData.environmentOptions.");
                continue;
            }

            // Instantiate and Replace buttons
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add " + environment.environmentName))
            {
                InstantiateEnvironment(environment, levelData);
            }

            // Ensure that a GameObject is selected in the editor to show the Replace button
            if (Selection.activeGameObject != null && GUILayout.Button("Replace with " + environment.environmentName))
            {
                ReplaceEnvironment(Selection.activeGameObject, environment, levelData);
            }

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Delete Selected Environment") && Selection.activeGameObject != null)
        {
            DeleteEnvironment(Selection.activeGameObject, levelData);
        }

        if (GUILayout.Button("Delete All Environments"))
        {
            DeleteAllEnvironments(levelData);
        }
    }

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

    private void ReplaceEnvironment(GameObject oldEnvironment, EnvironmentData newEnvironmentData, LevelData levelData)
    {
        Vector3 position = oldEnvironment.transform.position;
        Quaternion rotation = oldEnvironment.transform.rotation;
        UnityEngine.Object.DestroyImmediate(oldEnvironment);

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

    private void DeleteEnvironment(GameObject environment, LevelData levelData)
    {
        Vector3 position = environment.transform.position;
        UnityEngine.Object.DestroyImmediate(environment);
        levelData.placedEnvironments.RemoveAll(e => e.position == position);
    }

    private void DeleteAllEnvironments(LevelData levelData)
    {
        var environmentContainer = GameObject.Find(environmentContainerName);
        if (environmentContainer != null)
        {
            UnityEngine.Object.DestroyImmediate(environmentContainer);
        }
        levelData.placedEnvironments.Clear();
    }

    private GameObject GetEnvironmentContainer()
    {
        GameObject container = GameObject.Find(environmentContainerName);
        if (container == null)
        {
            container = new GameObject(environmentContainerName);
        }
        return container;
    }

    private Vector3 GetSceneCenterInWorld()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        return sceneView != null ? sceneView.camera.transform.position + sceneView.camera.transform.forward * 10f : Vector3.zero;
    }
}
