using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Scripts's References")]
    [SerializeField] private ODMGearController ODMGearControllerRef;
    [SerializeField] private PlayerAttack playerAttack;

    [Header("Speed")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [HideInInspector] public bool isRunning = false;
    private float currentMoveSpeed;

    [Header("Components's References")]
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Transform playerCamera;

    [Header("Ground Checks And Gravity")]
    [SerializeField] private float GravityMultiplier = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask collisionLayers;
    [HideInInspector] public bool isGrounded;

    // Others
    [HideInInspector] public Vector3 move;
    private Vector3 moveDirection;
    [HideInInspector] public bool canMove;

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, collisionLayers);
        isGrounded = colliders.Length > 0;

        if (canMove)
        {
            float horizontalInput = LoadData.instance.GetHorizontalInput();
            float verticalInput = LoadData.instance.GetVerticalInput();

            Vector3 cameraForward = playerCamera.forward;
            Vector3 cameraRight = playerCamera.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            moveDirection = cameraForward.normalized * verticalInput + cameraRight.normalized * horizontalInput;

            moveDirection.Normalize();


            isRunning = Input.GetKey(LoadData.instance.sprint);
            currentMoveSpeed = isRunning ? sprintSpeed : moveSpeed;

            SetMove();

            if (!playerRb.isKinematic)
            {
                playerRb.velocity = move;

                if (moveDirection != Vector3.zero)
                {
                    Quaternion newRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10.0f);
                }
            }
        }
    }

    private void SetMove()
    {
        move = moveDirection * currentMoveSpeed;

        move = SetVector3Gravity(move);     
    }

    public Vector3 SetVector3Gravity(Vector3 target)
    {
        target.y = ((Physics.gravity.y * playerRb.mass)) * GravityMultiplier;
        return target;
    }

    private void Update()
    {
        if (!ODMGearControllerRef.isUseODMGear)
        {
            Vector3 Zrotation = transform.rotation.eulerAngles;

            Zrotation.z = 0f;

            transform.rotation = Quaternion.Euler(Zrotation);

            Vector3 Xrotation = transform.rotation.eulerAngles;

            Xrotation.x = 0f;

            transform.rotation = Quaternion.Euler(Xrotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}