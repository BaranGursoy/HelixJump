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

    private Coroutine lastVignetteCoroutine;


    private Transform ballTr;
    
    private Vector3 targetPos;
    private Vignette vignette;

    public void Initialize(Ball ball)
    {
        this.ball = ball;
        ballTr = ball.transform;
    }

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out vignette);
    }

    public void RandomizeColorsWithHue()
    {
        postProcessVolume.profile.TryGetSettings(out colorGrading);

        int randomChoice = Random.Range(0, 2);

        colorGrading.hueShift.value = randomChoice == 0 ? 0f : Random.Range(-180f, 180f);
    }

    public void ActivateVignette(Material material)
    {
        var color = material.color;

        if (vignette)
        {
            vignette.color.value = color;
            lastVignetteCoroutine = StartCoroutine(VignetteCoroutine(vignette));
        }
    }

    public void DeactivateVignette()
    {
        if (lastVignetteCoroutine != null)
        {
            StopCoroutine(lastVignetteCoroutine);
        }
        
        if (vignette)
        {
            vignette.intensity.value = 0f;
        }
    }

    private IEnumerator VignetteCoroutine(Vignette vignette)
    {
        float requiredTime = 0.3f;
        float timePassed = 0f;

        float targetValue = 0.52f;
        
        while (timePassed < requiredTime)
        {
            vignette.intensity.value = Mathf.Lerp(0f, targetValue, timePassed / requiredTime);
            
            timePassed += Time.deltaTime;
            yield return null;
        }

        vignette.intensity.value = targetValue;
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
