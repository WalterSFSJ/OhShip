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
    [Tooltip("Z para instanciar objetos (0 en 2D, 10 o más en 3D si lo necesitas)")]
    public float zPos = 0f; // antes estaba en 10f, que tapaba a los personajes

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

            bool fromLeft;
            if (sideObjects.Length == 2)
                fromLeft = (i == 0);
            else
                fromLeft = Random.value > 0.5f;

            float startX = fromLeft ? -width / 2f - offscreenDistance : width / 2f + offscreenDistance;
            float yPos = Random.Range(-height / 2f, height / 2f) + yOffset;

            // ? Corregido: zPos configurable (antes era 10f que bloqueaba la cámara)
            Vector3 spawnPos = new Vector3(cam.transform.position.x + startX, yPos, zPos);
            GameObject obj = Instantiate(sideObjects[i], spawnPos, Quaternion.identity);

            Vector3 target = new Vector3(
                cam.transform.position.x + Random.Range(-width / 4f, width / 4f),
                yOffset,
                zPos
            );

            // Si tiene SpriteRenderer, opcionalmente lo enviamos detrás de los personajes
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = -5;

            StartCoroutine(MoveObject(obj, target));
            Destroy(obj, appearDuration);
        }

        yield return null;
    }

    IEnumerator MoveObject(GameObject obj, Vector3 target)
    {
        float elapsed = 0f;
        while (elapsed < appearDuration)
        {
            if (obj == null) yield break;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}

