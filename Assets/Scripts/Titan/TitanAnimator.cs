using UnityEngine;
using System;

public class TitanAnimator : MonoBehaviour
{
    private enum AnimationTriggers { Walk, Attack, Eat, Idle }
    [SerializeField] private Animator animator;
    [SerializeField] private TitanController titanController;


    private void ResetAllAnimationTriggers()
    {
        foreach (AnimationTriggers trigger in Enum.GetValues(typeof(AnimationTriggers)))
        {
            animator.ResetTrigger(trigger.ToString());
        }
    }

    private void Update()
    {
        WalkAnimation();
        AttackAnimation();
        EatAnimation();
    }

    void WalkAnimation()
    {
        if (!titanController.isAttacking && !titanController.isEating)
        {
            ResetAllAnimationTriggers();
            animator.SetTrigger(AnimationTriggers.Walk.ToString());
        }
    }

    void AttackAnimation()
    {
        if (titanController.isAttacking && !titanController.isEating)
        {
            ResetAllAnimationTriggers();
            animator.SetTrigger(AnimationTriggers.Attack.ToString());
        }
    }

    void EatAnimation()
    {
        if (!titanController.isEating)
            return;

        ResetAllAnimationTriggers();
        animator.SetTrigger(AnimationTriggers.Eat.ToString());
    }

    void IdleAnimation()
    {
        if (!titanController.isRelax)
            return;

        ResetAllAnimationTriggers();
        animator.SetTrigger(AnimationTriggers.Idle.ToString());
    }
}

