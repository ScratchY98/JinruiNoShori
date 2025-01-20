using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using static UnityEngine.AudioSettings;
using System.Linq.Expressions;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Component's Reference :")]
    [SerializeField] private Transform player;
    private PlayerInput playerInput;

    [Header("Script's Reference :")]
    [SerializeField] private ODMGas ODMGasRef;


    [Header("Limit's Settings :")]
    [SerializeField] private float maxXRot = 90f;
    [SerializeField] private float minXRot = -75f;

    [Header("Speed settings :")]
    [SerializeField] private float rotationSpeed = 5.0f;

    [Header("Distance and offset settings :")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, 0);
    [SerializeField] private float WallDistance = 0.4f;
    [SerializeField] private float setDistanceTimeDuration = 0.5f;
    [SerializeField] private float initialDistance;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float ODMGearDistance = 8.0f;
    private bool isCoroutine;

    [Header("Sensitivity Settings")]
    [SerializeField] private float gamepadSensitivity;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float lerpSpeed = 12.5f;

    // Rotations settings
    private Quaternion rot;
    private Vector2 rotation;

    private bool canMoveCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        initialDistance = distance;
        isCoroutine = false;
        canMoveCamera = true;

        playerInput = LoadData.instance.playerInput;
        ChangeSensitivity(PlayerPrefs.GetFloat("MouseSensibilityData", 750f), false);
        ChangeSensitivity(PlayerPrefs.GetFloat("GamepadSensibilityData", 50f), true);
    }

    private void LateUpdate()
    {
        if (!canMoveCamera)
            return;

        HandleCameraRotation();
        HandleCameraPosition();

        float targetDistance = ODMGasRef.IsUseODMGear() ? ODMGearDistance : initialDistance;
        bool shouldStartCoroutine = !isCoroutine && distance != targetDistance;

        if (shouldStartCoroutine)
            StartCoroutine(SetDistanceCoroutine(ODMGasRef.IsUseODMGear()));

        rot = transform.rotation;
    }

    private void HandleCameraRotation()
    {
        float sensitivity = playerInput.currentControlScheme == "Keyboard" ? mouseSensitivity : gamepadSensitivity;

        Vector2 lookInput =  playerInput.actions["Look"].ReadValue<Vector2>();

        // Appliquer la vitesse de rotation
        Vector2 lookOutput = new Vector2(lookInput.x, lookInput.y) * rotationSpeed * Time.deltaTime * sensitivity;

        rotation.x -= lookOutput.y;
        rotation.y += lookOutput.x;

        rotation.x = Mathf.Clamp(rotation.x, minXRot, maxXRot);

        Quaternion cameraRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraRotation, Time.deltaTime * lerpSpeed);

    }

    public void ChangeSensitivity(float newSensitivity, bool isGamepad)
    {
        if (isGamepad) gamepadSensitivity = newSensitivity;
        else mouseSensitivity = newSensitivity;
    }

    private void HandleCameraPosition()
    {
        Vector3 offset = player.position - transform.forward * distance + cameraOffset;

        RaycastHit hit;
        if (Physics.Raycast(player.position, -transform.forward, out hit, distance))
            offset = hit.point + transform.forward * WallDistance;

        transform.position = offset;
    }

    private IEnumerator SetDistanceCoroutine(bool useGear)
    {
        isCoroutine = true;
        float elapsedTime = 0f;
        float startDistance = useGear ? initialDistance : ODMGearDistance;
        float endDistance = useGear ? ODMGearDistance : initialDistance;

        while (elapsedTime < setDistanceTimeDuration)
        {
            distance = Mathf.Lerp(startDistance, endDistance, elapsedTime / setDistanceTimeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        distance = endDistance;
        isCoroutine = false;
    }

    public void ActiveDeathCamera(Vector3 pos, Quaternion rot)
    {
        canMoveCamera = false;
        transform.position = pos;
        transform.rotation = rot;
    }
}
