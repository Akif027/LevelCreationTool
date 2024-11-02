using UnityEngine;

public class CharacterController : CharacterMovement
{
    private void OnEnable()
    {
        // Subscribe to the event
        EventManager.OnAllCorrectWordsSelected += StartMoving;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        EventManager.OnAllCorrectWordsSelected -= StartMoving;
    }
}
