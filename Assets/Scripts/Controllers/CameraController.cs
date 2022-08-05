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
    
    [SerializeField] private float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;


    private Transform ballTr;
    
    private Vector3 targetPos;

    private void Start()
    {
        ballTr = ball.transform;
    }

    [Button(ButtonSizes.Large)]
    public void GetAndSetCurrentOffset()
    {
        offset = mainCameraTr.position - ballTr.position;
    }
    
    private void LateUpdate() // FIXME hala durdugu yerde biraz oynuyor kamera bunu da coz
    {
        if (ball.passedCurrentFloor)
        {
            targetPos = ballTr.position + offset;
            mainCameraTr.position = Vector3.SmoothDamp(mainCameraTr.position, targetPos, ref velocity, smoothTime);
        }

        else if(Mathf.Abs(mainCameraTr.position.y - targetPos.y) > 0.1f && ball.currentFloorTr) // FIXME bir kere basta cok icine giriyor kamera
        {
            mainCameraTr.position = Vector3.SmoothDamp(mainCameraTr.position, targetPos, ref velocity, smoothTime);
        }
    }
}
