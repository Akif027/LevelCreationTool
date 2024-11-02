using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
   [SerializeField] private LevelData levelData;
   [SerializeField] private Transform wordSetParent;

   private HashSet<string> selectedCorrectWords;
   private HashSet<string> requiredCorrectWords;
   public GameObject animatedSceneInstance;
   public Transform OnCompleteLevelPrefabPos;

   private void Start()
   {
      InitializeGame();
   }

   private void InitializeGame()
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
            EventManager.CorrectWordSelected();
            selectedButton.interactable = false;

            if (selectedCorrectWords.Count == requiredCorrectWords.Count)
            {
               EventManager.AllCorrectWordsSelected();
            }
         }
      }
      else
      {
         EventManager.IncorrectWordSelected();
      }
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
      Debug.Log("Correct word selected! Performing correct word actions.");
   }

   private void HandleIncorrectWord()
   {
      Debug.Log("Incorrect word selected! Performing incorrect word actions.");
   }

   private void HandleLevelComplete()
   {
      if (animatedSceneInstance != null)
      {
         animatedSceneInstance.SetActive(levelData.animatedSceneOnCompletion);
      }

      Debug.Log("All correct words selected! Level complete.");

      // Instantiate success prefab at the specified position if assigned
      if (levelData.successPrefab != null && OnCompleteLevelPrefabPos != null)
      {
         Instantiate(levelData.successPrefab, OnCompleteLevelPrefabPos.position, Quaternion.identity);
      }
      else
      {
         Debug.LogWarning("Success prefab or completion position is not set in LevelData.");
      }
   }
}
