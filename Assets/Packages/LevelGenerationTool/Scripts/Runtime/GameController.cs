namespace LevelEditorPlugin.Runtime
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Linq;

    public class GameController : MonoBehaviour
    {
        [SerializeField] private int level = 1;
        [SerializeField] private GameView gameView;
        [SerializeField] private GameObject animatedSceneInstance;
        private LevelData levelData;
        private GameModel gameModel;

        private void Start()
        {
            LoadLevelData();
            InitializeGame(levelData);
            EventManager.OnAllCorrectWordsSelected += OnLevelComplete;
            EventManager.OnCorrectWordSelected += PlayCorrectSound;
            EventManager.OnIncorrectWordSelected += PlayIncorrectSound;

            animatedSceneInstance = GameObject.Find("AnimatedScenePreview");

            if (animatedSceneInstance == null)
            {
                Debug.LogWarning("Animated scene instance not found in the hierarchy.");
            }
        }

        private void LoadLevelData()
        {
            int targetLevelIndex = level;
            LevelData[] allLevels = Resources.LoadAll<LevelData>("");
            levelData = allLevels.FirstOrDefault(level => level.levelNum == targetLevelIndex);

            if (levelData == null)
            {
                Debug.LogError($"LevelData with levelNum {targetLevelIndex} could not be found in the Resources folder.");
            }
        }

        private void InitializeGame(LevelData levelData)
        {
            if (levelData == null)
            {
                Debug.LogError("LevelData is not loaded. Initialization aborted.");
                return;
            }

            gameModel = new GameModel(levelData.correctWords);
            gameView.SetupUI(levelData);

            foreach (Transform child in gameView.WordSetParent)
            {
                Button button = child.GetComponent<Button>();
                Text wordText = child.GetComponentInChildren<Text>();
                if (button != null && wordText != null)
                {
                    string word = wordText.text.Trim();
                    button.onClick.AddListener(() => OnWordSelected(word, button));
                }
            }
        }

        private void OnWordSelected(string word, Button button)
        {
            if (gameModel.IsWordCorrect(word))
            {
                if (gameModel.AddSelectedWord(word))
                {
                    gameView.ShowCorrectWord(button);

                    // Play particle effect for correct word
                    GameObject particlePrefab = levelData.GetParticlePrefab("Pop");
                    if (particlePrefab != null)
                    {
                        gameView.PlayEffect(particlePrefab, button.transform.position);
                    }
                    else
                    {
                        Debug.LogWarning("Pop particle effect not found in LevelData.");
                    }

                    EventManager.TriggerCorrectWordSelected();
                }

                if (gameModel.AllWordsSelected())
                {
                    EventManager.TriggerAllCorrectWordsSelected();
                }
            }
            else
            {
                gameView.ShowIncorrectWord(button);
                EventManager.TriggerIncorrectWordSelected();
            }
        }

        private void OnLevelComplete()
        {
            gameView.ShowLevelCompleteUI();
            if (animatedSceneInstance != null)
            {
                animatedSceneInstance.SetActive(levelData.animatedSceneOnCompletion);
            }
            else
            {
                Debug.LogWarning("AnimatedSceneInstance is not assigned in GameController.");
            }
        }

        private void PlayCorrectSound()
        {
            AudioManager.Instance.PlaySFXByName(levelData, "CorrectWord");
        }

        private void PlayIncorrectSound()
        {
            AudioManager.Instance.PlaySFXByName(levelData, "IncorrectWord");
        }

        private void OnDestroy()
        {
            EventManager.OnAllCorrectWordsSelected -= OnLevelComplete;
            EventManager.OnCorrectWordSelected -= PlayCorrectSound;
            EventManager.OnIncorrectWordSelected -= PlayIncorrectSound;
        }
    }
}
