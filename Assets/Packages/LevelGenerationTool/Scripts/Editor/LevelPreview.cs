using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelPreview
{
    private Transform wordSetParent;
    private GameObject animatedSceneInstance; // Holds the instance of the animated scene prefab

    public void DrawPreviewButton(LevelData levelData)
    {
        if (GUILayout.Button("Preview/Update Level"))
        {
            PreviewLevel(levelData);
        }
    }

    private void PreviewLevel(LevelData levelData)
    {
        Debug.Log("Previewing Level: " + levelData.levelName);

        // Apply question text and instantiate words
        ApplyQuestionText(levelData.questionText);
        InstantiateWords(levelData);

        // Instantiate Animated Scene Prefab if assigned
        InstantiateAnimatedScene(levelData);
    }

    private void ApplyQuestionText(string questionText)
    {
        var questionTextUI = GameObject.Find("Question").GetComponent<TextMeshProUGUI>();
        if (questionTextUI != null)
        {
            questionTextUI.text = questionText;
        }
        else
        {
            Debug.LogWarning("No GameObject named 'Question' with a TextMeshProUGUI component found in the scene.");
        }
    }

    private void InstantiateWords(LevelData levelData)
    {
        wordSetParent = GameObject.Find("Wordset")?.transform;
        if (wordSetParent == null)
        {
            Debug.LogError("Wordset GameObject not found.");
            return;
        }

        // Clear all existing child objects in Wordset
        for (int i = wordSetParent.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(wordSetParent.GetChild(i).gameObject);
        }

        foreach (string word in levelData.words)
        {
            GameObject wordInstance = Object.Instantiate(levelData.wordButtonPrefab, wordSetParent);
            var wordText = wordInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (wordText != null)
            {
                wordText.text = word;
            }
        }
    }

    private void InstantiateAnimatedScene(LevelData levelData)
    {
        // Check if the animated scene prefab is assigned
        if (levelData.animatedScenePrefab != null)
        {
            // Destroy any existing instance first
            if (animatedSceneInstance != null)
            {
                Object.DestroyImmediate(animatedSceneInstance);
            }

            // Instantiate the animated scene prefab
            animatedSceneInstance = Object.Instantiate(levelData.animatedScenePrefab);
            animatedSceneInstance.name = "AnimatedScenePreview";  // Rename for clarity in hierarchy

            // Optionally, position it within the scene as needed
            animatedSceneInstance.transform.position = Vector3.zero;  // Example: center of the scene
        }
        else
        {
            Debug.LogWarning("No Animated Scene Prefab assigned in LevelData.");
        }
    }
}
