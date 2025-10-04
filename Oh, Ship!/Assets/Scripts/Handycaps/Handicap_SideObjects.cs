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
    public float yOffset = -2f; // Desplazamiento vertical hacia abajo

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

            // Si hay exactamente 2 objetos: uno izquierda y otro derecha
            bool fromLeft;
            if (sideObjects.Length == 2)
                fromLeft = (i == 0); // el primero por la izquierda, el segundo por la derecha
            else
                fromLeft = Random.value > 0.5f;

            float startX = fromLeft ? -width / 2f - offscreenDistance : width / 2f + offscreenDistance;

            // Ajuste en el eje Y
            float yPos = Random.Range(-height / 2f, height / 2f) + yOffset;

            Vector3 spawnPos = cam.transform.position + new Vector3(startX, yPos, 10f);
            GameObject obj = Instantiate(sideObjects[i], spawnPos, Quaternion.identity);

            // Apunta hacia una zona aleatoria del centro
            Vector3 target = cam.transform.position + new Vector3(Random.Range(-width / 4f, width / 4f), yOffset, 10f);

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
