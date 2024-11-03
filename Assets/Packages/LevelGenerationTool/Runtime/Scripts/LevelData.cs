using System.Collections.Generic;
using UnityEngine;

namespace LevelEditorPlugin.Runtime
{
    /// <summary>
    /// A ScriptableObject that stores data for a single level, including settings,
    /// environment configurations, word lists, particle effects, and sound effects.
    /// </summary>
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Level/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Information")]
        [Tooltip("The unique number for this level.")]
        public int levelNum;

        [Tooltip("The name of the level.")]
        public string levelName;

        [Tooltip("The background image for the level.")]
        public Sprite backgroundImg;

        [Header("Question and Words")]
        [Tooltip("The question or prompt to display in this level.")]
        public string questionText;

        [Tooltip("List of words associated with this level.")]
        public List<string> words = new List<string>();

        [Tooltip("List of correct words to answer the question or prompt.")]
        public List<string> correctWords = new List<string>();

        [Header("Prefabs and Effects")]
        [Tooltip("Prefab for the word buttons used in the level.")]
        public GameObject wordButtonPrefab;

        [Tooltip("Prefab for the animated scene that plays upon level completion.")]
        public GameObject animatedScenePrefab;

        [Tooltip("If true, plays the animated scene on level completion.")]
        public bool animatedSceneOnCompletion = true;

        [Tooltip("Animations that play when the player completes the level successfully.")]
        public AnimationClip[] successAnimations;

        [Header("Environment Settings")]
        [Tooltip("Available environment options for this level.")]
        public EnvironmentData[] environmentOptions;

        [Tooltip("List of environments placed in this level, including position and rotation data.")]
        public List<PlacedEnvironment> placedEnvironments = new List<PlacedEnvironment>();

        [Header("Particle Effects")]
        [Tooltip("List of particle system prefabs to use in this level.")]
        public List<ParticleEntry> particlePrefabs = new List<ParticleEntry>();

        // Dictionary to retrieve particle prefabs by name (for runtime access)
        private Dictionary<string, GameObject> particleDictionary;

        [Header("Sound Effects")]
        [Tooltip("List of sound effects to use in this level.")]
        public List<SoundEntry> soundEffects = new List<SoundEntry>();

        // Dictionary to retrieve sound clips by name (for runtime access)
        private Dictionary<string, AudioClip> soundDictionary;

        /// <summary>
        /// Represents an environment object placed in the level, including its
        /// associated data, position, and rotation.
        /// </summary>
        [System.Serializable]
        public class PlacedEnvironment
        {
            [Tooltip("The environment data associated with this placement.")]
            public EnvironmentData environmentData;

            [Tooltip("The position of the environment object in the level.")]
            public Vector3 position;

            [Tooltip("The rotation of the environment object in the level.")]
            public Quaternion rotation;
        }

        /// <summary>
        /// Represents a particle effect entry, including a name and a prefab.
        /// </summary>
        [System.Serializable]
        public class ParticleEntry
        {
            [Tooltip("The name of the particle effect.")]
            public string name;

            [Tooltip("The prefab for the particle effect.")]
            public GameObject prefab;
        }

        /// <summary>
        /// Represents a sound effect entry, including a name and an AudioClip.
        /// </summary>
        [System.Serializable]
        public class SoundEntry
        {
            [Tooltip("The name of the sound effect.")]
            public string name;

            [Tooltip("The AudioClip for the sound effect.")]
            public AudioClip clip;
        }

        /// <summary>
        /// Initializes dictionaries for quick lookup of particle and sound entries by name.
        /// </summary>
        private void OnEnable()
        {
            InitializeDictionaries();
        }

        /// <summary>
        /// Populates the particle and sound dictionaries for runtime access.
        /// </summary>
        private void InitializeDictionaries()
        {
            particleDictionary = new Dictionary<string, GameObject>();
            foreach (var entry in particlePrefabs)
            {
                if (!particleDictionary.ContainsKey(entry.name) && entry.prefab != null)
                {
                    particleDictionary.Add(entry.name, entry.prefab);
                }
            }

            soundDictionary = new Dictionary<string, AudioClip>();
            foreach (var entry in soundEffects)
            {
                if (!soundDictionary.ContainsKey(entry.name) && entry.clip != null)
                {
                    soundDictionary.Add(entry.name, entry.clip);
                }
            }
        }

        /// <summary>
        /// Retrieves a particle prefab by name.
        /// </summary>
        /// <param name="name">The name of the particle effect.</param>
        /// <returns>The associated particle prefab, or null if not found.</returns>
        public GameObject GetParticlePrefab(string name) =>
            particleDictionary.TryGetValue(name, out var prefab) ? prefab : null;

        /// <summary>
        /// Retrieves a sound clip by name.
        /// </summary>
        /// <param name="name">The name of the sound effect.</param>
        /// <returns>The associated AudioClip, or null if not found.</returns>
        public AudioClip GetSoundClip(string name) =>
            soundDictionary.TryGetValue(name, out var clip) ? clip : null;
    }
}
