using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ODMGearController : MonoBehaviour
{

    [Header("Visuals")]
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform gunTip;

    [Header("Grappling Settings")]
    [SerializeField] private LayerMask ODMGearLayer;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCamera;
    [HideInInspector] public Transform ODMGearPoint;
    [SerializeField] private GameObject ODMGearPointPrefab;
    [SerializeField] private float ODMGearVelocity = 10;
    [SerializeField] private ODMGearController odm;

    [Header("Joint Settings")]
    [SerializeField] private float jointTolerance;
    [SerializeField] private float maxDistance = 170f;

    [Header("Input")]
    [SerializeField] private bool isLeft;
    private PlayerInput playerInput;
    private string rewindInput;
    private string grapplingInput;


    private Rigidbody playerRb;
    public static bool canUseODMGear;
    [HideInInspector] public SpringJoint joint;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        canUseODMGear = true;
        playerInput = LoadData.instance.playerInput;
        rewindInput = isLeft ? "Left Rewind" : "Right Rewind";
        grapplingInput = isLeft ? "Left ODMGear" : "Right ODMGear"; // Gear, don't "Geass" or "Code Geass" (;
    }
    private void LateUpdate()
    {
        DrawRope();
    }

    private void Update()
    {
        HandleInput();

        if (joint)
            UpdateJoint();
    }

    private void FixedUpdate()
    {
        if (playerInput.actions[rewindInput].IsPressed())
            Rewind();
        else if (!playerRb.useGravity)
            playerRb.useGravity = true;

    }

    private void HandleInput()
    {
        if (playerInput.actions[grapplingInput].WasPerformedThisFrame() && canUseODMGear)
            StartODMGear();

        if (playerInput.actions[grapplingInput].WasReleasedThisFrame())
            StopODMGear();
    }

    private void Rewind()
    {
        if (!joint)
            return;

        playerRb.useGravity = false;

        Vector3 direction = (ODMGearPoint.position - player.position).normalized;
        playerRb.AddForce(direction * ODMGearVelocity * Time.deltaTime, ForceMode.Impulse);
    }

    private void StartODMGear()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, maxDistance, ODMGearLayer))
        {

            if (ODMGearPoint == null)
            {
                Transform odmGearPoint = Instantiate(ODMGearPointPrefab.transform);
                odmGearPoint.name = "ODMGearPoint";
                ODMGearPoint = odmGearPoint;
            }

            ODMGearPoint.position = hit.point;
            ODMGearPoint.parent = hit.transform;

            player.transform.LookAt(ODMGearPoint);
            CreateSpringJoint();
            SetConnectedAunchor();
            DrawRope();
        }

    }

    public void StopODMGear()
    {
        lr.positionCount = 0;

        if (joint != null)
            Destroy(joint);
    }

    private void CreateSpringJoint()
    {
        if (joint != null)
            return;

        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = ODMGearPoint.position;
        joint.spring = 4.5f;
        joint.damper = 10;
        joint.massScale = 4.5f;
        joint.tolerance = jointTolerance; 
        joint.maxDistance = Vector3.Distance(player.position, ODMGearPoint.position);
        joint.minDistance = 0;
    }

    private void SetConnectedAunchor()
    {
        if (!joint)
            return;

        joint.connectedAnchor = ODMGearPoint.position;
    }

    private void UpdateJoint()
    {
        joint.maxDistance = Vector3.Distance(player.position, ODMGearPoint.position);

        if (joint.maxDistance < 0)
            joint.maxDistance = 0;
    }

    private void DrawRope()
    {
        if (joint == null)
        {
            lr.positionCount = 0;
            return;
        }

        if (lr.positionCount != 2)
        {
            lr.positionCount = 2;
        }

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, ODMGearPoint.position);
    }

    void OnValidate()
    {
        if (odm != null)
            odm.odm = this;
    }
}


