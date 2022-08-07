using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class LevelWinUI : MonoBehaviour
{
    public void PlayNextLevel()
    {
        GameManager.Instance.IncreaseLevelId();
        GameManager.Instance.LoadNextLevel();
        gameObject.SetActive(false);
    }
}
