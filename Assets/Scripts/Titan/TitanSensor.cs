using UnityEngine;

public enum WhatGizmosDraw { PatrolRadius, huntingDetectionRadius, AttackDetectionRadius }

public class TitanSensor : MonoBehaviour
{
    [SerializeField] private RangeAngleDetection visionDetection =  new RangeAngleDetection(5f, 90f);
    [SerializeField] private RangeAngleDetection attackDetection = new RangeAngleDetection(2f, 90f);
    [SerializeField] private GameObject huntingIndication;
    public float patrolRadius;
    [SerializeField] private Transform originRadius;
    [SerializeField] private Transform _player;

    private bool _canSeePlayer = false;
    private bool _canAttackPlayer = false;

    public bool canSeePlayer => _canSeePlayer;
    public bool canAttackPlayer => _canAttackPlayer;
    public Vector3 PlayerPosition => _player.position;
    [SerializeField] private WhatGizmosDraw whatGizmosDraw;
    [HideInInspector] public bool shouldSeeTheHuntIndic = true;

    private void Update()
    {
        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 playerDelta = _player.position - transform.position;
        float distanceToPlayer = playerDelta.magnitude;

        _canSeePlayer = visionDetection.IsAngleDetected(transform.position, _player.position, transform.forward);
        _canAttackPlayer = attackDetection.IsAngleDetected(transform.position, _player.position, transform.forward);

        huntingIndication.SetActive(_canSeePlayer && shouldSeeTheHuntIndic);
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
                Gizmos.DrawWireSphere(originRadius.position, visionDetection.range);
                break;
            case WhatGizmosDraw.AttackDetectionRadius:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(originRadius.position, attackDetection.range);
                break;
            default:
                break;
        }
    }
}