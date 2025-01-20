using UnityEngine;
using UnityEngine.AI;


public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private AISensor sensor;
    [SerializeField] private NavMeshAgent agent;

    private void Update()
    {
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
        print("Attacking");
        agent.isStopped = true;
    }

    private void UpdateHunt()
    {
        agent.isStopped = false;
        agent.SetDestination(sensor.PlayerPosition);
    }

    private void UpdateNeutral()
    {
        agent.isStopped = false;

        if (Vector3.Distance(agent.destination, agent.transform.position) > 1f)
            return;

        Vector3 newDestination = GetNewPatrolPoint();
        agent.SetDestination(newDestination);
    }

    private Vector3 GetNewPatrolPoint()
    {
        NavMeshHit hit;

        for(int i = 0; i < 1000; i ++)
        {
            Vector3 _randomPosition = agent.transform.position + Random.insideUnitSphere * sensor.patrolRadius;

            NavMesh.SamplePosition(_randomPosition, out hit, 1f, 1);

            if (hit.hit) return hit.position;
        }

        return agent.transform.position;
    }
}
