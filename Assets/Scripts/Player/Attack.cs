using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float AttackDistance = 3f;
    [SerializeField] private SpawnTitan spawnTitan;
    public LayerMask EnemyLayer;
    [SerializeField] private ODMGearController ODMGearControllerRef;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool canAttack;
    [SerializeField] private Transform player;

    private void Start()
    {
        canAttack = true;
    }

    private void Update()
    {

        Debug.DrawRay(player.position, player.forward * AttackDistance, Color.green);

        if (Input.GetKey(LoadData.instance.attack) && canAttack)
        {
            isAttacking = true;
            Attack();
        }
        else
            isAttacking = false;
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
                spawnTitan.SpawnTitanAtRandomPosition();

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
