using UnityEngine;

public class SpawnPowerUp : MonoBehaviour
{
    [SerializeField] private GameObject powerUpsPrefabs;
    [SerializeField] private float YSpawnDiff = 0;

    [SerializeField] private int powerUpsNumber = 175;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float spawnRange = 600.0f;

    [SerializeField] private GameObject powerUpsParent;

    private void Start()
    {
        for (int i = 0; i < powerUpsNumber; i++)
        {
            SpawnPowerUpAtRandomPosition();
        }
    }

    public void SpawnPowerUpAtRandomPosition()
    {
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPosition = SpawnPoint.position + new Vector3(randomX, YSpawnDiff, randomZ);

        GameObject instantiatedPowerUps = Instantiate(powerUpsPrefabs, spawnPosition, SpawnPoint.rotation) as GameObject;

        instantiatedPowerUps.transform.parent = powerUpsParent.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(SpawnPoint.position, new Vector3(spawnRange * 2, 0.1f + YSpawnDiff, spawnRange * 2));
    }
}