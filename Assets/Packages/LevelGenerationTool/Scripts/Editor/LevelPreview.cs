using UnityEditor;
using UnityEngine;
using LevelEditorPlugin.Runtime;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LevelEditorPlugin.Editor
{
    public class LevelPreview
    {
        private Transform wordSetParent;
        private GameObject animatedSceneInstance;

        public void DrawPreviewButton(LevelData levelData)
        {
            if (GUILayout.Button("Preview/Update Level"))
            {
                PreviewLevel(levelData);
            }
        }

        private void PreviewLevel(LevelData levelData)
        {
            Debug.Log($"Previewing Level: {levelData.levelName}");

            // Apply question text and instantiate words
            ApplyQuestionText(levelData.questionText);
            InstantiateWords(levelData);

            // Instantiate Animated Scene Prefab if assigned
            InstantiateAnimatedScene(levelData);
        }

        private void ApplyQuestionText(string questionText)
        {
            var questionTextUI = GameObject.Find("Question")?.GetComponent<Text>();
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

            // Clear existing child objects in Wordset by collecting them first
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in wordSetParent)
            {
                children.Add(child.gameObject);
            }

#if UNITY_EDITOR
            // In Editor mode
            foreach (GameObject child in children)
            {
                Undo.DestroyObjectImmediate(child);
            }
#else
    // In Play mode
    foreach (GameObject child in children)
    {
        Destroy(child);
    }
#endif

            // Instantiate each word as a UI button
            foreach (string word in levelData.words)
            {
                GameObject wordInstance = Object.Instantiate(levelData.wordButtonPrefab, wordSetParent);
                var wordText = wordInstance.GetComponentInChildren<Text>();
                if (wordText != null)
                {
                    wordText.text = word;
                }
            }
        }


        private void InstantiateAnimatedScene(LevelData levelData)
        {
            // Check if an object named "AnimatedScenePreview" already exists in the scene
            animatedSceneInstance = GameObject.Find("AnimatedScenePreview");

            if (animatedSceneInstance != null)
            {
                Debug.Log("AnimatedScenePreview already exists in the scene. Skipping instantiation.");
                return;
            }

            if (levelData.animatedScenePrefab != null)
            {
                // Instantiate the animated scene prefab
                animatedSceneInstance = Object.Instantiate(levelData.animatedScenePrefab);
                animatedSceneInstance.name = "AnimatedScenePreview";  // Rename for clarity in the hierarchy

                // Position it within the scene if needed
                animatedSceneInstance.transform.position = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("No Animated Scene Prefab assigned in LevelData.");
            }
        }

    }
}
