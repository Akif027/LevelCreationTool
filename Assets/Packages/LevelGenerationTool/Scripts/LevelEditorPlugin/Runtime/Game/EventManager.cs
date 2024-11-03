namespace LevelEditorPlugin.Runtime
{
    using System;

    public static class EventManager
    {
        public static event Action OnCorrectWordSelected;
        public static event Action OnIncorrectWordSelected;
        public static event Action OnAllCorrectWordsSelected;

        public static void TriggerCorrectWordSelected()
        {
            OnCorrectWordSelected?.Invoke();
        }

        public static void TriggerIncorrectWordSelected()
        {
            OnIncorrectWordSelected?.Invoke();
        }

        public static void TriggerAllCorrectWordsSelected()
        {
            OnAllCorrectWordsSelected?.Invoke();
        }
    }
}
