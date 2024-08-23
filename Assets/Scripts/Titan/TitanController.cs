using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class TitanController : MonoBehaviour
{
    [Header("Components's References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody hipsRb;
    [SerializeField] private static Transform player;

    [Header("Hunting's Settings")]
    [SerializeField] private float huntingDetectionRadius = 200f;
    [SerializeField] private float huntingSpeed = 5f;
    [HideInInspector] public bool isHunting = false;

    [Header("Patrol's Settings")]
    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float patrolSpeed = 3f;
    private Vector3 patrolDestination;
    private bool isPatrolling = true;

    [Header("Attack's Settings")]
    [SerializeField] private float attackDetectionRadius = 5f;
    [HideInInspector] public bool isAttacking = false;

    [Header("Particle's Settings")]
    [SerializeField] private ParticleSystem smokeParticleSystem;
    [SerializeField] private ParticleSystem[] bloodParticleSystem;

    [Header("IK's Settings")]
    [SerializeField] private static Transform grab;

    [Header("Eat's Settings")]
    [HideInInspector] public bool isEating = false;

    [Header("Other's Settings")]
    [SerializeField] private float destinationGizmosSIze = 5f;
    [SerializeField] private Transform originRadius;
    [HideInInspector] public bool isDead;
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
        if (canMove)
        {
            CheckStatus();

            if (isHunting)
            {
                Hunting();
            }
            else if (isPatrolling)
            {
                Patrol();
            }
        }
    }

    private void CheckStatus()
    {
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
        agent.speed = huntingSpeed;
        agent.destination = SamplePos(player.position);;
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if(ComparePosWithoutYAxis(transform.position, agent.destination))
            patrolDestination = GetRandomPatrolPoint();

        if (isPatrolling)
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
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        return SamplePos(randomDirection);
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
        if (isAttacking && !isEating)
        {
            float dotProduct = Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(grab.position));

            if (dotProduct > 0)
            {
                SetHandsIK(AvatarIKGoal.RightHand, grab);
                SetHandsIK(AvatarIKGoal.LeftHand, grab);

                SetHeadIK(grab);
            }
        }
    }

    private void SetHandsIK(AvatarIKGoal IkGoal, Transform target)
    {
        animator.SetIKPositionWeight(IkGoal, 1);

        animator.SetIKPosition(IkGoal, target.position);
    }

    private void SetHeadIK(Transform target)
    {
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(target.position);
    }

    private void OnDrawGizmos()
    {
       Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(originRadius.position, patrolRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(agent.destination, destinationGizmosSIze);
        /*
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(originRadius.position, huntingDetectionRadius);
       */
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(originRadius.position, attackDetectionRadius);
    }
}
