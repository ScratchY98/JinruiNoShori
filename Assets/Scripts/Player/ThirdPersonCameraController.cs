using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header ("Component's Reference :")]
    [SerializeField] private Transform player;
    private PlayerInput playerInput;

    [Header("Script's Reference :")]
    [SerializeField] private ODMGas ODMGasRef;


    [Header("Limit's Settings :")]
    [SerializeField] private float maxXRot = 90f;
    [SerializeField] private float minXRot = -75f;

    [Header("Speed settings :")]
    [SerializeField] private float rotationSpeed = 5.0f;

    [Header ("Distance and offset settings :")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, 0);
    [SerializeField] private float WallDistance = 0.4f;
    [SerializeField] private float setDistanceTimeDuration = 0.5f;
    [SerializeField] private float initialDistance;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float ODMGearDistance = 8.0f;
    private bool isCoroutine;

    // Sensitivity Settings
    private float gamepadSensitivity;
    private float mouseSensitivity;

    // Rotations settings
    private Quaternion rot;
    private float rotationX = 0;
    private float rotationY = 0;



    [Header("Others :")]
    [SerializeField] private bool canMoveCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        initialDistance = distance;
        isCoroutine = false;
        canMoveCamera = true;

        playerInput = LoadData.instance.playerInput;
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensibilityData", 0.5f);
        gamepadSensitivity = PlayerPrefs.GetFloat("GamepadSensibilityData", 2.5f);
    }

    private void LateUpdate()
    {
        if (!canMoveCamera)
            return;

        HandleCameraRotation();
        HandleCameraPosition();

        if (ODMGasRef.IsUseODMGear())
        {
            if (!isCoroutine && distance != ODMGearDistance)
                StartCoroutine(SetDistanceCoroutine(true));
        }
        else
        {
            if (!isCoroutine && distance != initialDistance)
                StartCoroutine(SetDistanceCoroutine(false));
        }

        rot = transform.rotation;
    }

    private void HandleCameraRotation()
    {
        bool isKeyboard = LoadData.instance.playerInput.currentControlScheme == "Keyboard";

        float sensitivity = isKeyboard ? mouseSensitivity : gamepadSensitivity;

        float mouseX = LoadData.instance.playerInput.actions["Look"].ReadValue<Vector2>().x * sensitivity;
        float mouseY = LoadData.instance.playerInput.actions["Look"].ReadValue<Vector2>().y * sensitivity;

        rotationX -= mouseY;
        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, minXRot, maxXRot);

        Quaternion cameraRotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraRotation, Time.deltaTime * rotationSpeed);
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
