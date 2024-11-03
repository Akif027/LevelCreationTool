using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
   [Header("UI Components")]
   [SerializeField] private Transform wordSetParent;
   [SerializeField] private GameObject levelUI;
   [SerializeField] private Button nextLevelButton;

   [Header("Level Settings")]
   [Tooltip("The integer value used to load LevelData by index.")]
   [SerializeField] private int levelIndex = 0;

   [SerializeField] GameObject animatedInstance;
   private LevelData currentLevelData;
   private HashSet<string> selectedCorrectWords;
   private HashSet<string> requiredCorrectWords;

   private void Start()
   {
      SetupLevel();
   }

   private void SetupLevel()
   {
      nextLevelButton.gameObject.SetActive(false);
      if (levelUI != null) levelUI.SetActive(true);
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

   private void OnEnable()
   {
      EventManager.OnCorrectWordSelected += HandleCorrectWord;
      EventManager.OnIncorrectWordSelected += HandleIncorrectWord;
      EventManager.OnAllCorrectWordsSelected += HandleLevelComplete;
   }

   private void OnDisable()
   {
      EventManager.OnCorrectWordSelected -= HandleCorrectWord;
      EventManager.OnIncorrectWordSelected -= HandleIncorrectWord;
      EventManager.OnAllCorrectWordsSelected -= HandleLevelComplete;
   }

   private void HandleCorrectWord()
   {
      AudioManager.Instance.PlaySFXByName("CorrectWord");
   }

   private void HandleIncorrectWord()
   {
      AudioManager.Instance.PlaySFXByName("IncorrectWord");
   }

   private void HandleLevelComplete()
   {
      StartCoroutine(DelayedHideLevelUI());
      if (currentLevelData.animatedSceneOnCompletion)
      {
         // var animatedInstance = Instantiate(currentLevelData.animatedScenePrefab);
         animatedInstance.SetActive(true);
      }
      Debug.Log("All correct words selected! Level complete.");
   }

   private IEnumerator DelayedHideLevelUI()
   {
      yield return new WaitForSeconds(1f);
      if (levelUI != null) levelUI.SetActive(false);
      nextLevelButton.gameObject.SetActive(true);
   }

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

   private void ResetForNewLevel()
   {
      selectedCorrectWords.Clear();
      requiredCorrectWords.Clear();
   }
}
