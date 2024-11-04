using UnityEditor;
using UnityEngine;
using LevelEditorPlugin.Runtime;
using System.Collections.Generic;

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

            if (GUILayout.Button("Create New Level"))
            {
                CreateNewLevelData();
            }

            currentLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevelData, typeof(LevelData), false);

            if (currentLevelData != null)
            {
                serializedLevelData = new SerializedObject(currentLevelData);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                DrawSections();
                EditorGUILayout.EndScrollView();

                if (GUILayout.Button("Save Level Settings"))
                {
                    SaveLevelData();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Select or create a LevelData asset to edit.", MessageType.Info);
            }
        }

        private void DrawSections()
        {
            serializedLevelData.Update();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Level Settings", EditorStyles.boldLabel);
            uiManager.DrawLevelSettings(currentLevelData);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Environment Management", EditorStyles.boldLabel);
            environmentUIController.DrawEnvironmentUI(currentLevelData);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Particle System Prefabs", EditorStyles.boldLabel);
            uiManager.DrawParticlePrefabsList(currentLevelData);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Sound Effects", EditorStyles.boldLabel);
            uiManager.DrawSoundEffectsList(currentLevelData);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            levelPreview.DrawPreviewButton(currentLevelData);

            serializedLevelData.ApplyModifiedProperties();
        }

        private void CreateNewLevelData()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create New Level", "NewLevelData", "asset", "Specify where to save the new LevelData.");
            if (!string.IsNullOrEmpty(path))
            {
                LevelData newLevelData = ScriptableObject.CreateInstance<LevelData>();

                InitializeLevelData(newLevelData);

                AssetDatabase.CreateAsset(newLevelData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                currentLevelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
                Repaint();

                Debug.Log("New LevelData asset created at: " + path);
            }
            else
            {
                Debug.LogWarning("No path specified. Level creation cancelled.");
            }
        }

        private void InitializeLevelData(LevelData levelData)
        {
            levelData.levelName = "New Level";
            levelData.successAnimations = new AnimationClip[0];
            levelData.words = new List<string>();
            levelData.correctWords = new List<string>();
            levelData.environmentOptions = new EnvironmentData[0];
            levelData.placedEnvironments = new List<LevelData.PlacedEnvironment>();
            levelData.particlePrefabs = new List<LevelData.ParticleEntry>();
            levelData.soundEffects = new List<LevelData.SoundEntry>();
            levelData.animatedSceneOnCompletion = true; // Set default for animated scene toggle
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
