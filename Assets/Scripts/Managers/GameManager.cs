using System;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
        
        public int playerScore;
        public int bestScore;
        private int levelId = 1;

        private CanvasManager canvasManager;
        private LevelManager levelManager;
        private CameraController cameraController;
        private PlayerController playerController;

        private GameObject lastCreatedLevel;

        private void Start()
        {
            Application.targetFrameRate = 60;
            canvasManager = CanvasManager.Instance;
            levelManager = LevelManager.Instance;
            cameraController = CameraController.Instance;
            playerController = PlayerController.Instance;

            LoadNextLevel();
        }

        public void UpdatePlayerScore(int point)
        {
            playerScore += point;
            canvasManager.CreateFlyingPointForUI(point);
            canvasManager.UpdatePlayerScoreText(playerScore);
        }

        public void UpdateLevelProgressionBar(int currentFloorNumber, int totalFloorNumber)
        {
            canvasManager.UpdateLevelProgressionBar(currentFloorNumber, totalFloorNumber);
        }

        public int GetLevelId()
        {
            return levelId;
        }

        public void LoadNextLevel()
        {
            ResetAllLevelVariables();

            bestScore = PlayerPrefs.GetInt("BestScore");

            if (lastCreatedLevel != null)
            {
                Destroy(lastCreatedLevel);
            }

            levelId = PlayerPrefs.GetInt("LevelId");

            if (levelId == 0)
            {
                levelId = 1;
            }
            
            canvasManager.ChangeLevelIndicatorNumbers(levelId);

            var levelPrefab = levelManager.GetNextLevel(levelId);
            lastCreatedLevel = Instantiate(levelPrefab);
            var levelDataHolder = lastCreatedLevel.GetComponent<LevelDataHolder>();

            cameraController.Initialize(levelDataHolder.ball);
            playerController.Initialize(levelDataHolder.mainPlatform.transform);
        }

        public void IncreaseLevelId()
        {
            levelId++;
            PlayerPrefs.SetInt("LevelId", levelId);
        }
        
        private void ResetAllLevelVariables()
        {
            playerScore = 0;
            canvasManager.Reset();
            cameraController.Reset();
            playerController.Reset();
        }

        public void GameFinished(bool won)
        {
            playerController.LevelCompleted();
            canvasManager.LevelFinished(won);

            if (won)
            {
                SaveBestScore();
            }
        }

        private void SaveBestScore()
        {
            if (playerScore > bestScore)
            {
                bestScore = playerScore;
                PlayerPrefs.SetInt("BestScore", bestScore);
            }
        }
    }
    
}
