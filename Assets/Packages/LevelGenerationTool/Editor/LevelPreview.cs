using UnityEditor;
using UnityEngine;
using TMPro;
using LevelEditorPlugin.Runtime;

namespace LevelEditorPlugin.Editor
{
    /// <summary>
    /// Provides level preview functionality in the Level Editor, allowing users
    /// to visualize their level configuration in real-time.
    /// </summary>
    public class LevelPreview
    {
        private Transform wordSetParent;
        private GameObject animatedSceneInstance;

        /// <summary>
        /// Draws the Preview button in the editor window, allowing users to update
        /// and preview the current level configuration.
        /// </summary>
        /// <param name="levelData">The LevelData asset being previewed.</param>
        public void DrawPreviewButton(LevelData levelData)
        {
            if (GUILayout.Button("Preview/Update Level"))
            {
                PreviewLevel(levelData);
            }
        }

        /// <summary>
        /// Generates a preview of the level, including question text, words, and animated scenes.
        /// </summary>
        /// <param name="levelData">The LevelData asset being previewed.</param>
        private void PreviewLevel(LevelData levelData)
        {
            Debug.Log($"Previewing Level: {levelData.levelName}");

            // Apply question text and instantiate words
            ApplyQuestionText(levelData.questionText);
            InstantiateWords(levelData);

            // Instantiate Animated Scene Prefab if assigned
            InstantiateAnimatedScene(levelData);
        }

        /// <summary>
        /// Applies the question text to the designated UI element in the scene.
        /// </summary>
        /// <param name="questionText">The question text to display.</param>
        private void ApplyQuestionText(string questionText)
        {
            var questionTextUI = GameObject.Find("Question")?.GetComponent<TextMeshProUGUI>();
            if (questionTextUI != null)
            {
                questionTextUI.text = questionText;
            }
            else
            {
                Debug.LogWarning("No GameObject named 'Question' with a TextMeshProUGUI component found in the scene.");
            }
        }

        /// <summary>
        /// Instantiates the word objects in the scene based on the words list in LevelData.
        /// </summary>
        /// <param name="levelData">The LevelData asset containing the words list.</param>
        private void InstantiateWords(LevelData levelData)
        {
            wordSetParent = GameObject.Find("Wordset")?.transform;
            if (wordSetParent == null)
            {
                Debug.LogError("Wordset GameObject not found.");
                return;
            }

            // Clear existing child objects in Wordset
            foreach (Transform child in wordSetParent)
            {
                Object.DestroyImmediate(child.gameObject);
            }

            // Instantiate each word as a UI button
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

        /// <summary>
        /// Instantiates the animated scene prefab in the scene if assigned in LevelData.
        /// </summary>
        /// <param name="levelData">The LevelData asset containing the animated scene prefab.</param>
        private void InstantiateAnimatedScene(LevelData levelData)
        {
            if (levelData.animatedScenePrefab != null)
            {
                // Destroy any existing instance to prevent duplicates
                if (animatedSceneInstance != null)
                {
                    Object.DestroyImmediate(animatedSceneInstance);
                }

                // Instantiate the animated scene prefab
                animatedSceneInstance = Object.Instantiate(levelData.animatedScenePrefab);
                animatedSceneInstance.name = "AnimatedScenePreview";  // Rename for clarity in hierarchy

                // Optionally, position it within the scene as needed
                animatedSceneInstance.transform.position = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("No Animated Scene Prefab assigned in LevelData.");
            }
        }
    }
}
