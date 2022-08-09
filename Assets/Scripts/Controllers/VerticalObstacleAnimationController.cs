using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VerticalObstacleAnimationController : MonoBehaviour
{
    [SerializeField] private Animator obstacleAnimator;

    private readonly int PlayAnimation = Animator.StringToHash("PlayAnimation");

    private void Start()
    {
        int randomValue = Random.Range(0, 3);

        if (randomValue == 0)
        {
            obstacleAnimator.SetBool(PlayAnimation, true);
        }        
    }

    public void StopAnimation()
    {
        obstacleAnimator.speed = 0f;
    }
}
