using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace LevelEditorPlugin.Runtime
{
   /// <summary>
   /// Controls the main gameplay logic for the level, including level setup,
   /// word selection handling, and progression to the next level.
   /// </summary>
   public class GameController : MonoBehaviour
   {
      [Header("UI Components")]
      [Tooltip("Parent transform containing word buttons.")]
      [SerializeField] private Transform wordSetParent;

      [Tooltip("UI element for the level interface.")]
      [SerializeField] private GameObject levelUI;

      [Tooltip("Button for progressing to the next level.")]
      [SerializeField] private Button nextLevelButton;

      [Header("Level Settings")]
      [Tooltip("The index of the current level to load the correct LevelData.")]
      [SerializeField] private int levelIndex = 0;

      [Tooltip("Instance of the animated scene shown upon level completion.")]
      [SerializeField] private GameObject animatedInstance;

      private LevelData currentLevelData;
      private HashSet<string> selectedCorrectWords;
      private HashSet<string> requiredCorrectWords;

      /// <summary>
      /// Initializes the level at the start of the scene.
      /// </summary>
      private void Start()
      {
         SetupLevel();
      }

      /// <summary>
      /// Sets up the level based on the current LevelData, configuring UI and gameplay elements.
      /// </summary>
      private void SetupLevel()
      {
         nextLevelButton.gameObject.SetActive(false);
         levelUI?.SetActive(true);
         LoadLevelData();

         if (currentLevelData != null)
         {
            InitializeGame(currentLevelData);
         }
         else
         {
            Debug.LogError($"LevelData with index '{levelIndex}' could not be loaded. Ensure it exists in Resources.");
         }

         nextLevelButton.onClick.AddListener(NextLevel);
      }

      /// <summary>
      /// Loads the LevelData asset for the current level.
      /// </summary>
      private void LoadLevelData()
      {
         var allLevels = Resources.LoadAll<LevelData>("");
         foreach (var levelData in allLevels)
         {
            if (levelData.levelNum == levelIndex)
            {
               currentLevelData = levelData;
               Debug.Log($"Loaded LevelData: {currentLevelData.levelName} with level index {levelIndex}");
               return;
            }
         }
         Debug.LogWarning($"No LevelData found with level index {levelIndex} in Resources.");
      }

      /// <summary>
      /// Initializes the gameplay elements, including setting up word buttons and required words.
      /// </summary>
      /// <param name="levelData">The LevelData asset to use for this level.</param>
      private void InitializeGame(LevelData levelData)
      {
         selectedCorrectWords = new HashSet<string>();
         requiredCorrectWords = new HashSet<string>(levelData.correctWords);

         foreach (Transform child in wordSetParent)
         {
            Button button = child.GetComponent<Button>();
            TextMeshProUGUI wordText = child.GetComponentInChildren<TextMeshProUGUI>();

            if (button != null && wordText != null)
            {
               string word = wordText.text;
               button.onClick.AddListener(() => OnWordSelected(word, button));
            }
         }
      }

      /// <summary>
      /// Handles word selection, checking if the selected word is correct and triggering appropriate events.
      /// </summary>
      /// <param name="selectedWord">The word selected by the player.</param>
      /// <param name="selectedButton">The button associated with the selected word.</param>
      private void OnWordSelected(string selectedWord, Button selectedButton)
      {
         if (requiredCorrectWords.Contains(selectedWord))
         {
            if (selectedCorrectWords.Add(selectedWord))
            {
               PlayEffect("Pop", selectedButton.transform.position);
               selectedButton.interactable = false;

               if (selectedCorrectWords.Count == requiredCorrectWords.Count)
               {
                  EventManager.AllCorrectWordsSelected();
               }
            }
            EventManager.CorrectWordSelected();
         }
         else
         {
            StartCoroutine(ShakeButton(selectedButton));
            EventManager.IncorrectWordSelected();
         }
      }

      /// <summary>
      /// Plays a particle effect at the specified position.
      /// </summary>
      /// <param name="particleName">The name of the particle effect to play.</param>
      /// <param name="position">The position to play the effect at.</param>
      private void PlayEffect(string particleName, Vector3 position)
      {
         var particlePrefab = currentLevelData.GetParticlePrefab(particleName);
         if (particlePrefab != null)
         {
            Instantiate(particlePrefab, position, Quaternion.identity);
         }
         else
         {
            Debug.LogWarning($"Particle '{particleName}' not found in LevelData.");
         }
      }

      /// <summary>
      /// Shakes a button to indicate an incorrect selection.
      /// </summary>
      /// <param name="button">The button to shake.</param>
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

      /// <summary>
      /// Subscribes to events triggered by word selection.
      /// </summary>
      private void OnEnable()
      {
         EventManager.OnCorrectWordSelected += HandleCorrectWord;
         EventManager.OnIncorrectWordSelected += HandleIncorrectWord;
         EventManager.OnAllCorrectWordsSelected += HandleLevelComplete;
      }

      /// <summary>
      /// Unsubscribes from events when the script is disabled.
      /// </summary>
      private void OnDisable()
      {
         EventManager.OnCorrectWordSelected -= HandleCorrectWord;
         EventManager.OnIncorrectWordSelected -= HandleIncorrectWord;
         EventManager.OnAllCorrectWordsSelected -= HandleLevelComplete;
      }

      /// <summary>
      /// Plays a sound effect when a correct word is selected.
      /// </summary>
      private void HandleCorrectWord()
      {
         AudioManager.Instance.PlaySFXByName("CorrectWord");
      }

      /// <summary>
      /// Plays a sound effect when an incorrect word is selected.
      /// </summary>
      private void HandleIncorrectWord()
      {
         AudioManager.Instance.PlaySFXByName("IncorrectWord");
      }

      /// <summary>
      /// Handles level completion, including showing animations and updating UI.
      /// </summary>
      private void HandleLevelComplete()
      {
         StartCoroutine(DelayedHideLevelUI());
         if (currentLevelData.animatedSceneOnCompletion)
         {
            animatedInstance?.SetActive(true);
         }
         Debug.Log("All correct words selected! Level complete.");
      }

      /// <summary>
      /// Delays hiding the level UI upon completion.
      /// </summary>
      private IEnumerator DelayedHideLevelUI()
      {
         yield return new WaitForSeconds(1f);
         levelUI?.SetActive(false);
         nextLevelButton.gameObject.SetActive(true);
      }

      /// <summary>
      /// Loads the next level scene if available, or logs an error if no further levels exist.
      /// </summary>
      public void NextLevel()
      {
         int currentIndex = SceneManager.GetActiveScene().buildIndex;
         int nextIndex = currentIndex + 1;

         if (nextIndex < SceneManager.sceneCountInBuildSettings)
         {
            SceneManager.LoadScene(nextIndex);
         }
         else
         {
            Debug.LogError("No more levels available.");
         }
         ResetForNewLevel();
      }

      /// <summary>
      /// Resets level-specific data for the next level.
      /// </summary>
      private void ResetForNewLevel()
      {
         selectedCorrectWords.Clear();
         requiredCorrectWords.Clear();
      }
   }
}
