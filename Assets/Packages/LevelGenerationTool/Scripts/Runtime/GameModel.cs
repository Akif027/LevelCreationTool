using System.Collections.Generic;

public class GameModel
{
    private HashSet<string> correctWords;
    private HashSet<string> selectedWords;

    public GameModel(List<string> correctWordsList)
    {
        correctWords = new HashSet<string>(correctWordsList, System.StringComparer.OrdinalIgnoreCase); // Case-insensitive
        selectedWords = new HashSet<string>();
    }

    public bool IsWordCorrect(string word)
    {
        return correctWords.Contains(word);
    }

    public bool AddSelectedWord(string word)
    {
        if (correctWords.Contains(word) && !selectedWords.Contains(word))
        {
            selectedWords.Add(word);
            return true;
        }
        return false;
    }

    public bool AllWordsSelected()
    {
        return selectedWords.Count == correctWords.Count;
    }
}
