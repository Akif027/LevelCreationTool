namespace LevelEditorPlugin.Runtime
{
    using UnityEngine;
    using UnityEngine.UI;

    using System.Collections;
    using UnityEngine.SceneManagement;

    public class GameView : MonoBehaviour
    {
        [SerializeField] private Transform wordSetParent;
        public Transform WordSetParent => wordSetParent;

        [SerializeField] private GameObject levelUI;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Text questionText;

        // Initialize word buttons and other UI elements based on the provided LevelData
        public void SetupUI(LevelData levelData)
        {
            questionText.text = levelData.questionText;

            // Clear existing word buttons
            foreach (Transform child in wordSetParent)
            {
                Destroy(child.gameObject);
            }

            // Create word buttons based on the level's word list
            foreach (string word in levelData.words)
            {
                GameObject wordButtonObj = Instantiate(levelData.wordButtonPrefab, wordSetParent);
                var wordText = wordButtonObj.GetComponentInChildren<Text>();
                if (wordText != null)
                {
                    wordText.text = word;
                }
            }

            nextLevelButton.gameObject.SetActive(false);
            nextLevelButton.onClick.AddListener(LoadNextLevel); // Add the listener for the next level button
            levelUI.SetActive(true);
        }

        // Update button visual feedback for correct and incorrect words
        public void ShowCorrectWord(Button button)
        {
            button.interactable = false;
        }

        public void ShowIncorrectWord(Button button)
        {
            StartCoroutine(ShakeButton(button));
        }

        private IEnumerator ShakeButton(Button button)
        {
            Vector3 originalPosition = button.transform.position;
            float shakeDuration = 0.2f;
            float shakeMagnitude = 10f;
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                float offsetX = Random.Range(-0.02f, 0.02f) * shakeMagnitude;
                button.transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            button.transform.position = originalPosition;
        }
        public void PlayEffect(GameObject particlePrefab, Vector3 position)
        {
            if (particlePrefab != null)
            {
                GameObject particleprefab = Instantiate(particlePrefab, position, Quaternion.identity);
                Destroy(particleprefab, 1);
            }
            else
            {
                Debug.LogWarning("Particle prefab is not assigned.");
            }
        }

        // Shows the level completion UI
        public void ShowLevelCompleteUI()
        {
            levelUI.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
        }

        // Method to load the next level

        private void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Check if there's a next level
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {

                Debug.LogWarning("No more levels available. You have completed all levels.");
            }
        }

    }
}
