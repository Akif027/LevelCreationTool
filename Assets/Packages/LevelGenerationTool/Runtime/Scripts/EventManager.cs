using System;

namespace LevelEditorPlugin.Runtime
{
    /// <summary>
    /// Static event manager to handle and broadcast important game events, such as
    /// selecting correct or incorrect words, and completing all correct word selections.
    /// Other scripts can subscribe to these events to respond accordingly.
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// Invoked when a correct word is selected by the player.
        /// </summary>
        public static event Action OnCorrectWordSelected;

        /// <summary>
        /// Invoked when an incorrect word is selected by the player.
        /// </summary>
        public static event Action OnIncorrectWordSelected;

        /// <summary>
        /// Invoked when all correct words have been selected, signaling level completion.
        /// </summary>
        public static event Action OnAllCorrectWordsSelected;

        /// <summary>
        /// Triggers the OnCorrectWordSelected event.
        /// </summary>
        public static void CorrectWordSelected()
        {
            OnCorrectWordSelected?.Invoke();
        }

        /// <summary>
        /// Triggers the OnIncorrectWordSelected event.
        /// </summary>
        public static void IncorrectWordSelected()
        {
            OnIncorrectWordSelected?.Invoke();
        }

        /// <summary>
        /// Triggers the OnAllCorrectWordsSelected event, used to signal level completion.
        /// </summary>
        public static void AllCorrectWordsSelected()
        {
            OnAllCorrectWordsSelected?.Invoke();
        }
    }
}
