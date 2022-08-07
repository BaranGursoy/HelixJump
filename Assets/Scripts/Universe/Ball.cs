using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float bounceForce;
    [SerializeField] private ParticleSystem splashParticle;
    [SerializeField] private BallAnimationController ballAnimationController;
    [SerializeField] private GameObject splashSpritePrefab;
    [SerializeField] private MainPlatform mainPlatform;
    
    [SerializeField] private MeshRenderer ballMeshRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Material feverModeMat;
    [SerializeField] private GameObject feverModeParticleObj;

    [HideInInspector] public bool passedCurrentFloor;
    [HideInInspector] public bool isFalling;

    private int baseGainedPoint;

    private int comboMultiplier = 1;

    private bool isJumping;
    public Transform currentFloorTr;
    public Transform lastCollidedFloorTr;

    private GameManager gameManager;
    private int totalFloorCount;

    private Material originalMat;
    private bool feverModeActive;

    private void Start()
    {
        gameManager = GameManager.Instance;
        baseGainedPoint = gameManager.GetLevelId();
        originalMat = ballMeshRenderer.sharedMaterial;
        SetColorsOfBallParticles();
    }

    public void Initialize(Floor startingFloor, int totalFloorCount)
    {
        currentFloorTr = startingFloor.transform;
        this.totalFloorCount = totalFloorCount;
    }
    
    private void Update()
    {
        mainPlatform.CheckForFloorNumber(transform.position);

        if (!isFalling)
        {
            return;
        }
    }

    private void ExplodeFloorWithFeverMode(Collision collision)
    {
        var floorTr = collision.transform.parent;
        var floor = floorTr.GetComponent<Floor>();
        floor.ExplodeFloor(originalMat);
        DeactivateFeverMode();
        Bounce();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Finish"))
        {
            GameManager.Instance.GameFinished(true);
        }
        
        if (collision.transform.CompareTag("Obstacle"))
        {
            if (!feverModeActive)
            {
                GameManager.Instance.GameFinished(false);
                return;
            }

            else
            {
                ExplodeFloorWithFeverMode(collision);
            }
        }
        
        if (collision.transform.CompareTag("PlatformPiece"))
        {
            passedCurrentFloor = false;
            
            var floorTr = collision.transform.parent;
            
            if (feverModeActive)
            {
                ExplodeFloorWithFeverMode(collision);
            }

            lastCollidedFloorTr = floorTr;

            if (!isJumping)
            {
                Bounce();
                CreatePaintSplash(collision.transform, collision.contacts[0].point);
            }

            if (lastCollidedFloorTr == currentFloorTr)
            {
                comboMultiplier = 1;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("PlatformPiece"))
        {
            isJumping = false;
        }
    }

    private void Bounce()
    {
        if (!PlayerController.Instance.GetPlaying())
        {
            return;
        }
        
        isJumping = true;
        
        splashParticle.Play();
        ballAnimationController.PlayBounceAnimation();
        
        var force = bounceForce * Vector3.up;
        rb.AddForce(force, ForceMode.Impulse);
    }
    

    private void CreatePaintSplash(Transform parentTr, Vector3 spawnPos)
    {
        var splashObj = Instantiate(splashSpritePrefab, spawnPos, Quaternion.Euler(90f, 0f, 0f));
        
        var splashSpriteRenderer = splashObj.GetComponent<SpriteRenderer>();
        splashSpriteRenderer.color = ballMeshRenderer.material.color;
        
        splashObj.transform.SetParent(parentTr);
    }

    public void CheckForCombo(int currentFloorNumber)
    {
        var pointGained = baseGainedPoint;
        pointGained *= comboMultiplier;
        comboMultiplier++;

        if (comboMultiplier == 4)
        {
            ActivateFeverMode();
        }
        
        gameManager.UpdatePlayerScore(pointGained);
        gameManager.UpdateLevelProgressionBar(currentFloorNumber, totalFloorCount);
    }

    public void FloorPassed()
    {
        isFalling = true;
    }

    private void SetColorsOfBallParticles()
    {
        var ballColor = ballMeshRenderer.material.color;
        
        var particleMainModule = splashParticle.main;
        particleMainModule.startColor = ballColor;

        trailRenderer.startColor = ballColor;

        var trailEndColor = ballColor;
        trailEndColor.a = 0f;
        
        trailRenderer.endColor = trailEndColor;
    }

    private void ActivateFeverMode()
    {
        feverModeActive = true;
        ChangeMaterialsForFeverMode();
        feverModeParticleObj.SetActive(true);
    }

    private void DeactivateFeverMode()
    {
        feverModeActive = false;
        ChangeMaterialsToOriginal();
        feverModeParticleObj.SetActive(false);
    }

    private void ChangeMaterialsForFeverMode()
    {
        ballMeshRenderer.material = feverModeMat;
        SetColorsOfBallParticles();
    }

    private void ChangeMaterialsToOriginal()
    {
        ballMeshRenderer.material = originalMat;
        SetColorsOfBallParticles();
    }
}
