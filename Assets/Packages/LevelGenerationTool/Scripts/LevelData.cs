using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;                                // Name of the level

    // Words and Correct Answers
    public List<WordData> words = new List<WordData>();     // List of words for the level
    public WordData[] correctWords;                         // Correct words needed to solve the level

    // Background and Question
    public Sprite backgroundImg;                            // Background image for the level
    public string questionText;
    // Question text for the level
    // WordButton prefab reference
    public GameObject wordButtonPrefab;                     // Reference to the WordButton prefab
    // Animated Scene and Success Animations
    public GameObject animatedScenePrefab;                  // Reference to animated scene or GIF as a prefab
    public AnimationClip[] successAnimations;               // Actions/animations triggered when the level is solved

    // Environment Data
    public EnvironmentData[] environmentOptions;            // Available environment types for this level
    public List<PlacedEnvironment> placedEnvironments = new List<PlacedEnvironment>(); // List of placed environments

    // Class to represent placed environments in the level
    [System.Serializable]
    public class PlacedEnvironment
    {
        public EnvironmentData environmentData;             // The environment data used
        public Vector3 position;                            // Position where it was placed
        public Quaternion rotation;                         // Rotation of the placed element
    }
}
