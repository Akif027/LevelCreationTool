namespace LevelEditorPlugin.Runtime
{
    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [SerializeField] private AudioSource sfxSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Ensures it persists across scenes
            }
        }

        // Play sound effects by name from the level's audio clip collection
        public void PlaySFXByName(LevelData levelData, string soundName, float volume = 1f)
        {
            if (sfxSource != null && levelData != null)
            {
                AudioClip clip = levelData.GetSoundClip(soundName);
                if (clip != null)
                {
                    sfxSource.PlayOneShot(clip, volume);
                }
                else
                {
                    Debug.LogWarning($"Sound '{soundName}' not found in LevelData.");
                }
            }
        }
    }
}
