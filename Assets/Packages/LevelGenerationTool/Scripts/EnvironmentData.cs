using UnityEngine;

public enum EnvironmentType
{
    Ground,
    Obstacle,
    Decoration,
    Water,
    // Add more as needed
}

[CreateAssetMenu(fileName = "NewEnvironmentData", menuName = "Environment/Environment Data")]
public class EnvironmentData : ScriptableObject
{
    public string environmentName;      // Name of the environment element for identification
    public Sprite icon;                 // Icon to display in the Level Editor window
    public GameObject prefab;           // Reference to the prefab itself
    public EnvironmentType environmentType; // Type of environment (e.g., Ground, Obstacle)
}
