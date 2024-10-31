using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // public LevelData currentLevel;  // Reference to the current level data
    // public Text[] wordButtons;      // UI Buttons for words
    // public Animator animator;       // Animator for triggering animations

    // private void Start()
    // {
    //     LoadLevel();
    // }

    // private void LoadLevel()
    // {
    //     if (currentLevel == null)
    //     {
    //         Debug.LogError("No Level Data assigned!");
    //         return;
    //     }

    //     // Load words into UI based on available prefabs
    //     for (int i = 0; i < wordButtons.Length && i < currentLevel.prefabOptions.Length; i++)
    //     {
    //         wordButtons[i].text = currentLevel.prefabOptions[i].prefabName;
    //     }
    // }

    // public void OnWordSelected(string word)
    // {
    //     bool isCorrect = false;

    //     // Check if the selected word matches any of the correct prefab options
    //     foreach (TileData prefabData in currentLevel.prefabOptions)
    //     {
    //         if (prefabData.prefabName == word && prefabData.isCorrectPrefab)
    //         {
    //             isCorrect = true;
    //             break;
    //         }
    //     }

    //     if (isCorrect)
    //     {
    //         TriggerAnimation();
    //         Debug.Log("Correct prefab selected!");
    //     }
    //     else
    //     {
    //         Debug.Log("Incorrect prefab.");
    //     }
    // }

    // private void TriggerAnimation()
    // {
    //     if (animator != null)
    //     {
    //         animator.SetTrigger("PlayAnimation");
    //     }
    // }
}
