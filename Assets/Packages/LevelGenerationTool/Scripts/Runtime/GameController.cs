namespace LevelEditorPlugin.Runtime
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameView gameView;
        [SerializeField] private LevelData levelData;
        private GameModel gameModel;

        private void Start()
        {
            InitializeGame(levelData);
            EventManager.OnAllCorrectWordsSelected += OnLevelComplete;
            EventManager.OnCorrectWordSelected += PlayCorrectSound;
            EventManager.OnIncorrectWordSelected += PlayIncorrectSound;
        }

        private void InitializeGame(LevelData levelData)
        {
            gameModel = new GameModel(levelData.correctWords);
            gameView.SetupUI(levelData);

            // Loop through each word button in the word set parent
            foreach (Transform child in gameView.WordSetParent) // Using the public property here
            {
                Button button = child.GetComponent<Button>();
                TextMeshProUGUI wordText = child.GetComponentInChildren<TextMeshProUGUI>();
                if (button != null && wordText != null)
                {
                    string word = wordText.text;
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
