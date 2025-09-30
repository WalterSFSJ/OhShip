using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [Header("Prefabs a spawnear")]
    public GameObject[] prefabs;

    [Header("Límite de spawn")]
    public int maxObjects = 5;
    public int respawnThreshold = 2; 

    [Header("Rango de tiempo entre spawns")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Vector3 areaSize;
    private float nextSpawnTime;
    private bool shouldRespawn = true; 

    void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            areaSize = renderer.bounds.size;
        }

        ScheduleNextSpawn();
    }

    void Update()
    {
        spawnedObjects.RemoveAll(obj => obj == null);

        if (spawnedObjects.Count <= respawnThreshold)
        {
            shouldRespawn = true;
        }

        if (shouldRespawn && Time.time >= nextSpawnTime && spawnedObjects.Count < maxObjects)
        {
            SpawnPrefab();
            ScheduleNextSpawn();

            if (spawnedObjects.Count >= maxObjects)
            {
                shouldRespawn = false;
            }
        }
    }

    void ScheduleNextSpawn()
    {
        float delay = Random.Range(minSpawnTime, maxSpawnTime);
        nextSpawnTime = Time.time + delay;
    }

    void SpawnPrefab()
    {
        if (prefabs.Length == 0) return;

        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        Vector3 center = transform.position;
        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
        float z = Random.Range(-areaSize.z / 2f, areaSize.z / 2f);

        Vector3 spawnPos = center + new Vector3(x, y, z);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedObjects.Add(obj);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
