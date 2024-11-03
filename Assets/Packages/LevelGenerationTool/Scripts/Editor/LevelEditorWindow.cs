using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private LevelData currentLevelData;
    private Vector2 scrollPos;

    private EnvironmentManager environmentManager;
    private UIManager uiManager;
    private LevelPreview levelPreview;

    private bool showLevelSettings = true;
    private bool showEnvironmentSettings = true;
    private bool showParticleSettings = true;
    private bool showSoundSettings = true; // Toggle for sound settings

    [MenuItem("Tools/Level Environment and Level Editor")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.titleContent = new GUIContent("Level Environment and Level Editor");
        window.minSize = new Vector2(600, 600);
        window.Show();
    }

    private void OnEnable()
    {
        environmentManager = new EnvironmentManager();
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

        currentLevelData = (LevelData)EditorGUILayout.ObjectField(
            new GUIContent("Level Data", "Select the LevelData asset to edit"),
            currentLevelData,
            typeof(LevelData),
            false
        );

        if (currentLevelData != null)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            showLevelSettings = EditorGUILayout.Foldout(showLevelSettings, "Level Settings");
            if (showLevelSettings)
            {
                uiManager.DrawLevelSettings(currentLevelData);
            }

            showEnvironmentSettings = EditorGUILayout.Foldout(showEnvironmentSettings, "Environment Management");
            if (showEnvironmentSettings)
            {
                environmentManager.DrawEnvironmentManagement(currentLevelData);
            }

            showParticleSettings = EditorGUILayout.Foldout(showParticleSettings, "Particle System Prefabs");
            if (showParticleSettings)
            {
                DrawParticlePrefabsList(currentLevelData);
            }

            showSoundSettings = EditorGUILayout.Foldout(showSoundSettings, "Sound Effects");
            if (showSoundSettings)
            {
                DrawSoundEffectsList(currentLevelData);
            }

            levelPreview.DrawPreviewButton(currentLevelData);

            DrawSaveButton();

            EditorGUILayout.EndScrollView();
        }
    }

    private void DrawParticlePrefabsList(LevelData levelData)
    {
        EditorGUILayout.LabelField("Particle System Prefabs", EditorStyles.boldLabel);

        for (int i = 0; i < levelData.particlePrefabs.Count; i++)
        {
            var entry = levelData.particlePrefabs[i];
            EditorGUILayout.BeginHorizontal();

            entry.name = EditorGUILayout.TextField("Name", entry.name);
            entry.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", entry.prefab, typeof(GameObject), false);

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                levelData.particlePrefabs.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        if (GUILayout.Button("Add Particle Prefab"))
        {
            levelData.particlePrefabs.Add(new LevelData.ParticleEntry());
        }
    }

    private void DrawSoundEffectsList(LevelData levelData)
    {
        EditorGUILayout.LabelField("Sound Effects", EditorStyles.boldLabel);

        for (int i = 0; i < levelData.soundEffects.Count; i++)
        {
            var entry = levelData.soundEffects[i];
            EditorGUILayout.BeginHorizontal();

            entry.name = EditorGUILayout.TextField("Name", entry.name);
            entry.clip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", entry.clip, typeof(AudioClip), false);

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                levelData.soundEffects.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        if (GUILayout.Button("Add Sound Effect"))
        {
            levelData.soundEffects.Add(new LevelData.SoundEntry());
        }
    }

    private void DrawSaveButton()
    {
        GUILayout.Space(10);
        if (GUILayout.Button("Save Level Settings"))
        {
            SaveLevelData();
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

    private void CreateNewLevelData()
    {
        string path = EditorUtility.SaveFilePanelInProject("Create New Level", "NewLevelData", "asset", "Specify where to save the new LevelData.");
        if (!string.IsNullOrEmpty(path))
        {
            LevelData newLevelData = ScriptableObject.CreateInstance<LevelData>();
            InitializeLevelDataFields(newLevelData);
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

    private void InitializeLevelDataFields(LevelData levelData)
    {
        if (levelData != null)
        {
            levelData.words = new System.Collections.Generic.List<string>();
            levelData.correctWords = new System.Collections.Generic.List<string>();
            levelData.successAnimations = new AnimationClip[0];
            levelData.environmentOptions = new EnvironmentData[0];
            levelData.placedEnvironments = new System.Collections.Generic.List<LevelData.PlacedEnvironment>();
            levelData.particlePrefabs = new System.Collections.Generic.List<LevelData.ParticleEntry>();
            levelData.soundEffects = new System.Collections.Generic.List<LevelData.SoundEntry>();

            levelData.levelName = "New Level";
            levelData.animatedSceneOnCompletion = true;
        }
    }
}
