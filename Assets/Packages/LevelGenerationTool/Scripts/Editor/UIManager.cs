using UnityEditor;
using UnityEngine;
using LevelEditorPlugin.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace LevelEditorPlugin.Editor
{
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
            levelData.levelName = EditorGUILayout.TextField("Level Name", levelData.levelName);

            // Background Image
            GUILayout.Label("Background Image", EditorStyles.boldLabel);
            levelData.backgroundImg = (Sprite)EditorGUILayout.ObjectField("Background Image", levelData.backgroundImg, typeof(Sprite), false);

            if (GUILayout.Button("Apply Background"))
            {
                ApplyBackground(levelData.backgroundImg);
            }

            GUILayout.Space(10);

            // Question Text
            levelData.questionText = EditorGUILayout.TextField("Question Text", levelData.questionText);

            GUILayout.Space(10);

            // Animated Scene Prefab
            levelData.animatedScenePrefab = (GameObject)EditorGUILayout.ObjectField("Animated Scene Prefab", levelData.animatedScenePrefab, typeof(GameObject), false);

            // Toggle for Animated Scene on Completion
            levelData.animatedSceneOnCompletion = EditorGUILayout.Toggle(
                new GUIContent("Animated Scene On Completion", "Enable or disable the animated scene upon level completion"),
                levelData.animatedSceneOnCompletion
            );


            GUILayout.Space(10);

            // Success Animations
            DrawSuccessAnimations(levelData);

            // Word Management
            DrawWordManagement(levelData);

            // Correct Words Management
            DrawCorrectWords(levelData);
        }

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
        public void DrawSuccessAnimations(LevelData levelData)
        {
            GUILayout.Label("Success Animations", EditorStyles.boldLabel);

            if (levelData.successAnimations == null)
            {
                levelData.successAnimations = new AnimationClip[0]; // Initialize if null
            }

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

        public void DrawWordManagement(LevelData levelData)
        {
            GUILayout.Label("Manage Words", EditorStyles.boldLabel);

            if (GUILayout.Button("Add New Word"))
            {
                levelData.words.Add(string.Empty);
            }

            // Temporary list to store indices of items to delete
            List<int> indicesToRemove = new List<int>();

            for (int i = 0; i < levelData.words.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                levelData.words[i] = EditorGUILayout.TextField(
                    new GUIContent($"Word {i + 1}", "Enter a word for this level."),
                    levelData.words[i]
                );

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    indicesToRemove.Add(i);  // Mark for deletion
                }

                EditorGUILayout.EndHorizontal();
            }

            // Remove items after the loop
            foreach (int index in indicesToRemove.OrderByDescending(i => i))
            {
                levelData.words.RemoveAt(index);
            }
        }


        private void DrawCorrectWords(LevelData levelData)
        {
            GUILayout.Label("Correct Words", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Correct Word"))
            {
                levelData.correctWords.Add(string.Empty);
            }

            // List to store indices of items to remove
            List<int> indicesToRemove = new List<int>();

            for (int i = 0; i < levelData.correctWords.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                levelData.correctWords[i] = EditorGUILayout.TextField($"Correct Word {i + 1}", levelData.correctWords[i]);

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    indicesToRemove.Add(i);  // Mark for deletion
                }

                EditorGUILayout.EndHorizontal();
            }

            // Remove marked items after the loop
            foreach (int index in indicesToRemove.OrderByDescending(i => i))
            {
                levelData.correctWords.RemoveAt(index);
            }
        }



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
