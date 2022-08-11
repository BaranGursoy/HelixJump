using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Floor : MonoBehaviour
{
    [SerializeField] private List<PlatformPiece> platformPieces;
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

    public void AdjustMaterialsForLevelCreation()
    {
        foreach (var platformPiece in platformPieces)
        {
            platformPiece.ChangeMaterialAndCollider();    
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
            platformPiece.ChangeMaterialAndCollider();
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
            platformPiece.ChangeMaterialAndCollider();
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
            chosenPlatform.ChangeMaterialAndCollider();
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
            chosenPlatform.ChangeMaterialAndCollider();
            chosenPlatform.transform.tag = "Breakable";
            breakableCounter++;

            if (breakableCounter >= randomCountForBrekable)
            {
                break;
            }
        }
    }

    public void ChangeTags()
    {
        foreach (var platformPiece in platformPieces)
        {
            if (platformPiece.GetPlatformType() == PlatformPieceType.Obstacle)
            {
                platformPiece.transform.tag = "Obstacle";
            }
            
            if (platformPiece.GetPlatformType() == PlatformPieceType.Breakable)
            {
                platformPiece.transform.tag = "Breakable";
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
}
