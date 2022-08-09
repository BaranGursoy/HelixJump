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
    private bool hasExploded;
    
    public void GetPlatformPieces()
    {
        platformPieces = new List<PlatformPiece>();
        platformPieces = GetComponentsInChildren<PlatformPiece>().ToList();
    }

    public void SetHasExploded()
    {
        hasExploded = true;
    }

    public bool HasFloorExploded()
    {
        return hasExploded;
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

    public void RandomlyGenerateBoostsAndVerticalObstacles()
    {
        foreach (var platformPiece in platformPieces)
        {
            platformPiece.AdjustBoostsAndVerticalObstacles();
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

    public void ChangeFloorMaterials(Material material)
    {
        foreach (var platformPiece in platformPieces)
        {
            platformPiece.ChangeMaterial(material);
        }
    }

    public void ChangeFloorMaterialsToOriginal()
    {
        foreach (var platformPiece in platformPieces)
        {
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
            chosenPlatform.transform.tag = "Obstacle";
            obstacleCounter++;

            if (obstacleCounter >= randomCountForObstacle)
            {
                break;
            }
        }
        
        int randomCountForBrekable = Random.Range(0, 2);
        int breakableCounter = 0;
        
        
        List<PlatformPiece> breakablePlatformPieces = new List<PlatformPiece>();

        while (true)
        {
            int index = Random.Range(0, platformPieces.Count);
            var chosenPlatform = platformPieces[index];

            if (breakablePlatformPieces.Contains(chosenPlatform))
            {
                continue;
            }

            chosenPlatform.ChangeTypeToBreakable();
            chosenPlatform.ChangeMaterial();
            chosenPlatform.transform.tag = "Breakable";
            breakableCounter++;

            if (breakableCounter >= randomCountForBrekable)
            {
                break;
            }
        }
    }

    public void ChangeObstaclesTags()
    {
        foreach (var platformPiece in platformPieces)
        {
            if (platformPiece.GetPlatformType() == PlatformPieceType.Obstacle)
            {
                platformPiece.transform.tag = "Obstacle";
            }
        }
    }
    
    public void ExplodeFloor(Material material)
    {
        SetHasExploded();
        foreach (var platformPiece in platformPieces)
        {
            if (platformPiece != null && platformPiece.gameObject.activeInHierarchy)
            {
                platformPiece.ExplodePiece(material);
            }
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
