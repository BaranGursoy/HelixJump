using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformPiece : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private PlatformPieceType platformPieceType;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material normalTransparentMat;
    [SerializeField] private Material breakableMat;
    [SerializeField] private Material obstacleMat;
    [SerializeField] private Material obstacleTransparentMat;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider collider;
    [SerializeField] private float explosionForce;
    [SerializeField] private float requiredTimeForFadeOut = 1f;
    [SerializeField] private GameObject boostObj;
    [SerializeField] private GameObject verticleObstacleObj;
    [SerializeField] private GameObject breakableObj;

    [SerializeField] private List<Collider> breakableColliders;
    [SerializeField] private List<Rigidbody> breakableRigidbodies;

    private int breakableHealth = 2;
    
    public void AdjustBoostsAndVerticalObstacles()
    {
        if (CompareTag("Finish"))
        {
            return;
        }
        
        var randomNumber = Random.Range(0, 50);

        if (randomNumber == 15)
        {
            //boostObj.SetActive(true);
        }

        if (randomNumber == 0)
        {
            verticleObstacleObj.SetActive(true);
        }
    }
    
    public void ChangeMaterialAndCollider()
    {
        if (meshRenderer == null)
        {
            return;
        }
        
        if (platformPieceType == PlatformPieceType.Normal)
        {
            if (!CompareTag("Finish"))
            {
                meshRenderer.material = normalMat;
                collider.isTrigger = false;
            }
        }

        if(platformPieceType == PlatformPieceType.Obstacle)
        {
            meshRenderer.material = obstacleMat;
            collider.isTrigger = false;
        }

        if (platformPieceType == PlatformPieceType.Breakable)
        {
            meshRenderer.material = breakableMat;
            collider.isTrigger = false;
            collider.transform.tag = "Breakable";
        }
    }
    
    public void ChangeMaterial(Material material)
    {
        if (meshRenderer && !CompareTag("Finish"))
        {
            meshRenderer.material = material;
        }
    }

    public void ChangeTypeToObstacle()
    {
        platformPieceType = PlatformPieceType.Obstacle;
    }
    
    public void ChangeTypeToNormal()
    {
        platformPieceType = PlatformPieceType.Normal;
    }
    
    public void ChangeTypeToBreakable()
    {
        platformPieceType = PlatformPieceType.Breakable;
    }

    public PlatformPieceType GetPlatformType()
    {
        return platformPieceType;
    }

    public void BreakableHit(Ball ball)
    {
        breakableHealth--;

        if (breakableHealth >= 1)
        {
            ball.Bounce();
            meshRenderer.enabled = false;
            breakableObj.SetActive(true);
            
        }

        if (breakableHealth == 0)
        {
            ExplodeBreakablePieces();
            
            rb.isKinematic = false;

            CloseCollider();
        
            transform.SetParent(null);
        
            var force = (-transform.up + transform.right) * explosionForce * 1.5f;
            rb.AddForce(force, ForceMode.Impulse);
            StartCoroutine(MakeExplodedPieceTransparentCor()); 
        }
    }

    private void ExplodeBreakablePieces()
    {
        foreach (var breakableCollider in breakableColliders)
        {
            breakableCollider.isTrigger = false;
        }

        foreach (var rb in breakableRigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    public void ExplodePiece(Material material)
    {
        if (material)
        {
            meshRenderer.material = material;
        }
        
        else switch (platformPieceType)
        {
            case PlatformPieceType.Normal:
                meshRenderer.material = normalTransparentMat;
                break;
            case PlatformPieceType.Obstacle:
                meshRenderer.material = obstacleTransparentMat;
                break;
        }
        
        rb.isKinematic = false;

        CloseCollider();
        
        transform.SetParent(null);
        
        var force = transform.right * explosionForce;
        rb.AddForce(force, ForceMode.Impulse);
        StartCoroutine(MakeExplodedPieceTransparentCor());
    }

    private IEnumerator MakeExplodedPieceTransparentCor()
    {
        float passedTime = 0f;

        var originalValue = meshRenderer.material.color.a;
        
        while (passedTime < requiredTimeForFadeOut)
        {
            var color = meshRenderer.material.color;
            color.a = Mathf.Lerp(originalValue, 0f, passedTime / requiredTimeForFadeOut);
            meshRenderer.material.color = color;
            
            passedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void CloseCollider()
    {
        collider.enabled = false;
    }

    private void OnDestroy()
    {
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("Boost") && !child.CompareTag("PlatformPiece") && !child.CompareTag("Obstacle"))
            {
                child.gameObject.SetActive(false);

                if (ObjectPooler.Instance)
                {
                    child.gameObject.transform.SetParent(ObjectPooler.Instance.transform);
                }
            }
        }
    }
}
