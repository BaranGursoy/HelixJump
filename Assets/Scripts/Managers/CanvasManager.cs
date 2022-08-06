using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Slider levelProgressionBar;
        [SerializeField] private Image nextLevelIndicatorImage;
        [SerializeField] private Image thisLevelIndicatorImage;

        public void UpdatePlayerScoreText(int playerScore)
        {
            playerScoreTMP.text = playerScore.ToString();
        }

        public void CreateFlyingPointForUI(int point)
        {
            var flyingPoint = Instantiate(flyingPointPrefab, GameUITr).GetComponent<FlyingPointText>();
            flyingPoint.SetPointText(point);
        }

        public void UpdateLevelProgressionBar(int currentFloorNumber, int totalFloorNumber)
        {
            var progress = currentFloorNumber/(totalFloorNumber - 1f);
            StartCoroutine(LevelProgressCoroutine(progress));
        }

        public void Reset()
        {
            UpdatePlayerScoreText(0);
            levelProgressionBar.value = 0f;
        }

        private IEnumerator LevelProgressCoroutine(float targetValue)
        {
            float requiredTime = 0.3f;
            float timePassed = 0f;

            var startValue = levelProgressionBar.value;

            while (timePassed < requiredTime)
            {
                levelProgressionBar.value = Mathf.Lerp(startValue, targetValue, timePassed / requiredTime);
                
                timePassed += Time.deltaTime;
                yield return null;
            }

            levelProgressionBar.value = targetValue;

            if (Math.Abs(levelProgressionBar.value - 1f) < 0.03F)
            {
                nextLevelIndicatorImage.color = thisLevelIndicatorImage.color;
            }
        }
    }
}
