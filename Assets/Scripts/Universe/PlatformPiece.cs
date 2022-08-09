﻿using System;
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
    
    public void AdjustBoostsAndVerticalObstacles()
    {
        if (CompareTag("Finish"))
        {
            return;
        }
        
        var randomNumber = Random.Range(0, 50);

        if (randomNumber == 15)
        {
            boostObj.SetActive(true);
        }

        if (randomNumber == 0)
        {
            verticleObstacleObj.SetActive(true);
        }
    }


    /*public void SearchNeighbor(ref List<PlatformPiece> platformPieces)
    {
        foreach (var platformPiece in platformPieces)
        {
            if (Math.Abs(Mathf.Abs(platformPiece.transform.rotation.eulerAngles.y % 360f -
                                   transform.rotation.eulerAngles.y % 360f) - 45f) < 0.1F)
            {
                neighbors.Add();
            }
        }
    }*/
    
    //FIXME butonla direkt platformu ayarlayabildigimiz platforma ozel bir editor scripti yaz
    
    public void ChangeMaterial()
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
            }
        }

        if(platformPieceType == PlatformPieceType.Obstacle)
        {
            meshRenderer.material = obstacleMat;
        }

        if (platformPieceType == PlatformPieceType.Breakable)
        {
            meshRenderer.material = breakableMat;
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
    
    public void BreakableExplode()
    {
        meshRenderer.enabled = false;
        breakableObj.SetActive(true);
        
        rb.isKinematic = false;

        CloseCollider();
        
        transform.SetParent(null);
        
        var force = (-transform.up + transform.right) * explosionForce * 1.5f;
        rb.AddForce(force, ForceMode.Impulse);
        StartCoroutine(MakeExplodedPieceTransparentCor()); 
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
        collider.isTrigger = true;
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
