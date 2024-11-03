using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager
{
    private Image backgroundImgUI;
    private TextMeshProUGUI questionTextUI;

    public void DrawLevelSettings(LevelData levelData)
    {
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);

        // Level Name
        levelData.levelName = EditorGUILayout.TextField(
            new GUIContent("Level Name", "Enter a unique name for this level."),
            levelData.levelName
        );

        // Background Image
        GUILayout.Label("Background Image", EditorStyles.boldLabel);
        levelData.backgroundImg = (Sprite)EditorGUILayout.ObjectField(
            new GUIContent("Background Image", "Assign a background image for the level."),
            levelData.backgroundImg,
            typeof(Sprite),
            false
        );

        if (GUILayout.Button("Apply Background"))
        {
            ApplyBackground(levelData.backgroundImg);
        }
        GUILayout.Space(10);
        // Success Animations
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        // Question Text
        levelData.questionText = EditorGUILayout.TextField(
            new GUIContent("Question Text", "Enter the question text for this level."),
            levelData.questionText
        );

        // Animated Scene Prefab
        levelData.animatedScenePrefab = (GameObject)EditorGUILayout.ObjectField(
            new GUIContent("Animated Scene Prefab", "Assign a prefab for the animated scene."),
            levelData.animatedScenePrefab,
            typeof(GameObject),
            false
        );


        GUILayout.Space(10);
        // Success Animations

        DrawSuccessAnimations(levelData);


        // Word Management
        DrawWordManagement(levelData);

        // Correct Words Management
        DrawCorrectWords(levelData);
    }

    private void ApplyBackground(Sprite backgroundImg)
    {
        GameObject bgObject = GameObject.FindWithTag("Background");

        if (bgObject == null)
        {
            Debug.LogWarning("No GameObject with tag 'Background' found in the scene. Background not applied.");
            return;
        }

        if (backgroundImgUI == null)
        {
            backgroundImgUI = bgObject.GetComponent<Image>();
            if (backgroundImgUI == null)
            {
                backgroundImgUI = bgObject.AddComponent<Image>();
            }
        }

        if (backgroundImg != null)
        {
            backgroundImgUI.sprite = backgroundImg;
        }
        else
        {
            Debug.LogWarning("Background image is null. Please assign a background image.");
        }
    }

    private void DrawSuccessAnimations(LevelData levelData)
    {
        int animationCount = EditorGUILayout.IntField("Add Animation Clips", levelData.successAnimations.Length);
        if (animationCount != levelData.successAnimations.Length)
        {
            System.Array.Resize(ref levelData.successAnimations, animationCount);
        }

        for (int i = 0; i < animationCount; i++)
        {
            levelData.successAnimations[i] = (AnimationClip)EditorGUILayout.ObjectField(
                new GUIContent("Animation " + (i + 1), "Select an animation clip to play upon level success."),
                levelData.successAnimations[i],
                typeof(AnimationClip),
                false
            );
        }
    }

    public void DrawWordManagement(LevelData levelData)
    {

        GUILayout.Label("Manage Words", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Word"))
        {
            levelData.words.Add(string.Empty);
        }

        for (int i = 0; i < levelData.words.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelData.words[i] = EditorGUILayout.TextField(
                new GUIContent("Word " + (i + 1), "Enter a word for this level."),
                levelData.words[i]
            );

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                levelData.words.RemoveAt(i);
                break;  // Exit loop to prevent index errors after deletion
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    public void DrawCorrectWords(LevelData levelData)
    {
        GUILayout.Label("Correct Words", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Correct Word"))
        {
            levelData.correctWords.Add(string.Empty);
        }

        for (int i = 0; i < levelData.correctWords.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelData.correctWords[i] = EditorGUILayout.TextField(
                new GUIContent("Correct Word " + (i + 1), "Enter a correct word for this level."),
                levelData.correctWords[i]
            );

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                levelData.correctWords.RemoveAt(i);
                break;  // Exit loop to prevent index errors after deletion
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
