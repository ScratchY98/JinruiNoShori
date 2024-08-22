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
    public Transform ODMGearPoint;                                // Transform of the grappling point
    [HideInInspector] public bool canUseODMGear;                     // Bool for can grapple or no.
    [SerializeField] private float maxDistance = 170f;            // Maximum distance for the grappling hook
    [SerializeField] private float ODMGearVelocity = 10;          // Velocity of the player while using the gear

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
    private bool isUsingGaz = false;                              // Flag to check if gas is being used
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
        if (isUseODMGear && isUsingGaz && Input.GetKey(LoadData.instance.useGas))
        {
            UseGas();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(LoadData.instance.ODMGear) && canUseODMGear)
        {
            StartODMGear();

        }
        if (Input.GetKeyUp(LoadData.instance.ODMGear))
        {
            StopODMGear();
        }
        if (Input.GetKeyDown(LoadData.instance.useGas) && isUseODMGear)
        {
            if (currentGas != 0)
            {
                isUsingGaz = true;
                speedTrailParticles.Play();
                gazParticles.Play();
            }
        }
        if (Input.GetKeyUp(LoadData.instance.useGas))
        {
            StopUsingGas();
        }
    }

    private void UseGas()
    {
        Vector3 ODMGearDirection = Vector3.zero;

        if (Input.GetKey(LoadData.instance.left))
            ODMGearDirection += (playerCamera.right * -1);

        if (Input.GetKey(LoadData.instance.right))
            ODMGearDirection += playerCamera.right;

        if (Input.GetKey(LoadData.instance.up))
            ODMGearDirection += playerCamera.up;

        if (Input.GetKey(LoadData.instance.down))
            ODMGearDirection += (playerCamera.up * -1);

        if (!Input.GetKey(LoadData.instance.sprint))
            ODMGearDirection += playerCamera.forward;

        playerRb.AddForce(ODMGearDirection.normalized * ODMGearVelocity);

        currentGas -= 1;


        player.transform.LookAt(ODMGearPoint);
    }

    private void StopUsingGas()
    {
        speedTrailParticles.Stop();
        gazParticles.Stop();
        isUsingGaz = false;
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
           // if (!ODMGearPoint)
           //     Instantiate(gameObject, hit.point);
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
        playerController.canMove = true;

        ToggleFreezeRotationY();
    }

    private void ToggleFreezeRotationY()
    {
        if ((playerRb.constraints & RigidbodyConstraints.FreezeRotationY) != 0)
        {
            playerRb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            playerRb.constraints |= RigidbodyConstraints.FreezeRotationY;
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
