using UnityEngine;
using System.Collections;

public class Handicap_SideObjects : MonoBehaviour
{
    [Header("Objetos")]
    public GameObject[] sideObjects;
    public float appearDuration = 5f;
    public float moveSpeed = 5f;
    public float offscreenDistance = 10f;

    [Header("Posición Y")]
    public float yOffset = -2f;

    [Header("Posición Z")]
    public float zPos = 0f;

    void OnEnable()
    {
        HandicapManager.OnMidHandicap += ActivateHandicap;
    }

    void OnDisable()
    {
        HandicapManager.OnMidHandicap -= ActivateHandicap;
    }

    void ActivateHandicap()
    {
        StartCoroutine(SpawnAndMoveObjects());
    }

    IEnumerator SpawnAndMoveObjects()
    {
        Camera cam = Camera.main;
        if (cam == null) yield break;

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        for (int i = 0; i < sideObjects.Length; i++)
        {
            if (sideObjects[i] == null) continue;

            bool fromLeft = (sideObjects.Length == 2) ? (i == 0) : (Random.value > 0.5f);

            float startX = fromLeft ? -width / 2f - offscreenDistance : width / 2f + offscreenDistance;
            float yPos = Random.Range(-height / 2f, height / 2f) + yOffset;

            Vector3 spawnPos = new Vector3(cam.transform.position.x + startX, yPos, zPos);
            GameObject obj = Instantiate(sideObjects[i], spawnPos, Quaternion.identity);

            Vector3 target = new Vector3(
                cam.transform.position.x + Random.Range(-width / 4f, width / 4f),
                yOffset,
                zPos
            );

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = -5;

            // Calculamos el punto de salida (igual al lado de inicio)
            float exitX = fromLeft ? -width / 2f - offscreenDistance : width / 2f + offscreenDistance;
            Vector3 exitPos = new Vector3(cam.transform.position.x + exitX, yPos, zPos);

            StartCoroutine(MoveInAndOut(obj, target, exitPos));
        }

        yield return null;
    }

    IEnumerator MoveInAndOut(GameObject obj, Vector3 target, Vector3 exitPos)
    {
        // Entrar
        float elapsed = 0f;
        while (obj != null && elapsed < appearDuration / 2f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Salir
        elapsed = 0f;
        while (obj != null && elapsed < appearDuration / 2f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, exitPos, moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (obj != null)
            Destroy(obj);
    }
}


