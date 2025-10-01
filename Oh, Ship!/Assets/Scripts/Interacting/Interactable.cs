using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    [Header("UI asociada (solo si aplica)")]
    public GameObject uiPanel;

    [Header("Opcional")]
    public float interactionRange = 2f;

    [Header("Estado de interacción")]
    public bool canInteract = true;

    [Header("Prefabs de peces que pueden aparecer en cooldown")]
    public GameObject[] spawnPrefabs; 

    [Header("Referencia al jugador")]
    public Transform player; 

    [Header("Distancia de spawn respecto al jugador")]
    public float spawnDistance = 0.5f; 

    [Header("Arrastrable (sin UI)")]
    public bool isDraggable = false;  
    public float dragSpeed = 2f;       
    public float dragDistance = 1f;    

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

            Vector3 spawnPos = player.position + player.forward * spawnDistance;

            GameObject instance = Instantiate(prefab, spawnPos, player.rotation);

            instance.transform.SetParent(player);

            if (instance.GetComponent<Fish>() == null)
            {
                instance.AddComponent<Fish>();
            }  
        }
        else
        {
            if (spawnPrefabs == null || spawnPrefabs.Length == 0)
            if (player == null)
                { }
        }

        yield return new WaitForSeconds(seconds);

        canInteract = true;
        Debug.Log($"[Interactable] Cooldown terminado. {gameObject.name} listo para interactuar otra vez.");
    }
}




