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
            EventManager.OnAllCorrectWordsSelected += characterMovement.StartMoving;
        }

        private void OnDisable()
        {
            EventManager.OnAllCorrectWordsSelected -= characterMovement.StartMoving;
        }
    }
}
