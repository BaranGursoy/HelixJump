using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class MainPlatform : MonoBehaviour
{
    [SerializeField] private List<Floor> floors;
    [SerializeField] private float floorPassOffset = 0.3f;

    [SerializeField] private int wantedFloorNumber;
    [SerializeField] private float floorsYDelta = 0.3f;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject finishPrefab;

    [SerializeField] private Ball ball;

    private int floorCount;
    private int currentFloorNumber;
    private Floor currentFloor;
    private float originalFloorPassOffset;

    private void Start()
    {
        originalFloorPassOffset = floorPassOffset;
        floorCount = floors.Count;
        ball.Initialize(floors[currentFloorNumber], floorCount);
        currentFloor = floors[currentFloorNumber];
    }
#if UNITY_EDITOR
    [Button(ButtonSizes.Large), GUIColor(0f, 1f, 0f)]
    public void CreateRandomLevel()
    {
        foreach (var floor in floors)
        {
            DestroyImmediate(floor.gameObject);
        }

        floors.Clear();

        float lastY = Constants.FirstFloorYPos;

        for (int i = 0; i < wantedFloorNumber; i++)
        {
            var pos = Vector3.zero;
            pos.y = lastY;

            var createdFloorObj = PrefabUtility.InstantiatePrefab(floorPrefab) as GameObject;
            createdFloorObj.transform.SetParent(transform);
            var createdFloor = createdFloorObj.GetComponent<Floor>();
            createdFloor.transform.position = pos;

            lastY -= floorsYDelta;

            floors.Add(createdFloor);
        }

        var finishPos = Vector3.zero;
        finishPos.y = lastY;
        var finishFloorObj = PrefabUtility.InstantiatePrefab(finishPrefab) as GameObject;
        finishFloorObj.transform.SetParent(transform);
        var finishFloor = finishFloorObj.GetComponent<Floor>();
        finishFloor.transform.position = finishPos;
        floors.Add(finishFloor);

        for (int i = 1; i < floors.Count - 1; i++)
        {
            floors[i].RandomlyCreatePlatformPiece();
        }
    }


    [Button(ButtonSizes.Large), GUIColor(1f, 0f, 0f)]
    public void DeleteLevel()
    {
        foreach (var floor in floors)
        {
            DestroyImmediate(floor.gameObject);
        }

        floors.Clear();
    }

    [Button(ButtonSizes.Large)]
    public void GetAllPlatformPieces()
    {
        foreach (var floor in floors)
        {
            floor.UpdatePlatformPieces();
        }
    }

    [Button(ButtonSizes.Large)]
    public void ChangeObstacleTags()
    {
        foreach (var floor in floors)
        {
            floor.ChangeObstaclesTags();
        }
    }

    [Button(ButtonSizes.Large)]
    public void RandomlyGenerateBoostsAndVerticalObstacles()
    {
        foreach (var floor in floors)
        {
            floor.RandomlyGenerateBoostsAndVerticalObstacles();
        }
    }

#endif
    public void CheckForFloorNumber(Vector3 ballPos)
    {
        if (ballPos.y < currentFloor.transform.position.y - floorPassOffset)
        {
            if (ball.IsBoostModeActive())
            {
                currentFloor.ExplodeFloor(ball.GetBoostMaterial());
            }

            else if (!currentFloor.HasFloorExploded())
            {
                currentFloor.ExplodeFloor(null);
            }
                
            ball.passedCurrentFloor = true;
            currentFloorNumber++;
            
            if (currentFloorNumber >= floorCount) return;
            
            currentFloor = floors[currentFloorNumber];
            ball.currentFloorTr = currentFloor.transform;
            ball.CheckForCombo(currentFloorNumber);
        }
    }

    public void ChangeAllFloorMaterials(Material boostMat)
    {
        foreach (var floor in floors)
        {
            floor.ChangeFloorMaterials(boostMat);
        }
    }

    public void ChangeAllFloorMaterialsToOriginal()
    {
        foreach (var floor in floors)
        {
            floor.ChangeFloorMaterialsToOriginal();
        }
    }

    public void ChangeFloorPassOffset(float newOffset)
    {
        floorPassOffset = newOffset;
    }

    public void ResetFloorPassOffset()
    {
        floorPassOffset = originalFloorPassOffset;
    }
    
}
