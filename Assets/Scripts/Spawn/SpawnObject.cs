using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject[] spawnParent;
    [SerializeField] private SpawnableOject[] spawnableOjects;
    [SerializeField] private int gizmosObject;

    public static SpawnObject instance;

    private void Update()
    {
        instance = this;
    }

    private void Start()
    {
        for (int y = 0; y < spawnableOjects.Length; y++)
        {
            for (int i = 0; i < spawnableOjects[y].objectNumber; i++)
            {
                SpawnObjectsAtRandomPosition(y);
            }
        }
    }

    public void SpawnObjectsAtRandomPosition(int spawnableObject)
    {
        float randomX = Random.Range(-spawnableOjects[spawnableObject].spawnRange, spawnableOjects[spawnableObject].spawnRange);
        float randomZ = Random.Range(-spawnableOjects[spawnableObject].spawnRange, spawnableOjects[spawnableObject].spawnRange);

        Vector3 spawnPosition = spawnPoint[spawnableObject].position + new Vector3(randomX, spawnableOjects[spawnableObject].YSpawnDiff, randomZ);

        GameObject instantiatedPowerUps = Instantiate(spawnableOjects[spawnableObject].prefabs, spawnPosition, spawnPoint[spawnableObject].rotation) as GameObject;

        instantiatedPowerUps.transform.parent = spawnParent[spawnableObject].transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnPoint[gizmosObject].position, new Vector3(spawnableOjects[gizmosObject].spawnRange * 2, 0.1f + spawnableOjects[gizmosObject].YSpawnDiff, spawnableOjects[gizmosObject].spawnRange * 2));
    }
}