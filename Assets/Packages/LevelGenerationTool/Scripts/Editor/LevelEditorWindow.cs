using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorWindow : EditorWindow
{
    private LevelData currentLevelData; // Reference to the current level data
    private EnvironmentData selectedEnvironment; // Selected environment to instantiate or replace
    private const string environmentContainerName = "EnvironmentContainer";
    private Vector2 scrollPos;

    // References for UI elements in the scene
    private Image backgroundImgUI;
    private TextMeshProUGUI questionTextUI;
    private Transform wordSetParent;

    [MenuItem("Tools/Level Environment and Level Editor")]
    private static void ShowWindow()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.titleContent = new GUIContent("Level Environment and Level Editor");
        window.minSize = new Vector2(600, 600);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        currentLevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", currentLevelData, typeof(LevelData), false);

        if (currentLevelData != null)
        {
            DrawLevelEditor();
        }
    }

    private void DrawLevelEditor()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        DrawLevelName();
        DrawBackgroundImage();
        DrawQuestionText();
        DrawWordManagement();
        DrawCorrectWords();
        DrawAnimatedScene();
        DrawSuccessAnimations();
        DrawEnvironmentManagement();
        DrawPreviewButton();

        EditorGUILayout.EndScrollView();
    }

    private void DrawLevelName()
    {
        currentLevelData.levelName = EditorGUILayout.TextField("Level Name", currentLevelData.levelName);
    }

    private void DrawBackgroundImage()
    {
        GUILayout.Label("Background Image", EditorStyles.boldLabel);
        currentLevelData.backgroundImg = (Sprite)EditorGUILayout.ObjectField("Background Image", currentLevelData.backgroundImg, typeof(Sprite), false);

        if (GUILayout.Button("Apply Background"))
        {
            ApplyBackground();
        }
    }

    private void DrawQuestionText()
    {
        GUILayout.Label("Question Text", EditorStyles.boldLabel);
        currentLevelData.questionText = EditorGUILayout.TextField("Question", currentLevelData.questionText);
    }
    private void DrawWordManagement()
    {
        GUILayout.Label("Manage Words for Level", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Word"))
        {
            currentLevelData.words.Add(CreateInstance<WordData>());
        }

        for (int i = 0; i < currentLevelData.words.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            currentLevelData.words[i] = (WordData)EditorGUILayout.ObjectField(currentLevelData.words[i], typeof(WordData), false);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                currentLevelData.words.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Instantiate Words"))
        {
            InstantiateWords();
        }
    }

    private void DrawCorrectWords()
    {
        GUILayout.Label("Specify Correct Words", EditorStyles.boldLabel);

        int correctWordsCount = EditorGUILayout.IntField("Number of Correct Words", currentLevelData.correctWords != null ? currentLevelData.correctWords.Length : 0);
        if (correctWordsCount != (currentLevelData.correctWords?.Length ?? 0))
        {
            System.Array.Resize(ref currentLevelData.correctWords, correctWordsCount);
        }

        for (int i = 0; i < correctWordsCount; i++)
        {
            currentLevelData.correctWords[i] = (WordData)EditorGUILayout.ObjectField("Correct Word " + (i + 1), currentLevelData.correctWords[i], typeof(WordData), false);
        }
    }

    private void DrawAnimatedScene()
    {
        GUILayout.Label("Animated Scene", EditorStyles.boldLabel);
        currentLevelData.animatedScenePrefab = (GameObject)EditorGUILayout.ObjectField("Animated Scene Prefab", currentLevelData.animatedScenePrefab, typeof(GameObject), false);
    }

    private void DrawSuccessAnimations()
    {
        GUILayout.Label("Success Animations", EditorStyles.boldLabel);

        int animationsCount = EditorGUILayout.IntField("Number of Animations", currentLevelData.successAnimations != null ? currentLevelData.successAnimations.Length : 0);
        if (animationsCount != (currentLevelData.successAnimations?.Length ?? 0))
        {
            System.Array.Resize(ref currentLevelData.successAnimations, animationsCount);
        }

        for (int i = 0; i < animationsCount; i++)
        {
            currentLevelData.successAnimations[i] = (AnimationClip)EditorGUILayout.ObjectField("Animation " + (i + 1), currentLevelData.successAnimations[i], typeof(AnimationClip), false);
        }
    }

    private void DrawEnvironmentManagement()
    {
        GUILayout.Label("Environment Management", EditorStyles.boldLabel);

        foreach (var environment in currentLevelData.environmentOptions)
        {
            if (environment == null || environment.prefab == null) continue;

            if (GUILayout.Button(environment.environmentName))
            {
                if (Selection.activeGameObject != null)
                {
                    ReplaceEnvironment(Selection.activeGameObject, environment);
                }
                else
                {
                    InstantiateEnvironment(environment);
                }
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Delete Selected Environment") && Selection.activeGameObject != null)
        {
            DeleteEnvironment(Selection.activeGameObject);
        }

        if (GUILayout.Button("Delete All Environments"))
        {
            DeleteAllEnvironments();
        }

        if (GUILayout.Button("Save Environment Setup"))
        {
            SaveEnvironmentSetup();
        }
    }

    private void DrawPreviewButton()
    {
        if (GUILayout.Button("Preview Level"))
        {
            PreviewLevel();
        }
    }

    // Missing Method Definitions

    private void InstantiateEnvironment(EnvironmentData environmentData)
    {
        if (environmentData != null)
        {
            Vector3 defaultPosition = GetSceneCenterInWorld();
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(environmentData.prefab);
            if (instance != null)
            {
                instance.transform.position = defaultPosition;
                instance.transform.SetParent(GetEnvironmentContainer().transform);
                Selection.activeGameObject = instance;

                currentLevelData.placedEnvironments.Add(new LevelData.PlacedEnvironment
                {
                    environmentData = environmentData,
                    position = defaultPosition,
                    rotation = instance.transform.rotation
                });
            }
        }
    }

    private void ReplaceEnvironment(GameObject oldEnvironment, EnvironmentData newEnvironmentData)
    {
        if (newEnvironmentData != null)
        {
            Vector3 position = oldEnvironment.transform.position;
            Quaternion rotation = oldEnvironment.transform.rotation;

            DestroyImmediate(oldEnvironment);

            GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(newEnvironmentData.prefab);
            if (newInstance != null)
            {
                newInstance.transform.position = position;
                newInstance.transform.rotation = rotation;
                newInstance.transform.SetParent(GetEnvironmentContainer().transform);
                Selection.activeGameObject = newInstance;

                var existingEnvironment = currentLevelData.placedEnvironments.Find(e => e.position == position);
                if (existingEnvironment != null)
                {
                    existingEnvironment.environmentData = newEnvironmentData;
                    existingEnvironment.rotation = rotation;
                }
                else
                {
                    currentLevelData.placedEnvironments.Add(new LevelData.PlacedEnvironment
                    {
                        environmentData = newEnvironmentData,
                        position = position,
                        rotation = rotation
                    });
                }
            }
        }
    }

    private void DeleteEnvironment(GameObject environment)
    {
        Vector3 position = environment.transform.position;
        DestroyImmediate(environment);
        currentLevelData.placedEnvironments.RemoveAll(e => e.position == position);
    }

    private void DeleteAllEnvironments()
    {
        var environmentContainer = GameObject.Find(environmentContainerName);
        if (environmentContainer != null)
        {
            DestroyImmediate(environmentContainer);
        }
        currentLevelData.placedEnvironments.Clear();
    }

    private void SaveEnvironmentSetup()
    {
        EditorUtility.SetDirty(currentLevelData);
        AssetDatabase.SaveAssets();
        Debug.Log("Environment setup saved successfully.");
    }

    private GameObject GetEnvironmentContainer()
    {
        GameObject container = GameObject.Find(environmentContainerName);
        if (container == null)
        {
            container = new GameObject(environmentContainerName);
        }
        return container;
    }

    private Vector3 GetSceneCenterInWorld()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            Vector3 worldPosition = sceneView.camera.transform.position + sceneView.camera.transform.forward * 10f;
            return new Vector3(worldPosition.x, worldPosition.y, 0);
        }
        return Vector3.zero;
    }

    private void ApplyBackground()
    {
        // Attempt to find the background image GameObject by tag
        GameObject bgObject = GameObject.FindWithTag("Background");

        // If the background GameObject doesn't exist, create one
        if (bgObject == null)
        {
            // Create a new GameObject for the background
            bgObject = new GameObject("BackgroundImage");
            bgObject.tag = "Background";

            // Set it as a child of the Canvas (if one exists)
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                bgObject.transform.SetParent(canvas.transform, false);
            }

            // Add an Image component to the new GameObject
            backgroundImgUI = bgObject.AddComponent<Image>();

            // Optionally, set the new background to stretch to fill the Canvas
            RectTransform rectTransform = bgObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
        else
        {
            // If the background GameObject exists, get the Image component
            backgroundImgUI = bgObject.GetComponent<Image>();
        }

        // Set the background image if the Image component and the LevelData background sprite are available
        if (backgroundImgUI != null && currentLevelData.backgroundImg != null)
        {
            backgroundImgUI.sprite = currentLevelData.backgroundImg;
            Debug.Log("Background Image Applied.");
        }
        else if (backgroundImgUI == null)
        {
            Debug.LogError("Failed to find or create a background Image component.");
        }
    }
    private void ApplyQuestionText()
    {
        questionTextUI = GameObject.Find("Question").GetComponent<TextMeshProUGUI>();
        if (questionTextUI != null)
        {
            questionTextUI.text = currentLevelData.questionText;
            Debug.Log("Question Text Applied.");
        }
    }
    private void InstantiateWords()
    {
        // Find the Wordset GameObject
        wordSetParent = GameObject.Find("Wordset")?.transform;
        if (wordSetParent == null)
        {
            Debug.LogError("Wordset GameObject not found. Make sure there is a GameObject named 'Wordset' in the scene.");
            return;
        }

        // Clear all existing child objects in Wordset
        for (int i = wordSetParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(wordSetParent.GetChild(i).gameObject);
        }

        // Check if there are words to instantiate
        if (currentLevelData.words == null || currentLevelData.words.Count == 0)
        {
            Debug.Log("No words to instantiate; Wordset will remain empty.");
            return;
        }

        // Check if the WordButton prefab is assigned in LevelData
        if (currentLevelData.wordButtonPrefab == null)
        {
            Debug.LogError("WordButton prefab is not assigned in LevelData.");
            return;
        }

        // Instantiate new WordButton instances for each word in LevelData
        foreach (WordData word in currentLevelData.words)
        {
            if (word != null)
            {
                // Instantiate the WordButton prefab
                GameObject wordInstance = Instantiate(currentLevelData.wordButtonPrefab, wordSetParent);

                // Find the TextMeshProUGUI component in the prefab and set the word text
                TextMeshProUGUI wordText = wordInstance.GetComponentInChildren<TextMeshProUGUI>();
                if (wordText != null)
                {
                    wordText.text = word.word;
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI component not found in WordButton prefab.");
                }
            }
        }

        Debug.Log("Words instantiated in the Wordset.");
    }

    private void PreviewLevel()
    {
        Debug.Log("Previewing Level: " + currentLevelData.levelName);
        ApplyBackground();
        ApplyQuestionText();
        InstantiateWords();
    }
}
