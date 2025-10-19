using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public GameObject uiPanel;
    public GameObject defaultUIPanel;
    public float interactionRange = 2f;
    public bool canInteract = true;
    public GameObject[] spawnPrefabs;
    public float spawnDistance = 1f;
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
    }

    public void SpawnFish(Transform player)
    {
        if (spawnPrefabs == null || spawnPrefabs.Length == 0 || player == null)
        {
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
    }
}











