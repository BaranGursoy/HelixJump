using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private PlatformPieceType platformPieceType;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material normalTransparentMat;
    [SerializeField] private Material obstacleMat;
    [SerializeField] private Material obstacleTransparentMat;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private float explosionForce;
    [SerializeField] private float requiredTimeForFadeOut = 1f;

    public bool isPieceOfCompoundBlock;

    private List<PlatformPiece> neighbors = new List<PlatformPiece>();


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

    public PlatformPieceType GetPlatformType()
    {
        return platformPieceType;
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

        CloseColliders();
        
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

    private void CloseColliders()
    {
        foreach (var collider in colliders)
        {
            collider.isTrigger = true;
        }
    }

    private void OnDestroy()
    {
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("Boost") && !child.CompareTag("PlatformPiece"))
            {
                child.gameObject.SetActive(false);
                child.gameObject.transform.SetParent(ObjectPooler.Instance.transform);
            }
        }
    }
}
