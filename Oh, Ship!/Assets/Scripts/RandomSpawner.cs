using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [Header("Prefabs a spawnear")]
    public GameObject[] prefabs;

    [Header("Límite de spawn")]
    public int maxObjects = 5;
    public int respawnThreshold = 2; // cuando queden 2 o menos, empezar a reponer

    [Header("Rango de tiempo entre spawns")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Vector3 areaSize;
    private float nextSpawnTime;
    private bool shouldRespawn = true; // al inicio ya debe spawnear poco a poco

    void Start()
    {
        // Tomamos el tamaño del MeshRenderer del cubo
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            areaSize = renderer.bounds.size;
        }
        else
        {
            Debug.LogError("Este objeto no tiene MeshRenderer. Usa un cubo con MeshRenderer.");
        }

        ScheduleNextSpawn();
    }

    void Update()
    {
        // Limpia la lista de objetos destruidos
        spawnedObjects.RemoveAll(obj => obj == null);

        // Si hay <= respawnThreshold, habilitamos respawn
        if (spawnedObjects.Count <= respawnThreshold)
        {
            shouldRespawn = true;
        }

        // Si toca spawnear y necesitamos más
        if (shouldRespawn && Time.time >= nextSpawnTime && spawnedObjects.Count < maxObjects)
        {
            SpawnPrefab();
            ScheduleNextSpawn();

            // Si ya llegamos al máximo, detener respawn
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

        // Elegir prefab random
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        // Calcular posición random dentro del cubo
        Vector3 center = transform.position;
        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
        float z = Random.Range(-areaSize.z / 2f, areaSize.z / 2f);

        Vector3 spawnPos = center + new Vector3(x, y, z);

        // Spawnear
        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedObjects.Add(obj);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
