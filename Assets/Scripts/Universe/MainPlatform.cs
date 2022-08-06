﻿using System;
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

    private void Start()
    {
        floorCount = floors.Count;
        ball.Initialize(floors[currentFloorNumber], floorCount);
        currentFloor = floors[currentFloorNumber];
    }
#if UNITY_EDITOR
    [Button(ButtonSizes.Large), GUIColor(0f,1f,0f)]
    public void CreateRandomLevel() //FIXME Sonradan katlari da yarat ve belirli araliklar ver y'de
    {
        foreach (var floor in floors)
        {
            DestroyImmediate(floor.gameObject);
        }
        
        floors.Clear();
        
        float lastY = Constants.FirstFloorYLocalPos;
        
        for (int i = 0; i < wantedFloorNumber; i++)
        {
            var pos = Vector3.zero;
            pos.y = lastY;

            var createdFloorObj = PrefabUtility.InstantiatePrefab(floorPrefab, transform) as GameObject;
            var createdFloor = createdFloorObj.GetComponent<Floor>();
            createdFloor.transform.localPosition = pos;

            lastY -= floorsYDelta;
            
            floors.Add(createdFloor);
        }
        
        var finishPos = Vector3.zero;
        finishPos.y = lastY;
        var finishFloorObj = PrefabUtility.InstantiatePrefab(finishPrefab, transform) as GameObject;
        var finishFloor = finishFloorObj.GetComponent<Floor>();
        finishFloor.transform.localPosition = finishPos;
        floors.Add(finishFloor);

        for(int i=1; i<floors.Count - 1; i++)
        {
            floors[i].RandomlyCreatePlatformPiece();
        }
    }
#endif
    public void CheckForFloorNumber(Vector3 ballPos)
    {
        if (ballPos.y < currentFloor.transform.position.y - floorPassOffset)
        {
            //FIXME currentFloor.Explode()
            ball.passedCurrentFloor = true;
            currentFloorNumber++;
            
            ball.FloorPassed();

            if (currentFloorNumber >= floorCount) return;
            
            currentFloor = floors[currentFloorNumber];
            ball.currentFloorTr = currentFloor.transform;
            ball.CheckForCombo(currentFloorNumber);
        }

        else
        {
            ball.passedCurrentFloor = false;
        }
        
    }
    
}
