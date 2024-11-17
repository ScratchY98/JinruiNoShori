using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float XLimit;
    private float speed;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void Update()
    {
        if (transform.position.x < XLimit)
            Destroy(gameObject);
    }
}
