 using UnityEngine;

public class SpawnCloud : MonoBehaviour
{
    [SerializeField] private GameObject cloudpPrefab;

    [SerializeField] private int numberByWave;
    [SerializeField] private float waveDelay;
    [SerializeField] private float spawnRange;


    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject spawnParent;

    void Start()
    {
        this.gameObject.SetActive(PlayerPrefs.GetInt("isCloud", 1) == 1 ? true : false);

        InvokeRepeating("SpawnCloudAtRandomPosition", 0f, waveDelay);
    }


    private void SpawnCloudAtRandomPosition()
    {
        for (int i = 0; i < numberByWave; i++)
        {
            float randomZ = Random.Range(-spawnRange, spawnRange);

            Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, randomZ);

            GameObject instantiatedCloud = Instantiate(cloudpPrefab, spawnPosition, spawnPoint.rotation) as GameObject;

            instantiatedCloud.transform.parent = spawnParent.transform;
            Destroy(instantiatedCloud, 100f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnPoint.position, new Vector3(0.1f, 0.1f, spawnRange * 2));
    }
}
