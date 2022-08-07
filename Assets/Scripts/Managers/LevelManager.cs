using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton

    public static LevelManager Instance;

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

    [SerializeField] private List<GameObject> levels;

    public GameObject GetNextLevel(int levelId)
    {
        var actualLevelIndex = (levelId % levels.Count) - 1;

        if (levels.Count == 1)
        {
            actualLevelIndex = 0;
        }
        
        else if (levelId % levels.Count == 0)
        {
            actualLevelIndex = levelId - 1;
        }
        
        Debug.Log(actualLevelIndex); // FIXME delete this
        
        return levels[actualLevelIndex];
    }
}
