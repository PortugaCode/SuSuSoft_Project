using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionControl : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public delegate void animatorFunc();
    public animatorFunc[] doAnimatorArray;

    private void Start()
    {
        doAnimatorArray = new animatorFunc[] { PlayBasketBall };
    }

    #region [SetRootMotion] 
    public void SetApplyRootMotion_true()
    {
        animator.applyRootMotion = true;
    }

    public void SetApplyRootMotion_false()
    {
        animator.applyRootMotion = false;
    }
    #endregion

    private void PlayBasketBall()
    {
        animator.SetTrigger("DoBasketBall");
    }
}
