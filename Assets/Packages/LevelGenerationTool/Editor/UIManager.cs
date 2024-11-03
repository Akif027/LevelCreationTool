using LevelEditorPlugin.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditorPlugin.Editor
{
    /// <summary>
    /// Manages UI elements in the Level Editor window, including level settings,
    /// word lists, and other level properties.
    /// </summary>
    public class UIManager
    {
        private Image backgroundImgUI;

        /// <summary>
        /// Draws the main UI elements for level settings in the Level Editor window.
        /// </summary>
        /// <param name="levelData">The LevelData asset being edited.</param>
        public void DrawLevelSettings(LevelData levelData)
        {
            GUILayout.Label("Level Settings", EditorStyles.boldLabel);

            // Level Name
            levelData.levelName = EditorGUILayout.TextField(
                new GUIContent("Level Name", "Enter a unique name for this level."),
                levelData.levelName
            );

            // Background Image
            GUILayout.Label("Background Image", EditorStyles.boldLabel);
            levelData.backgroundImg = (Sprite)EditorGUILayout.ObjectField(
                new GUIContent("Background Image", "Assign a background image for the level."),
                levelData.backgroundImg,
                typeof(Sprite),
                false
            );

            if (GUILayout.Button("Apply Background"))
            {
                ApplyBackground(levelData.backgroundImg);
            }

            GUILayout.Space(10);

            // Question Text
            levelData.questionText = EditorGUILayout.TextField(
                new GUIContent("Question Text", "Enter the question text for this level."),
                levelData.questionText
            );

            GUILayout.Space(10);

            // Animated Scene Prefab
            levelData.animatedScenePrefab = (GameObject)EditorGUILayout.ObjectField(
                new GUIContent("Animated Scene Prefab", "Assign a prefab for the animated scene."),
                levelData.animatedScenePrefab,
                typeof(GameObject),
                false
            );

            GUILayout.Space(10);

            // Success Animations
            DrawSuccessAnimations(levelData);

            // Word Management
            DrawWordManagement(levelData);

            // Correct Words Management
            DrawCorrectWords(levelData);
        }

        /// <summary>
        /// Initializes a new LevelData instance with default values for fields.
        /// </summary>
        /// <param name="levelData">The LevelData instance to initialize.</param>
        public void InitializeLevelDataFields(LevelData levelData)
        {
            levelData.words = new System.Collections.Generic.List<string>();
            levelData.correctWords = new System.Collections.Generic.List<string>();
            levelData.successAnimations = new AnimationClip[0];
            levelData.environmentOptions = new EnvironmentData[0];
            levelData.placedEnvironments = new System.Collections.Generic.List<LevelData.PlacedEnvironment>();
            levelData.particlePrefabs = new System.Collections.Generic.List<LevelData.ParticleEntry>();
            levelData.soundEffects = new System.Collections.Generic.List<LevelData.SoundEntry>();

            levelData.levelName = "New Level";
            levelData.animatedSceneOnCompletion = true;
        }

        /// <summary>
        /// Applies the selected background image to the level if a UI background object is found.
        /// </summary>
        /// <param name="backgroundImg">The selected background Sprite.</param>
        private void ApplyBackground(Sprite backgroundImg)
        {
            GameObject bgObject = GameObject.FindWithTag("Background");

            if (bgObject == null)
            {
                Debug.LogWarning("No GameObject with tag 'Background' found in the scene. Background not applied.");
                return;
            }

            if (backgroundImgUI == null)
            {
                backgroundImgUI = bgObject.GetComponent<Image>();
                if (backgroundImgUI == null)
                {
                    backgroundImgUI = bgObject.AddComponent<Image>();
                }
            }

            if (backgroundImg != null)
            {
                backgroundImgUI.sprite = backgroundImg;
            }
            else
            {
                Debug.LogWarning("Background image is null. Please assign a background image.");
            }
        }

        /// <summary>
        /// Draws the success animations field, allowing the user to add and configure animation clips.
        /// </summary>
        /// <param name="levelData">The LevelData asset containing the animations.</param>
        private void DrawSuccessAnimations(LevelData levelData)
        {
            GUILayout.Label("Success Animations", EditorStyles.boldLabel);
            int animationCount = EditorGUILayout.IntField("Add Animation Clips", levelData.successAnimations.Length);
            if (animationCount != levelData.successAnimations.Length)
            {
                System.Array.Resize(ref levelData.successAnimations, animationCount);
            }

            for (int i = 0; i < animationCount; i++)
            {
                levelData.successAnimations[i] = (AnimationClip)EditorGUILayout.ObjectField(
                    new GUIContent($"Animation {i + 1}", "Select an animation clip to play upon level success."),
                    levelData.successAnimations[i],
                    typeof(AnimationClip),
                    false
                );
            }
        }

        /// <summary>
        /// Draws the word management UI, allowing users to add, edit, and remove words for the level.
        /// </summary>
        /// <param name="levelData">The LevelData asset containing the words.</param>
        public void DrawWordManagement(LevelData levelData)
        {
            GUILayout.Label("Manage Words", EditorStyles.boldLabel);

            if (GUILayout.Button("Add New Word"))
            {
                levelData.words.Add(string.Empty);
            }

            for (int i = 0; i < levelData.words.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                levelData.words[i] = EditorGUILayout.TextField(
                    new GUIContent($"Word {i + 1}", "Enter a word for this level."),
                    levelData.words[i]
                );

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    levelData.words.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Draws the correct words management UI, allowing users to designate specific words as correct answers.
        /// </summary>
        /// <param name="levelData">The LevelData asset containing the correct words.</param>
        public void DrawCorrectWords(LevelData levelData)
        {
            GUILayout.Label("Correct Words", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Correct Word"))
            {
                levelData.correctWords.Add(string.Empty);
            }

            for (int i = 0; i < levelData.correctWords.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                levelData.correctWords[i] = EditorGUILayout.TextField(
                    new GUIContent($"Correct Word {i + 1}", "Enter a correct word for this level."),
                    levelData.correctWords[i]
                );

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    levelData.correctWords.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Draws the particle prefabs list, allowing users to add, edit, and remove particle effect prefabs.
        /// </summary>
        /// <param name="levelData">The LevelData asset containing the particle prefabs.</param>
        public void DrawParticlePrefabsList(LevelData levelData)
        {
            GUILayout.Label("Particle System Prefabs", EditorStyles.boldLabel);

            for (int i = 0; i < levelData.particlePrefabs.Count; i++)
            {
                var entry = levelData.particlePrefabs[i];
                EditorGUILayout.BeginHorizontal();

                entry.name = EditorGUILayout.TextField("Name", entry.name);
                entry.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", entry.prefab, typeof(GameObject), false);

                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    levelData.particlePrefabs.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);
            }

            if (GUILayout.Button("Add Particle Prefab"))
            {
                levelData.particlePrefabs.Add(new LevelData.ParticleEntry());
            }
        }

        /// <summary>
        /// Draws the sound effects list, allowing users to add, edit, and remove sound effects.
        /// </summary>
        /// <param name="levelData">The LevelData asset containing the sound effects.</param>
        public void DrawSoundEffectsList(LevelData levelData)
        {
            GUILayout.Label("Sound Effects", EditorStyles.boldLabel);

            for (int i = 0; i < levelData.soundEffects.Count; i++)
            {
                var entry = levelData.soundEffects[i];
                EditorGUILayout.BeginHorizontal();

                entry.name = EditorGUILayout.TextField("Name", entry.name);
                entry.clip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", entry.clip, typeof(AudioClip), false);

                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    levelData.soundEffects.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);
            }

            if (GUILayout.Button("Add Sound Effect"))
            {
                levelData.soundEffects.Add(new LevelData.SoundEntry());
            }
        }
    }
}
