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
        public int levelId = 1;

        private CanvasManager canvasManager;
        private LevelManager levelManager;
        private CameraController cameraController;
        private PlayerInputController playerInputController;

        private GameObject lastCreatedLevel;

        private void Start()
        {
            Application.targetFrameRate = 60;
            canvasManager = CanvasManager.Instance;
            levelManager = LevelManager.Instance;
            cameraController = CameraController.Instance;
            playerInputController = PlayerInputController.Instance;

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

        private void LoadNextLevel()
        {
            if (lastCreatedLevel != null)
            {
                Destroy(lastCreatedLevel);
            }
            
            var levelPrefab = levelManager.GetNextLevel(levelId);
            lastCreatedLevel = Instantiate(levelPrefab);
            var levelDataHolder = lastCreatedLevel.GetComponent<LevelDataHolder>();

            cameraController.Initialize(levelDataHolder.ball);
            playerInputController.Initialize(levelDataHolder.mainPlatform.transform);
        }
    }
    
    
}
