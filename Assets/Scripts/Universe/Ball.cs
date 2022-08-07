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

    [HideInInspector] public bool passedCurrentFloor;
    [HideInInspector] public bool isFalling;
    [HideInInspector] public bool canMakeCombo;

    private int comboMultiplier = 1;

    private bool isJumping;
    public Transform currentFloorTr;
    public Transform lastCollidedFloorTr;

    private GameManager gameManager;
    private int totalFloorCount;

    private void Start()
    {
        gameManager = GameManager.Instance;
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

        if (currentFloorTr != lastCollidedFloorTr)
        {
            canMakeCombo = true; 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Finish"))
        {
            GameManager.Instance.GameFinished(true);
        }
        
        if (collision.transform.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameFinished(false);
            return;
        }
        
        if (collision.transform.CompareTag("PlatformPiece"))
        {
            var floorTr = collision.transform.parent;

            lastCollidedFloorTr = floorTr;

            if (!isJumping)
            {
                Bounce();
                CreatePaintSplash(collision.transform, collision.contacts[0].point);
            }

            if (lastCollidedFloorTr == currentFloorTr)
            {
                canMakeCombo = false;
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
        if (!PlayerInputController.Instance.GetPlaying())
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
        var pointGained = 3;
        pointGained *= comboMultiplier;
        comboMultiplier++;
        
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
}
