using TMPro;
using UnityEngine;

namespace Managers
{
    public class CanvasManager : MonoBehaviour
    {
        #region Singleton

        public static CanvasManager Instance;

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

        [SerializeField] private TextMeshProUGUI playerScoreTMP;
        [SerializeField] private GameObject flyingPointPrefab;

        public void UpdatePlayerScoreText(int playerScore)
        {
            playerScoreTMP.text = playerScore.ToString();
        }
    }
}
