using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Transform mainCameraTr;
    [SerializeField] private Vector3 offset;

    private Transform ballTr;

    private void Start()
    {
        ballTr = ball.transform;
    }

    [Button(ButtonSizes.Large)]
    public void GetAndSetCurrentOffset()
    {
        offset = mainCameraTr.position - ballTr.position;
    }
    
    private void LateUpdate()
    {
        if (!ball.passedCurrentFloor)
        {
            return;
        }
        
        var targetPos = ballTr.position + offset;
        mainCameraTr.position = targetPos;
    }
}
