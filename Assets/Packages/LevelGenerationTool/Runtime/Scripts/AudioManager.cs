using UnityEngine;

namespace LevelEditorPlugin.Runtime
{
    /// <summary>
    /// Manages and plays sound effects during gameplay. Uses a singleton pattern to
    /// ensure only one instance exists and persists across scenes. Looks up sound
    /// clips by name from the current LevelData.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        // Singleton instance
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [Tooltip("Audio source used for playing sound effects.")]
        [SerializeField] private AudioSource sfxSource;

        [Header("Level Data")]
        [Tooltip("Reference to the LevelData asset containing audio clips for this level.")]
        public LevelData currentLevelData;

        /// <summary>
        /// Ensures that only one instance of AudioManager exists and makes it persistent.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);  // Persist across scenes
            }
        }

        /// <summary>
        /// Plays a sound effect by name from the current LevelData.
        /// </summary>
        /// <param name="soundName">The name of the sound effect to play.</param>
        /// <param name="volume">The volume at which to play the sound (default is 1.0).</param>
        public void PlaySFXByName(string soundName, float volume = 1f)
        {
            if (sfxSource != null && currentLevelData != null)
            {
                // Retrieve the AudioClip from LevelData
                AudioClip clip = currentLevelData.GetSoundClip(soundName);
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
