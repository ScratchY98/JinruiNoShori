using UnityEngine;

public class SpawnTitan : MonoBehaviour
{
    [SerializeField] private GameObject TitanPrefab;
    [SerializeField] private float YSpawnDiff = 0;

    [SerializeField] private int TitanNumber = 286;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float spawnRange = 600.0f;

    [SerializeField] private GameObject TitanParent;

    private void Start()
    {
        for (int i = 0; i < TitanNumber; i++)
        {
            SpawnTitanAtRandomPosition();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnTitanAtRandomPosition();
        }
    }

    public void SpawnTitanAtRandomPosition()
    {
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPosition = SpawnPoint.position + new Vector3(randomX, YSpawnDiff, randomZ);

        GameObject instantiatedTitan = Instantiate(TitanPrefab, spawnPosition, SpawnPoint.rotation) as GameObject;

        instantiatedTitan.transform.parent = TitanParent.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(SpawnPoint.position, new Vector3(spawnRange * 2, 0.1f + YSpawnDiff, spawnRange * 2));
    }
}
