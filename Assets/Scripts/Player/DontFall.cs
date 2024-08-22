using UnityEngine;

public class DontFall : MonoBehaviour
{
    [SerializeField] private float limit = 0;
    [SerializeField] private float replacePoint = 0.1f;
    void Update()
    {
        if (transform.position.y < limit)
            transform.position = new Vector3(transform.position.x, replacePoint, transform.position.z);
    }
}