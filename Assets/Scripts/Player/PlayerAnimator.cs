using UnityEngine;
using System;

public class PlayerAnimator : MonoBehaviour
{
    private enum AnimationTriggers { Walk, Idle, Run, ODMGear, Attack }
    private Animator animator;
    private PlayerController playerControllerRef;
    private ODMGearController ODMGearControllerRef;
    private PlayerAttack playerAttackRef;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerControllerRef = GetComponent<PlayerController>();
        ODMGearControllerRef = GetComponent<ODMGearController>();
        playerAttackRef = GetComponent<PlayerAttack>();
    }

    private void ResetAllAnimationTriggers()
    {
        foreach (AnimationTriggers trigger in Enum.GetValues(typeof(AnimationTriggers)))
        {
            animator.ResetTrigger(trigger.ToString());
        }
    }

    private void ResetAnimationTriggers(params AnimationTriggers[] triggersToReset)
    {
        foreach (AnimationTriggers trigger in triggersToReset)
        {
            animator.ResetTrigger(trigger.ToString());
        }
    }

    private void Update()
    {
        PlayerControllerAnimation();
        PlayerAttackAnimation();
        ODMGearAnimation();
    }

    private void PlayerControllerAnimation()
    {
        if (playerControllerRef.canMove && !playerAttackRef.isAttacking)
        {
            float horizontalVelocity = new Vector3(playerControllerRef.move.x, 0, playerControllerRef.move.z).magnitude;

            ResetAllAnimationTriggers();

            if (playerControllerRef.isRunning && horizontalVelocity > 0.1f)
            {
                animator.SetTrigger(AnimationTriggers.Run.ToString());
            }
            else if (horizontalVelocity > 0.1f)
            {
                animator.SetTrigger(AnimationTriggers.Walk.ToString());
            }
            else if (!playerAttackRef.isAttacking)
            {
                animator.SetTrigger(AnimationTriggers.Idle.ToString());
            }
        }
        else
        {
            ResetAnimationTriggers(AnimationTriggers.Walk, AnimationTriggers.Idle, AnimationTriggers.Run);
        }
    }

    private void PlayerAttackAnimation()
    {
        if (playerAttackRef.isAttacking)
        {
            ResetAllAnimationTriggers();
            animator.SetTrigger(AnimationTriggers.Attack.ToString());
        }
    }

    private void ODMGearAnimation()
    {
        if (ODMGearControllerRef.isUseODMGear)
        {
            animator.SetTrigger(AnimationTriggers.ODMGear.ToString());
        }
        else
        {
            animator.ResetTrigger(AnimationTriggers.ODMGear.ToString());
        }
    }
}
