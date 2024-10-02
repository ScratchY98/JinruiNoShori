using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float gamepadSensitivityX = 0.5f;
    [SerializeField] private float gamepadSensitivityY = 0.5f;
    [SerializeField] private float keyboardSensitivityX = 2.0f;
    [SerializeField] private float keyboardSensitivityY = 2.0f;
    [SerializeField] private float maxXRot = 90f;
    [SerializeField] private float minXRot = -75f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float ODMGearDistance = 8.0f;
    [SerializeField] private float setDistanceTimeDuration = 0.5f;
    [SerializeField] private ODMGearController ODMGearControllerRef;

    private bool isCoroutine;
    [SerializeField] private bool canMoveCamera;

    [SerializeField] private float WallDistance = 0.4f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, 0);
    [SerializeField] private float initialDistance;
    [SerializeField] private Quaternion rot;

    private float rotationX = 0;
    private float rotationY = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        initialDistance = distance;
        isCoroutine = false;
        canMoveCamera = true;
    }

    private void LateUpdate()
    {
        if (canMoveCamera)
        {
            HandleCameraRotation();
            HandleCameraPosition();

            if (ODMGearControllerRef.isUseODMGear)
            {
                if (!isCoroutine && distance != ODMGearDistance)
                    StartCoroutine(SetDistanceCoroutine(true));
            }
            else
            {
                if (!isCoroutine && distance != initialDistance)
                    StartCoroutine(SetDistanceCoroutine(false));
            }
        }

        rot = transform.rotation;
    }

    private void HandleCameraRotation()
    {
        bool isKeyboard = LoadData.instance.playerInput.currentControlScheme == "Keyboard";

        float sensitivityX = isKeyboard ? keyboardSensitivityX : gamepadSensitivityX;
        float sensitivityY = isKeyboard ? keyboardSensitivityY : gamepadSensitivityY;

        float mouseX = LoadData.instance.playerInput.actions["Look"].ReadValue<Vector2>().x * sensitivityX;
        float mouseY = LoadData.instance.playerInput.actions["Look"].ReadValue<Vector2>().y * sensitivityY;

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
        {
            offset = hit.point + transform.forward * WallDistance;
        }

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
