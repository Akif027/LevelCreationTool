using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public EnvironmentData[] environmentOptions;  // Available environment types for this level
    public List<PlacedEnvironment> placedEnvironments = new List<PlacedEnvironment>(); // List of placed environments

    [System.Serializable]
    public class PlacedEnvironment
    {
        public EnvironmentData environmentData; // The environment data used
        public Vector3 position;                // Position where it was placed
        public Quaternion rotation;             // Rotation of the placed element
    }
}
