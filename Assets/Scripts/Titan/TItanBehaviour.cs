using UnityEngine;
using UnityEngine.AI;

public class TitanBehaviour : MonoBehaviour
{
    [SerializeField] private TitanSensor sensor;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private TitanAnimator titanAnimator;
    [HideInInspector] public bool isWaiting = false;

    [SerializeField, Min(0)] private float minRelaxDelay;
    [SerializeField, Min(0)] private float maxRelaxDelay;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem smokeParticleSystem;
    [SerializeField] private ParticleSystem[] bloodParticleSystem;

    [HideInInspector] public bool isDead = false;
    public bool shouldPatrol = true;
    private bool hasAddedScore = false;

    [SerializeField] private Rigidbody hipsRb;

    public virtual void Start()
    {
        ActivateBloodParticles(false);

        if (LoadData.instance.isTitanSmoke)
            smokeParticleSystem.Play();
        else
            smokeParticleSystem.Stop();

        isDead = false;
    }

    public virtual void Update()
    {

        if (isDead) return;

        if (sensor.canAttackPlayer)
        {
            HandleAttack();
            return;
        }

        if (sensor.canSeePlayer || !shouldPatrol)
        {
            HandleHunting();
            return;
        }

        HandleNeutralState();
    }

    private void HandleAttack()
    {
        agent.isStopped = true;
    }

    private void HandleHunting()
    {
        agent.isStopped = false;
        agent.SetDestination(sensor.PlayerPosition);
    }

    private void HandleNeutralState()
    {
        if (Vector3.Distance(agent.transform.position, agent.destination) > 1f || isWaiting) return;

        isWaiting = true;
        agent.isStopped = true;
        float waitTime = Random.Range(minRelaxDelay, maxRelaxDelay);
        Invoke(nameof(SetNewDestination), waitTime);
    }

    private void SetNewDestination()
    {
        if (sensor.canSeePlayer) return;

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
                return hit.position;
        }

        Debug.LogWarning($"{gameObject.name} n’a pas trouvé de position après 1000 essais.");
        return agent.transform.position;
    }

    private void ActivateBloodParticles(bool active)
    {
        foreach (var particle in bloodParticleSystem)
        {
            if (active) particle.Play();
            else particle.Stop();
        }
    }

    public virtual void Dead()
    {
        sensor.shouldSeeTheHuntIndic = false;
        isDead = true;
        agent.isStopped = true;
        ActivateBloodParticles(true);
        GetComponent<Animator>().enabled = false;

        if (!hasAddedScore)
        {
            hasAddedScore = true;
            ScoreManager.instance.AddScore(1);
        }

        Invoke(nameof(EnablePhysicsAfterDeath), 2f);
    }

    public void Eat()
    {
        sensor.shouldSeeTheHuntIndic = false;
        titanAnimator.isEating = true;
    }

    private void EnablePhysicsAfterDeath()
    {
        hipsRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        smokeParticleSystem.Stop();

        transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, 0), transform.position.z);

        Destroy(gameObject, 3f);
    }
}
