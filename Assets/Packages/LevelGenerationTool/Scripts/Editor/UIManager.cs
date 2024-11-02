using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager
{
    private Image backgroundImgUI;
    private TextMeshProUGUI questionTextUI;

    public void DrawLevelSettings(LevelData levelData)
    {
        GUILayout.Label("Level Name", EditorStyles.boldLabel);
        levelData.levelName = EditorGUILayout.TextField("Level Name", levelData.levelName);

        GUILayout.Label("Background Image", EditorStyles.boldLabel);
        levelData.backgroundImg = (Sprite)EditorGUILayout.ObjectField("Background Image", levelData.backgroundImg, typeof(Sprite), false);

        if (GUILayout.Button("Apply Background"))
        {
            ApplyBackground(levelData.backgroundImg);
        }

        GUILayout.Label("Question Text", EditorStyles.boldLabel);
        levelData.questionText = EditorGUILayout.TextField("Question", levelData.questionText);

        DrawAnimatedScene(levelData);
        DrawSuccessAnimations(levelData);
        DrawWordManagement(levelData);
        DrawCorrectWords(levelData);
        DrawSuccessPrefab(levelData);
    }

    private void ApplyBackground(Sprite backgroundImg)
    {
        GameObject bgObject = GameObject.FindWithTag("Background");

        if (bgObject == null)
        {
            bgObject = new GameObject("BackgroundImage") { tag = "Background" };
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas != null) bgObject.transform.SetParent(canvas.transform, false);

            backgroundImgUI = bgObject.AddComponent<Image>();
            RectTransform rectTransform = bgObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
        else
        {
            backgroundImgUI = bgObject.GetComponent<Image>();
        }

        if (backgroundImgUI != null && backgroundImg != null)
        {
            backgroundImgUI.sprite = backgroundImg;
        }
    }

    public void DrawAnimatedScene(LevelData levelData)
    {
        GUILayout.Label("Animated Scene Prefab", EditorStyles.boldLabel);
        levelData.animatedScenePrefab = (GameObject)EditorGUILayout.ObjectField("Animated Scene Prefab", levelData.animatedScenePrefab, typeof(GameObject), false);
    }

    public void DrawSuccessAnimations(LevelData levelData)
    {
        GUILayout.Label("Success Animations", EditorStyles.boldLabel);
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Animated Scene On Completion");
            levelData.animatedSceneOnCompletion = EditorGUILayout.Toggle(levelData.animatedSceneOnCompletion);
        }

        int animationCount = EditorGUILayout.IntField("Add Animation Clips", levelData.successAnimations.Length);
        if (animationCount != levelData.successAnimations.Length)
        {
            System.Array.Resize(ref levelData.successAnimations, animationCount);
        }

        for (int i = 0; i < animationCount; i++)
        {
            levelData.successAnimations[i] = (AnimationClip)EditorGUILayout.ObjectField("Animation " + (i + 1), levelData.successAnimations[i], typeof(AnimationClip), false);
        }
    }

    public void DrawWordManagement(LevelData levelData)
    {
        GUILayout.Label("Manage Words", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Word"))
        {
            levelData.words.Add(string.Empty);
        }

        for (int i = 0; i < levelData.words.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelData.words[i] = EditorGUILayout.TextField("Word", levelData.words[i]);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                levelData.words.RemoveAt(i);
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
            levelData.correctWords[i] = EditorGUILayout.TextField("Correct Word", levelData.correctWords[i]);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                levelData.correctWords.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    public void DrawSuccessPrefab(LevelData levelData)
    {
        GUILayout.Label("Success Prefab and Position", EditorStyles.boldLabel);
        levelData.successPrefab = (GameObject)EditorGUILayout.ObjectField("Success Prefab", levelData.successPrefab, typeof(GameObject), false);

    }
}
