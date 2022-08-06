using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class MainPlatform : MonoBehaviour
{
    [SerializeField] private List<Floor> floors;
    [SerializeField] private float floorPassOffset = 0.3f;
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

    [Button(ButtonSizes.Large)]
    public void CreateRandomLevel() //FIXME Sonradan katlari da yarat ve belirli araliklar ver y'de
    {
        floors = GetComponentsInChildren<Floor>().ToList();

        floors = floors.OrderByDescending(x => x.transform.position.y).ToList();

        for(int i=1; i<floors.Count; i++)
        {
            floors[i].RandomlyCreatePlatformPiece();
        }
    }

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
