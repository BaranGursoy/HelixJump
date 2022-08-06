using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float bounceForce;
    [SerializeField] private float floorPassOffset = 0.3f;

    [HideInInspector] public bool passedCurrentFloor;
    
    private bool isJumping;
    [HideInInspector] public Transform currentFloorTr;

    private void Update()
    {
        CheckFloorPasses();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PlatformPiece"))
        {
            var floorTr = collision.transform.parent.parent;
            
            if (currentFloorTr != floorTr)
            {
                currentFloorTr = floorTr;
                passedCurrentFloor = false;
            }
            
            if (!isJumping)
            {
                Bounce();
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
        isJumping = true;
        var force = bounceForce * Vector3.up;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void CheckFloorPasses()
    {
        if (currentFloorTr == null)
        {
            return;
        }
        
        if (transform.position.y < currentFloorTr.position.y - floorPassOffset)
        {
            passedCurrentFloor = true;
        }

        else
        {
            passedCurrentFloor = false;
        }
    }
}
