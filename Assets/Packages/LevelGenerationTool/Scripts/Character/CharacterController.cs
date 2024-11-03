namespace LevelEditorPlugin.Runtime
{
    using UnityEngine;

    public class CharacterController : MonoBehaviour
    {
        private CharacterMovement characterMovement;

        private void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
        }

        private void OnEnable()
        {
            // Directly add listener without null check
            EventManager.OnAllCorrectWordsSelected += StartMoving;
        }

        private void OnDisable()
        {
            // Directly remove listener without null check
            EventManager.OnAllCorrectWordsSelected -= StartMoving;
        }

        // This method is called when all correct words are selected
        private void StartMoving()
        {
            if (characterMovement != null)
            {
                characterMovement.StartMoving(); // Assuming CharacterMovement has a StartMoving method
            }
            else
            {
                Debug.LogWarning("CharacterMovement component is missing on this GameObject.");
            }
        }
    }
}
