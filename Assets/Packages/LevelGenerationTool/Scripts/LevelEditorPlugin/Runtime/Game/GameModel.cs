namespace LevelEditorPlugin.Runtime
{
    using System.Collections.Generic;

    public class GameModel
    {
        public HashSet<string> SelectedCorrectWords { get; private set; } = new HashSet<string>();
        public HashSet<string> RequiredCorrectWords { get; private set; }

        public GameModel(List<string> correctWords)
        {
            RequiredCorrectWords = new HashSet<string>(correctWords);
        }

        public bool IsWordCorrect(string word)
        {
            return RequiredCorrectWords.Contains(word);
        }

        public bool AddSelectedWord(string word)
        {
            return SelectedCorrectWords.Add(word);
        }

        public bool AllWordsSelected()
        {
            return SelectedCorrectWords.Count == RequiredCorrectWords.Count;
        }
    }
}
