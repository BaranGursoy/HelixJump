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
            canvasManager = CanvasManager.Instance;
        }

        public void UpdatePlayerScore(int point)
        {
            playerScore += point;
            canvasManager.CreateFlyingPointForUI(point);
            canvasManager.UpdatePlayerScoreText(playerScore);
        }
    }
    
    
}
