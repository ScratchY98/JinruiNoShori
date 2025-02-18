using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Scripts's References")]
    [SerializeField] private ODMGearController ODMGearControllerRef;

    [Header("Others")]
    [SerializeField] [Min(0)] private float AttackDistance = 3f;
    [SerializeField] private Transform playerCamera;
    private PlayerInput playerInput;
    public LayerMask EnemyLayer;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool canAttack;

    private void Start()
    {
        canAttack = true;
        playerInput = LoadData.instance.playerInput;
    }

    private void Update()
    {
        Debug.DrawRay(playerCamera.position, playerCamera.forward * AttackDistance, Color.green);
        isAttacking = playerInput.actions["Attack"].IsPressed() && canAttack;

        if (isAttacking)
            Attack();
    }

    private void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, AttackDistance, EnemyLayer))
        {
            Debug.Log("Attack", hit.collider.gameObject);
            TitanBehaviour titanBehaviour = GetTitanControllerFromParent(hit.collider.gameObject, 6);

            if (titanBehaviour != null)
            {
                if (ODMGearControllerRef.ODMGearPoint != null)
                    ODMGearControllerRef.ODMGearPoint.parent = null;

                ODMGearControllerRef.StopODMGear();
                titanBehaviour.Dead();
                SpawnObject.instance.SpawnObjectsAtRandomPosition(0);

            }
        }
    }

    public TitanBehaviour GetTitanControllerFromParent(GameObject obj, int parentnumber)
    {
        Transform goodParent = obj.transform;

        for (int i = 0; i < parentnumber; i++)
        {
            if (goodParent == null)
            {
                Debug.LogWarning("Can't find fithParent");
                return null;
            }

            TitanBehaviour titanBehaviour = goodParent.GetComponent<TitanBehaviour>();
            if (titanBehaviour != null)
                return titanBehaviour;

            goodParent = goodParent.parent;
        }

        Debug.LogWarning("Can't find TitanBehaviour !");
        return null;
    }
}
