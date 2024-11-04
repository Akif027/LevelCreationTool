namespace LevelEditorPlugin.Runtime
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Level/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Information")]
        public int levelNum;
        public string levelName;
        public Sprite backgroundImg;

        [Header("Question and Words")]
        public string questionText;
        public List<string> words = new List<string>();
        public List<string> correctWords = new List<string>();

        [Header("Prefabs and Effects")]
        public GameObject wordButtonPrefab;
        public GameObject animatedScenePrefab;
        public bool animatedSceneOnCompletion = true;
        public AnimationClip[] successAnimations;

        [Header("Environment Settings")]
        public EnvironmentData[] environmentOptions;
        public List<PlacedEnvironment> placedEnvironments = new List<PlacedEnvironment>();

        [Header("Particle Effects")]
        public List<ParticleEntry> particlePrefabs = new List<ParticleEntry>();

        [Header("Sound Effects")]
        public List<SoundEntry> soundEffects = new List<SoundEntry>();

        private Dictionary<string, GameObject> particleDictionary;
        private Dictionary<string, AudioClip> soundDictionary;

        [System.Serializable]
        public class PlacedEnvironment
        {
            public EnvironmentData environmentData;
            public Vector3 position;
            public Quaternion rotation;
        }

        [System.Serializable]
        public class ParticleEntry
        {
            public string name;
            public GameObject prefab;
        }

        [System.Serializable]
        public class SoundEntry
        {
            public string name;
            public AudioClip clip;
        }

        private void OnEnable()
        {
            InitializeDictionaries();
        }

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

        public GameObject GetParticlePrefab(string name) =>
            particleDictionary.TryGetValue(name, out var prefab) ? prefab : null;

        public AudioClip GetSoundClip(string name) =>
            soundDictionary.TryGetValue(name, out var clip) ? clip : null;
    }
}
