using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hopper {
    public class UiScreenManager : MonoBehaviour
    {
    
        [SerializeField] private Text gameScoreText;
        [SerializeField] private Text highestScoreText;
        [SerializeField] private Text startGameText;
        [SerializeField] private Text startGameTextShadow;
        [SerializeField] private Button startGame;
        [SerializeField] private GameObject gameOver;
        [SerializeField] private CharacterMovement character;
        [SerializeField] private CameraMovement cameraMovement;

        private const string HIGHEST_SCORE = "highest_score";

        private int _highestScore;

        private static UiScreenManager _instance;

        public static UiScreenManager GetInstance() {
            return _instance;
        }
        
        private void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Debug.LogError("Another UiScreen instance already present");
            }
        }

        public void SetGameOverScreen() {
            Clear();
            
            gameOver.SetActive(true);
            SetHighestScore();
            
            startGame.gameObject.SetActive(true);
            startGameText.text = "PLAY AGAIN";
            startGameTextShadow.text = "PLAY AGAIN";
            startGame.onClick.AddListener(() => SceneManager.LoadScene("HopperGameScene"));
        }

        private void SetHighestScore() {
            
            highestScoreText.gameObject.SetActive(true);
            _highestScore = PlayerPrefs.GetInt(HIGHEST_SCORE, 0);
            int currentScore = HopperGameManager.GetInstance().GetScore();
            if ( currentScore > _highestScore) {
                PlayerPrefs.SetInt(HIGHEST_SCORE, currentScore);
                PlayerPrefs.Save();
            }

            highestScoreText.text = "Highest Score : " + PlayerPrefs.GetInt(HIGHEST_SCORE);
        }

        public void SetStartGameButton() {
            startGame.gameObject.SetActive(true);
            startGameText.text = "START GAME";
            startGameTextShadow.text = "START GAME";
            startGame.onClick.RemoveAllListeners();
            startGame.onClick.AddListener(() => {
                startGame.gameObject.SetActive(false);
                character.StartCharacterMovement();
                cameraMovement.SetCameraMovement(true);
            });
        }
        
        public void SetScoreText(int score) {
            gameScoreText.text =  score.ToString();
        }
        
        public void SetStartScreen() {
            Clear();
            SetStartGameButton();
        }
        
        
        public void Clear() {
            highestScoreText.gameObject.SetActive(false);
            gameOver.SetActive(false);
            startGame.onClick.RemoveAllListeners();
            startGame.gameObject.SetActive(false);
        }
        
    }
}
