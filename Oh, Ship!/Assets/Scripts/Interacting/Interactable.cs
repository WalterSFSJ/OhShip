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

    [Header("Prefabs de peces que pueden aparecer en cooldown")]
    public GameObject[] spawnPrefabs; // cada prefab debe tener Fish.cs con su fishID

    [Header("Referencia al jugador")]
    public Transform player; // arrastra aquí el Player en el Inspector

    [Header("Distancia de spawn respecto al jugador")]
    public float spawnDistance = 0.5f; // bien pegado delante

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

            // posición en frente del jugador
            Vector3 spawnPos = player.position + player.forward * spawnDistance;

            // instanciamos el pez
            GameObject instance = Instantiate(prefab, spawnPos, player.rotation);

            // opcional: lo hacemos hijo del jugador para que aparezca pegado y siga su movimiento
            instance.transform.SetParent(player);

            // aseguramos que tiene Fish.cs
            if (instance.GetComponent<Fish>() == null)
                instance.AddComponent<Fish>();

            Debug.Log($"[Interactable] Pez {prefab.name} spawneado frente al jugador.");
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



