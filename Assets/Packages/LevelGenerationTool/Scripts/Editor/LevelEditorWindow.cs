using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private LevelData levelData; // Reference to the LevelData ScriptableObject
    private EnvironmentData selectedEnvironment; // Selected environment to instantiate or replace
    private const string environmentContainerName = "EnvironmentContainer";

    [MenuItem("Tools/Level Environment Editor")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.titleContent = new GUIContent("Level Environment Editor");
        window.minSize = new Vector2(400, 300);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Environment Placement Tool", EditorStyles.boldLabel);

        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);

        if (levelData != null)
        {
            DrawEnvironmentSelection();
            DrawControlButtons();
        }
    }

    private void DrawEnvironmentSelection()
    {
        GUILayout.Label("Environment Selection", EditorStyles.boldLabel);
        foreach (var environment in levelData.environmentOptions)
        {
            if (environment == null || environment.prefab == null) continue;

            if (GUILayout.Button(environment.environmentName))
            {
                if (Selection.activeGameObject != null)
                {
                    ReplaceEnvironment(Selection.activeGameObject, environment);
                }
                else
                {
                    InstantiateEnvironment(environment);
                }
            }
        }
    }

    private void DrawControlButtons()
    {
        GUILayout.Space(10);

        if (GUILayout.Button("Delete Selected Environment") && Selection.activeGameObject != null)
        {
            DeleteEnvironment(Selection.activeGameObject);
        }

        if (GUILayout.Button("Delete All Environments"))
        {
            DeleteAllEnvironments();
        }

        if (GUILayout.Button("Save Environment Setup"))
        {
            SaveEnvironmentSetup();
        }
    }

    private void InstantiateEnvironment(EnvironmentData environmentData)
    {
        if (environmentData != null)
        {
            Vector3 defaultPosition = GetSceneCenterInWorld();
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(environmentData.prefab);
            if (instance != null)
            {
                instance.transform.position = defaultPosition;
                instance.transform.SetParent(GetEnvironmentContainer().transform); // Set parent to EnvironmentContainer
                Selection.activeGameObject = instance;

                // Save instantiated environment to LevelData
                levelData.placedEnvironments.Add(new LevelData.PlacedEnvironment
                {
                    environmentData = environmentData,
                    position = defaultPosition,
                    rotation = instance.transform.rotation
                });
            }
        }
    }

    private void ReplaceEnvironment(GameObject oldEnvironment, EnvironmentData newEnvironmentData)
    {
        if (newEnvironmentData != null)
        {
            Vector3 position = oldEnvironment.transform.position;
            Quaternion rotation = oldEnvironment.transform.rotation;

            DestroyImmediate(oldEnvironment);

            GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(newEnvironmentData.prefab);
            if (newInstance != null)
            {
                newInstance.transform.position = position;
                newInstance.transform.rotation = rotation;
                newInstance.transform.SetParent(GetEnvironmentContainer().transform); // Set parent to EnvironmentContainer
                Selection.activeGameObject = newInstance;

                // Update LevelData for replaced environment
                var placedEnvironment = levelData.placedEnvironments.Find(e => e.position == position);
                if (placedEnvironment != null)
                {
                    placedEnvironment.environmentData = newEnvironmentData;
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
    }

    private void DeleteEnvironment(GameObject environment)
    {
        Vector3 position = environment.transform.position;
        DestroyImmediate(environment);

        // Remove from LevelData
        levelData.placedEnvironments.RemoveAll(e => e.position == position);
    }

    private void DeleteAllEnvironments()
    {
        var environmentContainer = GameObject.Find(environmentContainerName);
        if (environmentContainer != null)
        {
            DestroyImmediate(environmentContainer);
        }

        // Clear the list in LevelData
        levelData.placedEnvironments.Clear();
    }

    private void SaveEnvironmentSetup()
    {
        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();
        Debug.Log("Environment setup saved successfully.");
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
        if (sceneView != null)
        {
            Vector3 worldPosition = sceneView.camera.transform.position + sceneView.camera.transform.forward * 10f;
            return new Vector3(worldPosition.x, worldPosition.y, 0);
        }
        return Vector3.zero;
    }
}
