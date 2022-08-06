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
        [SerializeField] private Transform GameUITr;
        [SerializeField] private GameObject flyingPointPrefab;

        public void UpdatePlayerScoreText(int playerScore)
        {
            playerScoreTMP.text = playerScore.ToString();
        }

        public void CreateFlyingPointForUI(int point)
        {
            var flyingPoint = Instantiate(flyingPointPrefab, GameUITr).GetComponent<FlyingPointText>();
            flyingPoint.SetPointText(point);
        }
    }
}
