using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private List<PlatformPiece> platformPieces;
    private bool isFirstFloor;
    
    public void GetPlatformPieces()
    {
        platformPieces = new List<PlatformPiece>();
        platformPieces = GetComponentsInChildren<PlatformPiece>().ToList();
    }

    private void Reset()
    {
        foreach (Transform childTr in transform)
        {
            childTr.gameObject.SetActive(true);
        }

        GetPlatformPieces();

        foreach (var platformPiece in platformPieces)
        {
            platformPiece.ChangeTypeToNormal();
            platformPiece.ChangeMaterial();
        }
    }

    public void RandomlyCreatePlatformPiece()
    {
        if (isFirstFloor)
        {
            return;
        }

        Reset();
        
        int randomCountForHole = Random.Range(1, 4);
        int closedCounter = 0;

        List<PlatformPiece> closedPlatformPieces = new List<PlatformPiece>();

        while (true)
        {
            int index = Random.Range(0, platformPieces.Count);
            var chosenPlatform = platformPieces[index];

            if (closedPlatformPieces.Contains(chosenPlatform))
            {
                continue;
            }
            
            chosenPlatform.gameObject.SetActive(false);
            platformPieces.Remove(chosenPlatform);
            closedCounter++;

            if (closedCounter >= randomCountForHole)
            {
                break;
            }
        }

        closedPlatformPieces.Clear();
        closedPlatformPieces = null;

        int randomCountForObstacle = Random.Range(0, 3);
        int obstacleCounter = 0;
        
        
        List<PlatformPiece> obstaclePlatformPieces = new List<PlatformPiece>();

        while (true)
        {
            int index = Random.Range(0, platformPieces.Count);
            var chosenPlatform = platformPieces[index];

            if (obstaclePlatformPieces.Contains(chosenPlatform))
            {
                continue;
            }

            chosenPlatform.ChangeTypeToObstacle();
            chosenPlatform.ChangeMaterial();
            obstacleCounter++;

            if (obstacleCounter >= randomCountForObstacle)
            {
                break;
            }
        }
    }
}
