using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableOject", menuName = "SpawnableOject", order = 1)]
public class SpawnableOject : ScriptableObject
{
    public GameObject prefabs;
    public float YSpawnDiff = 0;

    public int objectNumber = 175;
    public float spawnRange = 600.0f;
}