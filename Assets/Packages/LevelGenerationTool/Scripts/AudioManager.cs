using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }  // Singleton instance

    [Header("Audio Sources")]

    [SerializeField] private AudioSource sfxSource;    // For sound effects

    [Header("Level Data")]
    public LevelData currentLevelData;  // Reference to the LevelData containing audio clips

    private void Awake()
    {
        // Ensure there's only one instance of AudioManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make persistent across scenes
        }


    }


    // Play a sound effect by name
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
