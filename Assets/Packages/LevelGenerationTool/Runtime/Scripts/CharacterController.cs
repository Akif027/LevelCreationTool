using UnityEngine;

namespace LevelEditorPlugin.Runtime
{
    /// <summary>
    /// Controls the character's behavior, starting movement when the correct game
    /// event is triggered. Subscribes to the EventManager to respond to level events.
    /// </summary>
    public class CharacterController : CharacterMovement
    {
        /// <summary>
        /// Subscribes to the OnAllCorrectWordsSelected event to start character movement.
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnAllCorrectWordsSelected += StartMoving;
        }

        /// <summary>
        /// Unsubscribes from the OnAllCorrectWordsSelected event when the script is disabled.
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnAllCorrectWordsSelected -= StartMoving;
        }
    }
}
