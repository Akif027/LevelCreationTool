using UnityEngine;

public static class EventManager
{
    // Define events using C# event delegates
    public static event System.Action OnCorrectWordSelected;
    public static event System.Action OnIncorrectWordSelected;
    public static event System.Action OnAllCorrectWordsSelected;

    // Methods to trigger events
    public static void CorrectWordSelected()
    {
        Debug.Log("Correct word selected event triggered.");
        OnCorrectWordSelected?.Invoke();
    }

    public static void IncorrectWordSelected()
    {
        Debug.Log("Incorrect word selected event triggered.");
        OnIncorrectWordSelected?.Invoke();
    }

    public static void AllCorrectWordsSelected()
    {
        Debug.Log("All correct words selected event triggered.");
        OnAllCorrectWordsSelected?.Invoke();
    }
}
