using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    [Header("Scripts's References")]
    [SerializeField] private ODMGearController ODMGearControllerRef;
    [SerializeField] private AudioClip playerAttackAudio;

    [Header("Others")]
    [SerializeField] [Min(0)] private float AttackDistance = 3f;
    [SerializeField] private Transform playerCamera;

    private PlayerInput playerInput;
    public LayerMask EnemyLayer;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool canAttack;
    bool isAttackInput;


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
        isAttackInput = playerInput.actions["Attack"].WasPerformedThisFrame();

        RaycastHit hit;
        if (isAttackInput)
            AudioManager.instance.PlayClipAt(playerAttackAudio, transform.position);

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, AttackDistance, EnemyLayer))
        {
            TitanBehaviour titanBehaviour = GetTitanControllerFromParent(hit.collider.gameObject, 6);

            if (titanBehaviour == null) return;

            if (ODMGearControllerRef.ODMGearPoint != null)
                ODMGearControllerRef.ODMGearPoint.parent = null;

            ODMGearControllerRef.StopODMGear();
            titanBehaviour.Dead();

            SpawnObject.instance.SpawnObjectsAtRandomPosition(0);
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
