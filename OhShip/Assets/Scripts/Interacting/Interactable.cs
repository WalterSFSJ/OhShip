using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    [Header("UI asociada al minijuego (si aplica)")]
    public GameObject uiPanel;

    [Header("UI predeterminada (por ejemplo, icono o panel visual)")]
    public GameObject defaultUIPanel;

    [Header("Opcional")]
    public float interactionRange = 2f;

    [Header("Estado de interacción")]
    public bool canInteract = true;

    [Header("Prefabs de peces que pueden aparecer en cooldown")]
    public GameObject[] spawnPrefabs;

    [Header("Distancia de spawn respecto al jugador")]
    public float spawnDistance = 1f;

    [Header("Arrastrable (sin UI)")]
    public bool isDraggable = false;
    public float dragSpeed = 2f;
    public float dragDistance = 1f;

    public void StartCooldown(float seconds)
    {
        StartCoroutine(CooldownCoroutine(seconds));
    }

    private IEnumerator CooldownCoroutine(float seconds)
    {
        canInteract = false;
        yield return new WaitForSeconds(seconds);
        canInteract = true;

        Debug.Log($"[Interactable] Cooldown terminado. {gameObject.name} listo para interactuar otra vez.");
    }

    /// <summary>
    /// Spawnea un pez frente al jugador y hace que lo siga automáticamente.
    /// </summary>
    public void SpawnFish(Transform player)
    {
        if (spawnPrefabs == null || spawnPrefabs.Length == 0 || player == null)
        {
            Debug.LogWarning("[Interactable] No se pudo spawnear pez: faltan referencias.");
            return;
        }

        int index = Random.Range(0, spawnPrefabs.Length);
        GameObject prefab = spawnPrefabs[index];

        GameObject instance = Instantiate(prefab, player.position, Quaternion.identity);

        instance.transform.SetParent(player);
        instance.transform.localPosition = new Vector3(0, 0, spawnDistance);
        instance.transform.localRotation = Quaternion.identity;

        if (instance.GetComponent<Fish>() == null)
            instance.AddComponent<Fish>();

        Debug.Log($"[Interactable] Pez {prefab.name} spawneado frente a {player.name}");
    }
}











