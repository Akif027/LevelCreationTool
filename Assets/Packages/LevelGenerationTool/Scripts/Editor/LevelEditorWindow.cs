using UnityEditor;
using UnityEngine;
using LevelEditorPlugin.Runtime;

namespace LevelEditorPlugin.Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        private LevelController levelController;
        private EnvironmentUIController environmentUIController;
        private UIManager uiManager;
        private LevelPreview levelPreview;

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
            // Initialize controllers and managers
            levelController = new LevelController();
            var environmentModel = new EnvironmentModel();
            var environmentManager = new EnvironmentManager(environmentModel);
            environmentUIController = new EnvironmentUIController(environmentManager);
            uiManager = new UIManager();
            levelPreview = new LevelPreview();
        }

        private void OnGUI()
        {
            GUILayout.Label("Level Editor", EditorStyles.boldLabel);

            // Button to create a new level
            if (GUILayout.Button("Create New Level"))
            {
                CreateNewLevelData();
            }

            // Field to select an existing LevelData asset
            currentLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevelData, typeof(LevelData), false);

            if (currentLevelData != null)
            {
                // Initialize SerializedObject for LevelData
                serializedLevelData = new SerializedObject(currentLevelData);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                DrawSections();
                EditorGUILayout.EndScrollView();

                // Save button
                if (GUILayout.Button("Save Level Settings"))
                {
                    SaveLevelData();
                }
            }
        }

        private void DrawSections()
        {
            serializedLevelData.Update();

            // Draw Level Settings Section
            GUILayout.Label("Level Settings", EditorStyles.boldLabel);
            uiManager.DrawLevelSettings(currentLevelData);

            GUILayout.Space(10);

            // Draw Environment Management Section
            GUILayout.Label("Environment Management", EditorStyles.boldLabel);
            environmentUIController.DrawEnvironmentUI(currentLevelData);

            GUILayout.Space(10);

            // Draw Particle System Prefabs Section
            GUILayout.Label("Particle System Prefabs", EditorStyles.boldLabel);
            uiManager.DrawParticlePrefabsList(currentLevelData);

            GUILayout.Space(10);

            // Draw Sound Effects Section
            GUILayout.Label("Sound Effects", EditorStyles.boldLabel);
            uiManager.DrawSoundEffectsList(currentLevelData);

            GUILayout.Space(10);

            // Draw Preview Button
            levelPreview.DrawPreviewButton(currentLevelData);

            serializedLevelData.ApplyModifiedProperties();
        }

        private void CreateNewLevelData()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create New Level", "NewLevelData", "asset", "Specify where to save the new LevelData.");
            if (!string.IsNullOrEmpty(path))
            {
                LevelData newLevelData = ScriptableObject.CreateInstance<LevelData>();
                AssetDatabase.CreateAsset(newLevelData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                currentLevelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                serializedLevelData = new SerializedObject(currentLevelData);
                Repaint();

                Debug.Log("New LevelData asset created at: " + path);
            }
            else
            {
                Debug.LogWarning("No path specified. Level creation cancelled.");
            }
        }

        private void SaveLevelData()
        {
            if (currentLevelData == null)
            {
                Debug.LogError("No LevelData assigned. Please assign a LevelData asset before saving.");
                return;
            }

            EditorUtility.SetDirty(currentLevelData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Level settings saved successfully.");
        }
    }
}
