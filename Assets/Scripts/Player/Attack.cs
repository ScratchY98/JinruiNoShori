using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Scripts's References")]
    [SerializeField] private ODMGearController ODMGearControllerRef;

    [Header("Others")]
    [SerializeField] private float AttackDistance = 3f;
    [SerializeField] private Transform player;
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
        Debug.DrawRay(player.position, player.forward * AttackDistance, Color.green);
        isAttacking = playerInput.actions["Attack"].IsPressed() && canAttack;

        if (isAttacking)
            Attack();
    }

    private void Attack()
    {
        RaycastHit hit;


        if (Physics.Raycast(player.position, player.forward, out hit, AttackDistance, EnemyLayer))
        {
            TitanController titanController = GetTitanControllerFromParent(hit.collider.gameObject, 6);

            if (titanController != null)
            {
                ODMGearControllerRef.ODMGearPoint.parent = null;
                ODMGearControllerRef.StopODMGear();
                titanController.Dead();
                SpawnObject.instance.SpawnObjectsAtRandomPosition(0);

            }
        }
    }

    public TitanController GetTitanControllerFromParent(GameObject obj, int parentnumber)
    {
        Transform goodParent = obj.transform;

        for (int i = 0; i < parentnumber; i++)
        {
            if (goodParent == null)
            {
                Debug.Log("Can't find fithParent");
                return null;
            }

            TitanController titanController = goodParent.GetComponent<TitanController>();
            if (titanController != null)
            {
                return titanController;
            }

            goodParent = goodParent.parent;
        }

        Debug.Log("Can't find TitanController");
        return null;
    }
}
