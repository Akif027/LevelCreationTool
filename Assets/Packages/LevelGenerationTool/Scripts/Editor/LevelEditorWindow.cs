using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private LevelData currentLevelData;
    private Vector2 scrollPos;

    private EnvironmentManager environmentManager;
    private UIManager uiManager;
    private WordManager wordManager;
    private LevelPreview levelPreview;

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
        wordManager = new WordManager();
        levelPreview = new LevelPreview();
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        currentLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevelData, typeof(LevelData), false);

        if (currentLevelData != null)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            uiManager.DrawLevelSettings(currentLevelData);
            // wordManager.DrawWordManagement(currentLevelData);
            environmentManager.DrawEnvironmentManagement(currentLevelData);
            levelPreview.DrawPreviewButton(currentLevelData);

            EditorGUILayout.EndScrollView();
        }
    }
}
