using UnityEngine;
using UnityEngine.AI;

public class TitanBehaviour : MonoBehaviour
{
    [SerializeField] private TitanSensor sensor;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private TitanAnimator titanAnimator;
    [HideInInspector] public bool isWaiting = false;

    [SerializeField][Min(0)] private float minRelaxDelay;
    [SerializeField][Min(0)] private float maxRelaxDelay;

    [Header("Particle's Settings")]
    [SerializeField] private ParticleSystem smokeParticleSystem;
    [SerializeField] private ParticleSystem[] bloodParticleSystem;

    [HideInInspector] public bool isDead = false;
    private bool wasAddScoreBefore = false;

    [SerializeField] private Rigidbody hipsRb;

    private void Start()
    {
        UseBloodParticle(false);

        if (LoadData.instance.isTitanSmoke)
            smokeParticleSystem.Play();
        else smokeParticleSystem.Stop();

        isDead = false;
    }


    private void Update()
    {
        if (isDead)
            return;

        if(sensor.canAttackPlayer)
        {
            UpdateAttack();
            return;
        }

        if (sensor.canSeePlayer)
        {
            UpdateHunt();
            return;
        }

        UpdateNeutral();
    }

    private void UpdateAttack()
    {
        agent.isStopped = true;
    }

    private void UpdateHunt()
    {
        agent.isStopped = false;
        agent.SetDestination(sensor.PlayerPosition);
    }

    private void UpdateNeutral()
    {
        if ((Vector3.Distance(agent.transform.position, agent.destination) > 1f) || isWaiting)
            return;

        isWaiting = true;
        agent.isStopped = true;
        float waitTime = Random.Range(minRelaxDelay, maxRelaxDelay);
        Invoke("SetNewDestination", waitTime);
    }


    private void SetNewDestination()
    {
        if (sensor.canSeePlayer)
            return;

        Vector3 newDestination = GetNewPatrolPoint();
        agent.SetDestination(newDestination);
        agent.isStopped = false;
        isWaiting = false;
    }

    private Vector3 GetNewPatrolPoint()
    {
        NavMeshHit hit;

        for (int i = 0; i < 1000; i++)
        {
            Vector3 randomPosition = agent.transform.position + Random.insideUnitSphere * sensor.patrolRadius;

            if (NavMesh.SamplePosition(randomPosition, out hit, 1f, NavMesh.AllAreas))
            {
                Debug.Log(gameObject + "Not find position this frame, wait for the next.");
                return hit.position;
            }
        }
        return agent.transform.position;
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
        sensor.shouldSeeTheHuntIndic = false;
        isDead = true;
        agent.isStopped = true;
        UseBloodParticle(true);
        transform.GetComponent<Animator>().enabled = false;

        if (!wasAddScoreBefore)
        {
            wasAddScoreBefore = true;
            ScoreManager.instance.AddScore(1);
        }

        Invoke("EnablePhysicsAfterDead", 2f);
    }

    public void Eat()
    {
        sensor.shouldSeeTheHuntIndic = false;
        titanAnimator.isEating = true;
    }

    private void EnablePhysicsAfterDead()
    {
        hipsRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        smokeParticleSystem.Stop();

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        Destroy(this.gameObject, 3f);
    }
}
