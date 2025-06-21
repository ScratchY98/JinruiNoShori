using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Component's Reference;")]
    [SerializeField] private Transform playerCamera;
    private ODMGas ODMGasRef;

    [Header("Speed")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 7f;
    private float speed;
    [HideInInspector] public bool isRunning;
    [SerializeField] private float maxVelocity = 10f;

    [Header("GroundCheck:")]
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 1f;
    [SerializeField] private float groundDistance;
    [HideInInspector] public bool canMove;

    [HideInInspector] public Vector3 inputDirection;
    private Rigidbody rb;
    private bool isGrounded;
    private PlayerInput playerInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ODMGasRef = GetComponent<ODMGas>();
        playerInput = LoadData.instance.playerInput;
        canMove = true;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundDistance, Color.green);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);
        if (!isGrounded || ODMGasRef.IsUseODMGear())
        {
            canMove = false;
            rb.linearDamping = airDrag;
        }
        else
        {
            canMove = true;
            rb.linearDamping = groundDrag;
        }

        rb.linearDamping = isGrounded ? groundDrag : airDrag;

        isRunning = playerInput.actions["Sprint"].IsPressed();
        speed = isRunning ? sprintSpeed : walkSpeed;
    }

    void FixedUpdate()
    {
        if (!canMove)
            return;

        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        inputDirection = new Vector3 (input.x, 0f, input.y).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirection.Normalize();


            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10.0f);

            rb.AddForce(moveDirection * speed, ForceMode.Force);

            if (rb.linearVelocity.magnitude > maxVelocity)
                rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
        }
    }
}
