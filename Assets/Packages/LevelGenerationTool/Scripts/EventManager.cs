using UnityEngine;
using System;

public static class EventManager
{
    public static event Action OnCorrectWordSelected;
    public static event Action OnIncorrectWordSelected;
    public static event Action OnAllCorrectWordsSelected;

    public static void CorrectWordSelected()
    {
        OnCorrectWordSelected?.Invoke();
    }

    public static void IncorrectWordSelected()
    {
        OnIncorrectWordSelected?.Invoke();
    }

    public static void AllCorrectWordsSelected()
    {
        OnAllCorrectWordsSelected?.Invoke();
    }
}
