using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;  // Name of the level

    // Words and Correct Answers
    public List<string> words = new List<string>(); // List of all possible words
    public List<string> correctWords = new List<string>(); // List of correct words

    // Background and Question
    public Sprite backgroundImg;  // Background image for the level
    public string questionText;  // Question text for the level

    // WordButton prefab reference
    public GameObject wordButtonPrefab;  // Reference to the WordButton prefab

    // Animated Scene and Success Animations
    public GameObject animatedScenePrefab;  // Reference to animated scene or GIF as a prefab
    public bool animatedSceneOnCompletion = true;  // Toggle for triggering animated scene on completion
    public AnimationClip[] successAnimations;  // Actions/animations triggered when the level is solved
    public GameObject successPrefab;  // The prefab to instantiate on level success
    // Environment Data
    public EnvironmentData[] environmentOptions;  // Available environment types for this level
    public List<PlacedEnvironment> placedEnvironments = new List<PlacedEnvironment>();  // List of placed environments

    // Class to represent placed environments in the level
    [System.Serializable]
    public class PlacedEnvironment
    {
        public EnvironmentData environmentData;  // The environment data used
        public Vector3 position;  // Position where it was placed
        public Quaternion rotation;  // Rotation of the placed element
    }
}
