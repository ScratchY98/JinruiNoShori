using UnityEngine;

public class InspectAvatar : MonoBehaviour
{
    [SerializeField] private Transform avatarTransform;
    [SerializeField] private float rotationSpeed =  100f;
    [SerializeField] private Vector3 previousMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            previousMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
            float rotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            float rotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            avatarTransform.rotation = rotation * avatarTransform.rotation;

            previousMousePosition = Input.mousePosition;

        }
    }
}
