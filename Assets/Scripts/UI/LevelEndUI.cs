using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField] private LevelFailUI levelFailUI;
    [SerializeField] private LevelWinUI levelWinUI;

    public void ActivateLevelEndUI(bool won)
    {
        if (won)
        {
            levelWinUI.gameObject.SetActive(true);
        }

        else
        {
            levelFailUI.gameObject.SetActive(true);
        }
    }
    
}
