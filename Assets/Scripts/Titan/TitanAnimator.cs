using System;
using UnityEngine;

public class TitanAnimator : MonoBehaviour
{
    private enum AnimationTriggers { Walk, Attack, Eat, Idle }
    [SerializeField] private Animator animator;
    [SerializeField] private TitanSensor sensor;
    [SerializeField] private TitanBehaviour titanBehaviour;

    [Header("IK's Settings")]
    [SerializeField][Min(0)] private float ikWeight = 0f;
    [SerializeField][Min(0)] private float ikSpeed = 2f;
    [SerializeField][Min(0)] private float maxIKWeight = 1f;
    public static Transform grab;

    [HideInInspector] public bool isEating;


    private void ResetAllAnimationTriggers()
    {
        foreach (AnimationTriggers trigger in Enum.GetValues(typeof(AnimationTriggers)))
        {
            animator.ResetTrigger(trigger.ToString());
        }
    }

    private void SetTrigger(AnimationTriggers animation)
    {
        ResetAllAnimationTriggers();
        animator.SetTrigger(animation.ToString());
    }

    private void Start()
    {
        if (grab == null)
            grab = GameObject.FindGameObjectWithTag("Grab").transform;
    }

    private void Update()
    {
        WalkAnimation();
        AttackAnimation();
        IdleAnimation();
        EatAnimation();
    }

    void WalkAnimation()
    {
        if (!sensor.canAttackPlayer && !isEating && !titanBehaviour.isWaiting)
        {
            SetTrigger(AnimationTriggers.Walk);
        }
    }

    void AttackAnimation()
    {
        if (sensor.canAttackPlayer && !isEating && !titanBehaviour.isWaiting)
        {
            SetTrigger(AnimationTriggers.Attack);
        }
    }

    void EatAnimation()
    {
        if (isEating)
        {
            SetTrigger(AnimationTriggers.Eat);
        }
    }

    void IdleAnimation()
    {
        if (titanBehaviour.isWaiting && !isEating)
        {
            SetTrigger(AnimationTriggers.Idle);
        }
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (sensor.canAttackPlayer && !isEating && Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(grab.position)) > 0)
        {
            ikWeight = Mathf.MoveTowards(ikWeight, maxIKWeight, ikSpeed * Time.deltaTime);

            SetHandsIK(AvatarIKGoal.RightHand, grab);
            SetHandsIK(AvatarIKGoal.LeftHand, grab);

            SetHeadIK(grab);
        }
        else
            ikWeight = 0;
    }

    private void SetHandsIK(AvatarIKGoal IkGoal, Transform target)
    {
        animator.SetIKPositionWeight(IkGoal, ikWeight);

        animator.SetIKPosition(IkGoal, target.position);
    }

    private void SetHeadIK(Transform target)
    {
        animator.SetLookAtWeight(ikWeight);
        animator.SetLookAtPosition(target.position);
    }
}
