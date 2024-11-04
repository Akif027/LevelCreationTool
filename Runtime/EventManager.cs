namespace LevelEditorPlugin.Runtime
{
    using System;
    using UnityEngine;

    public static class EventManager
    {
        public static event Action OnCorrectWordSelected;
        public static event Action OnIncorrectWordSelected;
        public static event Action OnAllCorrectWordsSelected;

        public static void TriggerCorrectWordSelected()
        {
            Debug.Log("Triggering CorrectWordSelected");
            OnCorrectWordSelected?.Invoke();
        }

        public static void TriggerIncorrectWordSelected()
        {
            Debug.Log("Triggering IncorrectWordSelected");
            OnIncorrectWordSelected?.Invoke();
        }

        public static void TriggerAllCorrectWordsSelected()
        {
            Debug.Log("Triggering AllCorrectWordsSelected");
            OnAllCorrectWordsSelected?.Invoke();
        }
    }
}
