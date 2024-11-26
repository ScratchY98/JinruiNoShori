using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public enum WhatGizmosDraw
{
    PatrolRadius,
    huntingDetectionRadius,
    AttackDetectionRadius,
    Destination
}


public class TitanController : MonoBehaviour
{
    [Header("Components's References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody hipsRb;
    [SerializeField] private static Transform player;

    [Header("Hunting's Settings")]
    [SerializeField][Min(0)] private float huntingDetectionRadius = 200f;
    [SerializeField][Min(0)] private float huntingSpeed = 5f;
    [HideInInspector] public bool isHunting = false;
    [SerializeField] private GameObject huntingIndication;

    [Header("Patrol's Settings")]
    [SerializeField][Min(0)] private float patrolRadius = 10f;
    [SerializeField][Min(0)] private float patrolSpeed = 3f;
    private Vector3 patrolDestination;
    private bool isPatrolling = true;
    private bool isFirstPatrol = true;
    [HideInInspector] public bool isRelax= false;
    [SerializeField] [Min(0)] private float minRelaxDelay;
    [SerializeField][Min(0)] private float maxRelaxDelay;

    [Header("Attack's Settings")]
    [SerializeField][Min(0)] private float attackDetectionRadius = 5f;
    [HideInInspector] public bool isAttacking = false;

    [Header("Particle's Settings")]
    [SerializeField] private ParticleSystem smokeParticleSystem;
    [SerializeField] private ParticleSystem[] bloodParticleSystem;

    [Header("IK's Settings")]
    [SerializeField][Min(0)] private float ikWeight = 0f;
    [SerializeField][Min(0)] private float ikSpeed = 2f;
    [SerializeField][Min(0)] private float maxIKWeight = 1f;
    public static Transform grab;

    [Header("Eat's Settings")]
    [HideInInspector] public bool isEating = false;

    [Header("Other's Settings")]
    [SerializeField][Min(0)] private float destinationGizmosSIze = 5f;
    [SerializeField] private Transform originRadius;
    [HideInInspector] public bool isDead;
    [SerializeField] private WhatGizmosDraw whatGizmosDraw;
    private bool canMove = true;

    private void Start()
    {
        UseBloodParticle(false);

        if (LoadData.instance.isTitanSmoke)
            smokeParticleSystem.Play();
        else smokeParticleSystem.Stop();

        canMove = true;
        isEating = false;
        isDead = false;
  
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if (grab == null)
            grab = GameObject.FindGameObjectWithTag("Grab").transform;
    }

    private void Update()
    {
        if (!canMove)
        return;

        CheckStatus();

        if (!isHunting)
        {
            huntingIndication.SetActive(false);

            if (isPatrolling)
                Patrol();

            return;

        }
        
        Hunting();

    }

    private void CheckStatus()
    {
        if (isRelax)
            return;

        if (Vector3.Distance(originRadius.position, player.position) <= huntingDetectionRadius)
        {
            isPatrolling = false;

            if (Vector3.Distance(originRadius.position, player.position) <= attackDetectionRadius)
            {
                isAttacking = true;
                isHunting = false;
            }
            else
            {
                isHunting = true;
                isAttacking = false;
            }
        }
        else
        {
            isHunting = false;
            isPatrolling = true;
            isAttacking = false;
        }
    }
    private void Hunting()
    {
        huntingIndication.SetActive(true);
        agent.speed = huntingSpeed;
        agent.destination = SamplePos(player.position);;
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (!isPatrolling)
            return;

        if (ComparePosWithoutYAxis(transform.position, agent.destination))
        {
            if (isFirstPatrol)
            {
                isFirstPatrol = false;
                RelaxAndChoice();
            }
            else
            {
                isRelax = true;
                agent.speed = 0;
                Invoke("RelaxAndChoice", Random.Range(minRelaxDelay, maxRelaxDelay));
            }
        }
    }

    private void RelaxAndChoice()
    {
        isRelax = false;
        agent.speed = patrolSpeed;
        patrolDestination = GetRandomPatrolPoint();
        agent.destination = patrolDestination;
    }

    private Vector3 SamplePos(Vector3 target)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(target, out hit, patrolRadius, 1);
        return hit.position;
    }

    private bool ComparePosWithoutYAxis(Vector3 firstPos, Vector3 secondPos)
    {
        if (new Vector3(firstPos.x, 0, firstPos.z) == new Vector3(secondPos.x, 0, secondPos.z))
            return true;
        else return false;
    }

    private Vector3 GetRandomPatrolPoint()
    {
        return SamplePos((Random.insideUnitSphere * patrolRadius) + transform.position);
    }

    private void UseBloodParticle(bool active)
    {
        for (int i = 0; i < bloodParticleSystem.Length; i++)
        {
            if (active)
                bloodParticleSystem[i].Play();
            else
                bloodParticleSystem[i].Stop();
        }
    }

    public void Dead()
    {
        Debug.Log("Dead !");
        huntingIndication.SetActive(false);
        isDead = true;
        canMove = false;
        agent.isStopped = true;
        UseBloodParticle(true);
        transform.GetComponent<Animator>().enabled = false;
        ScoreManager.instance.AddScore(1);
        StartCoroutine(EnablePhysicsAfterDead());
    }

    public void Eat()
    {
        huntingIndication.SetActive(false);
        canMove = false;
        isHunting = false;
        isPatrolling = false;
        isAttacking = false;
        isEating = true;
    }
    IEnumerator EnablePhysicsAfterDead()
    {
        yield return new WaitForSeconds(2);

        hipsRb.constraints = RigidbodyConstraints.FreezePositionX| RigidbodyConstraints.FreezePositionZ;
        smokeParticleSystem.Stop();

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        Destroy(this.gameObject, 3f);

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isAttacking && !isEating && Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(grab.position)) > 0)
        {
            ikWeight = Mathf.MoveTowards(ikWeight, maxIKWeight, ikSpeed * Time.deltaTime);

            // Appliquer l'IK pour les mains
            SetHandsIK(AvatarIKGoal.RightHand, grab);
            SetHandsIK(AvatarIKGoal.LeftHand, grab);

            // Appliquer l'IK pour la tête
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

    private void OnDrawGizmos()
    {
        switch (whatGizmosDraw)
        {
            case WhatGizmosDraw.PatrolRadius:
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(originRadius.position, patrolRadius);
                break;
            case WhatGizmosDraw.huntingDetectionRadius:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(originRadius.position, huntingDetectionRadius);
                break;
            case WhatGizmosDraw.AttackDetectionRadius:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(originRadius.position, attackDetectionRadius);
                break;
            case WhatGizmosDraw.Destination:
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(agent.destination, destinationGizmosSIze);
                break;
            default:
                break;
        }
    }
}
