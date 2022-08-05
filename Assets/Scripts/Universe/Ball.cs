using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float bounceForce;

    private bool isJumping;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PlatformPiece"))
        {
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
}
