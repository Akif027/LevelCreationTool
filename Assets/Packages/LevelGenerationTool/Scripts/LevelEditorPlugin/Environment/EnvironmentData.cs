namespace LevelEditorPlugin.Runtime
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewEnvironmentData", menuName = "Environment/Environment Data")]
    public class EnvironmentData : ScriptableObject
    {
        [Header("Environment Information")]
        [Tooltip("The name of the environment element for identification purposes.")]
        public string environmentName;

        [Tooltip("The icon that represents this environment element in the Level Editor.")]
        public Sprite icon;

        [Tooltip("The prefab for this environment element to be placed in the level.")]
        public GameObject prefab;

        [Tooltip("The type of environment (e.g., Ground, Obstacle, Decoration, Water).")]
        public EnvironmentType environmentType;
    }

    // Enum representing the various types of environment elements.
    public enum EnvironmentType
    {
        Ground,
        Obstacle,
        Decoration,
        Water
    }
}
