using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPiece : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private PlatformPieceType platformPieceType;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material obstacleMat;

    public void ChangeMaterial()
    {
        if (platformPieceType == PlatformPieceType.Normal)
        {
            meshRenderer.material = normalMat;
        }

        if(platformPieceType == PlatformPieceType.Obstacle)
        {
            meshRenderer.material = obstacleMat;
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
}
