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

        private CanvasManager canvasManager;

        private void Start()
        {
            Application.targetFrameRate = 60;
            canvasManager = CanvasManager.Instance;
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
    }
    
    
}
