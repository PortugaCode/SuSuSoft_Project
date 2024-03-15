using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionControl : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public delegate void animatorFunc();
    public animatorFunc[] doAnimatorArray;

    public Action<int> doCloneAnimation;

    private void Start()
    {
        doAnimatorArray = new animatorFunc[] { PlayBasketBall, PlayTennisBall, PlayGolf, PlayFootBall, PlayTrophy};
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

    private void PlayTennisBall()
    {
        animator.SetTrigger("DoTennisBall");
    }

    private void PlayGolf()
    {
        animator.SetTrigger("DoGolf");
    }

    private void PlayFootBall()
    {
        animator.SetTrigger("DoFootBall");
    }

    private void PlayTrophy()
    {
        animator.SetTrigger("DoTrophy");
    }

    public void PlayAnimation(int index)
    {
        doAnimatorArray[index]?.Invoke();
        doCloneAnimation?.Invoke(index);
    }
}
