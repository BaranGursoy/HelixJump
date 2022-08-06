using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAnimationController : MonoBehaviour
{
    [SerializeField] private Animator ballAnimator;
    private static readonly int BouncyBallAnim = Animator.StringToHash("BouncyBallAnim");

    public void PlayBounceAnimation()
    {
        ballAnimator.Play(BouncyBallAnim, 0, 0f);
    }
}
