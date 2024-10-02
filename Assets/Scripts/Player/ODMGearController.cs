using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ODMGearController : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer lrRight;                // Line renderer for the right grappling hook
    [SerializeField] private LineRenderer lrLeft;                 // Line renderer for the left grappling hook

    [Header("Grappling Settings")]
    [SerializeField] private LayerMask ODMGearLayer;              // Layer mask to detect grappling points
    [SerializeField] private Transform gunTipRight;               // Transform of the right gun tip
    [SerializeField] private Transform gunTipLeft;                // Transform of the left gun tip
    [SerializeField] private Transform player;                    // Transform of the player
    [SerializeField] private Transform playerCamera;              // Transform of the player's camera
    [HideInInspector] public Transform ODMGearPoint;              // Transform of the grappling point
    [SerializeField] private GameObject ODMGearPointPrefab;        // Prefab of the ODMGearPoint
    [HideInInspector] public bool canUseODMGear;                  // Bool for can grapple or no.
    [SerializeField] private float maxDistance = 170f;            // Maximum distance for the grappling hook
    [SerializeField] private float ODMGearVelocity = 10;          // Velocity of the player while using the gear

    [Header("Reset Velocity")]
    [SerializeField] private float slowDownFactor;
    [SerializeField] private float minMagnitudeForPlayerController = 0.3f;

    [Header("UI Elements")]
    [SerializeField] private Slider gasSlider;                    // UI slider to display remaining gas

    [Header("Script's References")]
    [SerializeField] private PlayerController playerController;   // PlayerController's reference
    [SerializeField] private ParticleSystem gazParticles;         // Particle system for gas effects
    [SerializeField] private ParticleSystem speedTrailParticles;  // Particle system for speed trail effects

    [Header("Gas")]
    public int maxGas;                                            // Maximum gas capacity
    public int currentGas;                                        // Current gas amount

    // Private Fields
    private Rigidbody playerRb;                                   // Rigidbody of the player
    private SpringJoint joint;                                    // Spring joint for the grappling hook
    public bool isUseODMGear = false;                             // Flag to check if the gear is in use
    private bool isUseGaz = false;                                // Flag to check if gas is being used
    private float distanceFromPoint;                              // Distance from the grappling point

    private void Start()
    {
        currentGas = maxGas;
        gasSlider.maxValue = maxGas;
        playerRb = player.GetComponent<Rigidbody>();
        gazParticles.Stop();
        speedTrailParticles.Stop();
        canUseODMGear = true;
    }

    void Update()
    {
        SetConnectedAunchor();
        HandleInput();
        currentGas = Mathf.Max(currentGas, 0);
        gasSlider.value = currentGas;
    }

    void LateUpdate()
    {
        DrawRope();
    }

   private void FixedUpdate()
    {
        if (isUseODMGear && isUseGaz && LoadData.instance.playerInput.actions["UseGas"].IsPressed())
        {
            UseGas();
        }
    }

    private void HandleInput()
    {
        if (LoadData.instance.playerInput.actions["ODMGear"].WasPerformedThisFrame() && canUseODMGear)
        {
            StartODMGear();

        }
        if (LoadData.instance.playerInput.actions["ODMGear"].WasReleasedThisFrame())
        {
            StopODMGear();
        }
        if (LoadData.instance.playerInput.actions["UseGas"].WasPerformedThisFrame() && isUseODMGear)
        {
            if (currentGas != 0)
            {
                isUseGaz = true;
                speedTrailParticles.Play();
                gazParticles.Play();
            }
        }
        if (LoadData.instance.playerInput.actions["UseGas"].WasReleasedThisFrame())
        {
            StopUsingGas();
        }
    }

    private void UseGas()
    {
        Vector2 moveInput = LoadData.instance.playerInput.actions["Move"].ReadValue<Vector2>();
        Debug.Log(moveInput);

        Vector3 ODMGearDirection = Vector3.zero;

        if (moveInput.x < 0)
            ODMGearDirection += (playerCamera.right * -1);

        if (moveInput.x > 0)
            ODMGearDirection += playerCamera.right;

        if (moveInput.y > 0)
            ODMGearDirection += playerCamera.up;

        if (moveInput.y < 0)
            ODMGearDirection += (playerCamera.up * -1);

        if (!LoadData.instance.playerInput.actions["Sprint"].IsPressed())
            ODMGearDirection += playerCamera.forward;

        playerRb.AddForce(ODMGearDirection.normalized * ODMGearVelocity);

        currentGas -= 1;


        player.transform.LookAt(ODMGearPoint);
    }

    private void StopUsingGas()
    {
        speedTrailParticles.Stop();
        gazParticles.Stop();
        isUseGaz = false;
        if (joint)
            joint.spring = 4.5f;
    }

    private void SetConnectedAunchor()
    {
        if (joint)
        {
            joint.connectedAnchor = ODMGearPoint.position;
        }
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

            isUseODMGear = true;
            playerController.canMove = false;
            DrawRope();
        }
    }

    private void CreateSpringJoint()
    {
        if (joint == null)
        {
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = ODMGearPoint.position;
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
            distanceFromPoint = Vector3.Distance(player.position, ODMGearPoint.position);
            joint.maxDistance = distanceFromPoint;
            joint.minDistance = 0;
        }
    }

    public void StopODMGear()
    {
        gazParticles.Stop();
        speedTrailParticles.Stop();

        lrRight.positionCount = 0;
        lrLeft.positionCount = 0;

        if (joint != null)
        {
            Destroy(joint);
        }

        isUseODMGear = false;

        StartCoroutine(SlowDownPlayer());
    }

    private IEnumerator SlowDownPlayer()
    {
        while ((Mathf.Abs(playerRb.velocity.x) > minMagnitudeForPlayerController && Mathf.Abs(playerRb.velocity.z) > minMagnitudeForPlayerController) && !isUseODMGear && !playerController.isGrounded)
        {
            playerRb.velocity *= slowDownFactor;

            playerRb.velocity = new Vector3(playerRb.velocity.x, playerController.SetVector3Gravity(playerRb.velocity).y - playerRb.velocity.y, playerRb.velocity.z);

            yield return new WaitForFixedUpdate();
        }

        if (!isUseODMGear && canUseODMGear)
        {
            playerRb.velocity = Vector3.zero;
            playerController.canMove = true;
        }
    }



    private void DrawRope()
    {
        if (joint == null)
        {
            lrRight.positionCount = 0;
            lrLeft.positionCount = 0;
            return;
        }

        if (lrRight.positionCount != 2)
        {
            lrRight.positionCount = 2;
        }

        if (lrLeft.positionCount != 2)
        {
            lrLeft.positionCount = 2;
        }

        lrRight.SetPosition(0, gunTipRight.position);
        lrRight.SetPosition(1, ODMGearPoint.position);

        lrLeft.SetPosition(0, gunTipLeft.position);
        lrLeft.SetPosition(1, ODMGearPoint.position);
    }

    public void FillGas(int amount)
    {
        currentGas = Mathf.Min(currentGas + amount, maxGas);
    }
}
