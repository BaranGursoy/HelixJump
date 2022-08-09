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
    [SerializeField] private Collider ballCollider;

    [HideInInspector] public bool passedCurrentFloor;

    private int baseGainedPoint;

    private int comboMultiplier = 1;

    private bool isJumping;
    public Transform currentFloorTr;
    public Transform lastCollidedFloorTr;

    private GameManager gameManager;
    private int totalFloorCount;

    private Material originalMat;
    private bool feverModeActive;
    private bool boostModeActive;
    private bool isJumpingAfterBoost;

    private float collisionBugTimer;

    private ObjectPooler objectPooler;

    private void Start()
    {
        gameManager = GameManager.Instance;
        objectPooler = ObjectPooler.Instance;

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
    }

    public bool IsBoostModeActive()
    {
        return boostModeActive;
    }

    public Material GetBoostMaterial()
    {
        return feverModeMat;
    }
    
    private void ExplodeFloorWithFeverMode(Collision collision)
    {
        var floorTr = collision.transform.parent;
        var floor = floorTr.GetComponent<Floor>();
        floor.ExplodeFloor(originalMat);
        DeactivateFeverMode();
        BounceAfterFeverAndBoost();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Finish"))
        {
            GameManager.Instance.GameFinished(true);
        }
        
        if (collision.transform.CompareTag("Obstacle"))
        {
            if (!feverModeActive && !boostModeActive)
            {
                GameManager.Instance.GameFinished(false);
                rb.isKinematic = true;
                return;
            }
            
            ExplodeFloorWithFeverMode(collision);
           
        }

        if (collision.transform.CompareTag("Breakable"))
        {
            var platformPiece = collision.transform.GetComponent<PlatformPiece>();
            platformPiece.BreakableExplode();
        }
        
        if (collision.transform.CompareTag("PlatformPiece"))
        {
            passedCurrentFloor = false;

            collisionBugTimer = 0f;
            
            var floorTr = collision.transform.parent;

            if (feverModeActive)
            {
                ExplodeFloorWithFeverMode(collision);
            }

            lastCollidedFloorTr = floorTr;

            if (!isJumping && !isJumpingAfterBoost)
            {
                Bounce();

                var pos = collision.contacts[0].point;
                pos.y = floorTr.position.y - 0.07f;
                
                CreatePaintSplash(collision.transform, pos);
            }

            isJumpingAfterBoost = false;

            if (lastCollidedFloorTr == currentFloorTr)
            {
                comboMultiplier = 1;
            }
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.transform.CompareTag("PlatformPiece"))
        {
            collisionBugTimer += Time.deltaTime;

            if (collisionBugTimer > 0.15f)
            {
                Bounce();
                collisionBugTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boost"))
        {
            ActivateBoostMode();
            Destroy(other.gameObject);
        }
    }

    public bool CheckForShouldCameraFollow()
    {
        return !(lastCollidedFloorTr == currentFloorTr);
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

        StartCoroutine(MakeIsJumpingFalseCoroutine());
    }

    private IEnumerator MakeIsJumpingFalseCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        isJumping = false;
    }
    private void BounceAfterFeverAndBoost()
    {
        if (!PlayerController.Instance.GetPlaying())
        {
            return;
        }
        
        splashParticle.Play();
        ballAnimationController.PlayBounceAnimation();
        isJumpingAfterBoost = true;
        
        var force = bounceForce * Vector3.up;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void CreatePaintSplash(Transform parentTr, Vector3 spawnPos)
    {
        var splashObj = objectPooler.SpawnFromPool("PaintSplash", spawnPos, Quaternion.Euler(90f, 0f, 0f));
        
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
        if (boostModeActive)
        {
            return;
        }
        
        feverModeActive = true;
        ChangeMaterialsForFeverMode();
        feverModeParticleObj.SetActive(true);

        CameraController.Instance.ActivateVignette(feverModeMat);
        CameraController.Instance.IncreaseFov();
    }

    private void DeactivateFeverMode()
    {
        if (!feverModeActive)
        {
            return;
        }
        
        feverModeActive = false;
        ChangeMaterialsToOriginal();
        feverModeParticleObj.SetActive(false);
        
        CameraController.Instance.DeactivateVignette();
        CameraController.Instance.ResetFov();
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

    private void ActivateBoostMode()
    {
        boostModeActive = true;
        ballCollider.isTrigger = true;
        ChangeMaterialsForFeverMode();
        mainPlatform.ChangeAllFloorMaterials(feverModeMat);
        StartCoroutine(BoostModeCor());
        
        mainPlatform.ChangeFloorPassOffset(-1f);
        
        CameraController.Instance.ActivateVignette(feverModeMat);
        CameraController.Instance.IncreaseFov();
    }

    private IEnumerator BoostModeCor() // FIXME belki kat sayisina gore yapilabilir
    {
        yield return new WaitForSeconds(1.2f);
        DeactivateBoostMode();
    }

    public void DeactivateBoostMode()
    {
        if (!boostModeActive)
        {
            return;
        }
        
        boostModeActive = false;
        ballCollider.isTrigger = false;
        ChangeMaterialsToOriginal();
        mainPlatform.ChangeAllFloorMaterialsToOriginal();
        
        if (!isJumping)
        {
            BounceAfterFeverAndBoost();
        }
        
        mainPlatform.ResetFloorPassOffset();
        
        CameraController.Instance.DeactivateVignette();
        CameraController.Instance.ResetFov();
    }
}
