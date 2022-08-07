using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    #region Singleton

    public static CameraController Instance;

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
    
    private Ball ball;
    [SerializeField] private Transform mainCameraTr;
    [SerializeField] private Vector3 offset;
    
    [SerializeField] private float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private GameObject confettiRightObj;
    [SerializeField] private GameObject confettiLeftObj;

    [SerializeField] private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;


    private Transform ballTr;
    
    private Vector3 targetPos;

    public void Initialize(Ball ball)
    {
        this.ball = ball;
        ballTr = ball.transform;
    }

    public void RandomizeColorsWithHue()
    {
        postProcessVolume.profile.TryGetSettings(out colorGrading);

        int randomChoice = Random.Range(0, 2);

        colorGrading.hueShift.value = randomChoice == 0 ? 0f : Random.Range(-180f, 180f);
    }
    

    [Button(ButtonSizes.Large)]
    public void GetAndSetCurrentOffset()
    {
        offset = mainCameraTr.position - ballTr.position;
    }
    
    private void LateUpdate()
    {
        if (ball.passedCurrentFloor)
        {
            targetPos = ballTr.position + offset;
            mainCameraTr.position = Vector3.SmoothDamp(mainCameraTr.position, targetPos, ref velocity, smoothTime);
        }


        else if(Mathf.Abs(mainCameraTr.position.y - targetPos.y) > 0.1f && ball.currentFloorTr != null)
        {
            targetPos = ballTr.position + offset;
            mainCameraTr.position = Vector3.SmoothDamp(mainCameraTr.position, targetPos, ref velocity, smoothTime);
        }
    }

    public void PlayConfettis()
    {
        confettiLeftObj.SetActive(true);
        confettiRightObj.SetActive(true);
    }

    private void CloseConfettis()
    {
        confettiLeftObj.SetActive(false);
        confettiRightObj.SetActive(false);
    }

    public void Reset()
    {
        var pos = mainCameraTr.position;
        pos.y = Constants.FirstYPosCamera;
        mainCameraTr.position = pos;

        CloseConfettis();
    }
}
