using UnityEngine;

[CreateAssetMenu(fileName = "NewWordData", menuName = "Level/Word Data")]
public class WordData : ScriptableObject
{
    public string word;
    public bool isCorrectWord; // Indicates if this word is part of the solution
    public Sprite wordIcon;    // Icon or visual representation of the word
}
