using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    [Header("UI asociada")]
    public GameObject uiPanel;

    [Header("Opcional")]
    public float interactionRange = 2f;

    [Header("Estado de interacción")]
    public bool canInteract = true;

    [Header("Prefabs que pueden aparecer en cooldown")]
    public GameObject[] spawnPrefabs; // Arrastra aquí tus prefabs en el Inspector

    [Header("Referencia al jugador")]
    public Transform player; // Arrastra aquí el Player en el Inspector

    [Header("Distancia de spawn respecto al jugador")]
    public float spawnDistance = 0.5f; 

    /// <summary>
    /// Inicia el cooldown del interactuable
    /// </summary>
    public void StartCooldown(float seconds)
    {
        StartCoroutine(CooldownCoroutine(seconds));
    }

    private IEnumerator CooldownCoroutine(float seconds)
    {
        canInteract = false;

        if (spawnPrefabs != null && spawnPrefabs.Length > 0 && player != null)
        {
            int index = Random.Range(0, spawnPrefabs.Length);
            GameObject prefab = spawnPrefabs[index];

            // Posición justo en frente del jugador
            Vector3 spawnPos = player.position + player.forward * spawnDistance;

            // Instanciamos el prefab
            GameObject instance = Instantiate(prefab, spawnPos, player.rotation);

            // Lo hacemos hijo del jugador para que se mueva con él
            instance.transform.SetParent(player);

            Debug.Log($"[Interactable] Spawned prefab {prefab.name} frente al jugador en {spawnPos}");
        }
        else
        {
            if (spawnPrefabs == null || spawnPrefabs.Length == 0)
                Debug.LogWarning("[Interactable] No hay prefabs asignados en spawnPrefabs");
            if (player == null)
                Debug.LogWarning("[Interactable] No se asignó referencia al Player");
        }

        yield return new WaitForSeconds(seconds);

        canInteract = true;
        Debug.Log($"[Interactable] Cooldown terminado. {gameObject.name} listo para interactuar otra vez.");
    }
}


