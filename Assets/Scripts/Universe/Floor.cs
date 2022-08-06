using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Floor : MonoBehaviour
{
    [SerializeField] private List<PlatformPiece> platformPieces;
    [SerializeField] private List<PlatformPiece> disabledPieces;
    private bool isFirstFloor;
    private bool isFinishFloor;
    
    public void GetPlatformPieces()
    {
        platformPieces = new List<PlatformPiece>();
        platformPieces = GetComponentsInChildren<PlatformPiece>().ToList();
    }
    
    public void UpdatePlatformPieces()
    {
        foreach (var platformPiece in platformPieces)
        {
            if (!platformPiece.gameObject.activeInHierarchy)
            {
                platformPieces.Remove(platformPiece);
            }
        }
    }

    private void ResetFloor()
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
        ResetFloor();
        
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

    public void ExplodeFloor()
    {
        foreach (var platformPiece in platformPieces)
        {
            platformPiece.ExplodePiece();
        }
    }

    /*private void SortPlatformPieces()
    {
        platformPieces = platformPieces.OrderBy(x => x.transform.rotation.eulerAngles.y).ToList();
    }

    private void SetDisabledPieces()
    {
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                disabledPieces.Add(child.GetComponent<PlatformPiece>());
            }
        }
    }

    public void MakeCompoundPieces()
    {
        SearchForNeighbors();
    }*/

    /*private void SearchForNeighbors()
    {
        foreach (var platformPiece in platformPieces)
        {
            platformPiece.SearchNeighbor(ref platformPieces);
        }
    }*/
}
