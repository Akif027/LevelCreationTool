using LevelEditorPlugin.Runtime;
using UnityEditor;
using UnityEngine;

namespace LevelEditorPlugin.Editor
{
    /// <summary>
    /// Main editor window for the Level Editor tool, allowing users to create, edit,
    /// and manage level configurations through the Unity editor.
    /// </summary>
    public class LevelEditorWindow : EditorWindow
    {
        // The current LevelData asset being edited
        private LevelData currentLevelData;
        private Vector2 scrollPos;

        // Managers for various aspects of the level editor
        private EnvironmentManager environmentManager;
        private UIManager uiManager;
        private LevelPreview levelPreview;

        // Toggle states for foldable sections
        private bool showLevelSettings = true;
        private bool showEnvironmentSettings = true;
        private bool showParticleSettings = true;
        private bool showSoundSettings = true;

        /// <summary>
        /// Adds the Level Editor to the Unity menu under "Tools" and opens the window.
        /// </summary>
        [MenuItem("Tools/Level Editor Plugin/Level Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelEditorWindow>();
            window.titleContent = new GUIContent("Level Editor");
            window.minSize = new Vector2(600, 600);
            window.Show();
        }

        /// <summary>
        /// Initializes required managers when the window is enabled.
        /// </summary>
        private void OnEnable()
        {
            environmentManager = new EnvironmentManager();
            uiManager = new UIManager();
            levelPreview = new LevelPreview();
        }

        /// <summary>
        /// Draws the GUI elements for the Level Editor window.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label("Level Editor", EditorStyles.boldLabel);

            // Button to create a new level
            if (GUILayout.Button("Create New Level"))
            {
                CreateNewLevelData();
            }

            // Field to select an existing LevelData asset
            currentLevelData = (LevelData)EditorGUILayout.ObjectField(
                new GUIContent("Level Data", "Select the LevelData asset to edit"),
                currentLevelData,
                typeof(LevelData),
                false
            );

            // Display editor sections if a LevelData asset is selected
            if (currentLevelData != null)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                DrawSections();
                EditorGUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// Draws each section of the Level Editor (settings, environment, particles, sounds).
        /// </summary>
        private void DrawSections()
        {
            // Level Settings Section
            showLevelSettings = EditorGUILayout.Foldout(showLevelSettings, "Level Settings");
            if (showLevelSettings)
            {
                uiManager.DrawLevelSettings(currentLevelData);
            }

            // Environment Management Section
            showEnvironmentSettings = EditorGUILayout.Foldout(showEnvironmentSettings, "Environment Management");
            if (showEnvironmentSettings)
            {
                environmentManager.DrawEnvironmentManagement(currentLevelData);
            }

            // Particle System Prefabs Section
            showParticleSettings = EditorGUILayout.Foldout(showParticleSettings, "Particle System Prefabs");
            if (showParticleSettings)
            {
                uiManager.DrawParticlePrefabsList(currentLevelData);
            }

            // Sound Effects Section
            showSoundSettings = EditorGUILayout.Foldout(showSoundSettings, "Sound Effects");
            if (showSoundSettings)
            {
                uiManager.DrawSoundEffectsList(currentLevelData);
            }

            // Preview and Save buttons
            levelPreview.DrawPreviewButton(currentLevelData);
            DrawSaveButton();
        }

        /// <summary>
        /// Creates a new LevelData asset and initializes it with default values.
        /// </summary>
        private void CreateNewLevelData()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create New Level", "NewLevelData", "asset", "Specify where to save the new LevelData.");
            if (!string.IsNullOrEmpty(path))
            {
                LevelData newLevelData = ScriptableObject.CreateInstance<LevelData>();
                uiManager.InitializeLevelDataFields(newLevelData);
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

        /// <summary>
        /// Draws the Save button, which allows users to save changes made to the LevelData asset.
        /// </summary>
        private void DrawSaveButton()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("Save Level Settings"))
            {
                SaveLevelData();
            }
        }

        /// <summary>
        /// Saves the current LevelData asset and refreshes the AssetDatabase.
        /// </summary>
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
