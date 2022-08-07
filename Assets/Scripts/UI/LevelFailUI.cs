using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

public class LevelFailUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelCompletePercentageTMP;

    private void OnEnable()
    {
        var value = CanvasManager.Instance.GetLevelCompletionPercentage();
        levelCompletePercentageTMP.text = "%" + value + " COMPLETED";
    }

    public void RestartLevel()
    {
        GameManager.Instance.LoadNextLevel();
        gameObject.SetActive(false);
    }
}
