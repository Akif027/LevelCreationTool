using UnityEditor;
using UnityEngine;
using LevelEditorPlugin.Runtime;
using LevelEditorPlugin.Editor;

public class LevelEditorWindow : EditorWindow
{
    private LevelController levelController;
    private EnvironmentUIController environmentUIController;
    private LevelData currentLevelData;
    private SerializedObject serializedLevelData;
    private Vector2 scrollPos;

    [MenuItem("Tools/Level Editor Plugin/Level Editor")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.titleContent = new GUIContent("Level Editor");
        window.minSize = new Vector2(600, 600);
        window.Show();
    }

    private void OnEnable()
    {
        levelController = new LevelController();
        var environmentModel = new EnvironmentModel();
        var environmentManager = new EnvironmentManager(environmentModel);
        environmentUIController = new EnvironmentUIController(environmentManager);
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        // Button to create a new level
        if (GUILayout.Button("Create New Level"))
        {
            string path = EditorUtility.SaveFilePanelInProject("Create New Level", "NewLevelData", "asset", "Specify where to save the new LevelData.");
            if (!string.IsNullOrEmpty(path))
            {
                currentLevelData = levelController.CreateNewLevelData(path);
                serializedLevelData = new SerializedObject(currentLevelData);
            }
        }

        // Field to select an existing LevelData asset
        currentLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevelData, typeof(LevelData), false);

        if (currentLevelData != null)
        {
            serializedLevelData = new SerializedObject(currentLevelData);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawSections();
            EditorGUILayout.EndScrollView();

            // Draw Save button
            if (GUILayout.Button("Save Level Settings"))
            {
                SaveLevelData();
            }
        }
    }

    private void DrawSections()
    {
        serializedLevelData.Update();

        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("levelNum"));
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("levelName"));
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("backgroundImg"));

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Question and Words", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("questionText"));
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("words"), true);
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("correctWords"), true);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Prefabs and Effects", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("wordButtonPrefab"));
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("animatedScenePrefab"));
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("animatedSceneOnCompletion"));
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("successAnimations"), true);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Environment Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("environmentOptions"), true);
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("placedEnvironments"), true);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Particle Effects", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("particlePrefabs"), true);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Sound Effects", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedLevelData.FindProperty("soundEffects"), true);

        serializedLevelData.ApplyModifiedProperties();
    }

    private void SaveLevelData()
    {
        EditorUtility.SetDirty(currentLevelData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Level settings saved successfully.");
    }
}
