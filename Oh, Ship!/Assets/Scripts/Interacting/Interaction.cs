using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public PlayerController playerController;

    [Header("Rangos")]
    [SerializeField] private float pickupRadius = 1.5f;

    private Interactable currentTarget;
    public Interactable CurrentTarget => currentTarget;

    private Fish carriedFish; // pez que llevamos
    private bool eEnabled = true; // controla si E está activa

    public void SetCurrentTarget(Interactable target)
    {
        currentTarget = target;
    }

    private void Update()
    {
        // ---------- Si llevamos pez ----------
        if (carriedFish != null)
        {
            return;
        }

        // ---------- Si NO llevamos pez ----------
        currentTarget = FindClosestInteractable();

        if (Keyboard.current.eKey.wasPressedThisFrame && eEnabled)
        {
            // 1) Intentar recoger un pez cercano
            Collider[] nearby = Physics.OverlapSphere(player.position, pickupRadius);
            foreach (var hit in nearby)
            {
                Fish fish = hit.GetComponent<Fish>();
                if (fish != null && !fish.isCarried)
                {
                    PickupFish(fish);
                    return;
                }
            }

            // 2) Si no hay pez, abrir la UI del interactable más cercano
            if (currentTarget != null && currentTarget.canInteract && !currentTarget.uiPanel.activeSelf)
            {
                OpenUI(currentTarget);
            }
        }
    }

    private Interactable FindClosestInteractable()
    {
        Interactable[] interactables = FindObjectsOfType<Interactable>();
        Interactable closest = null;
        float minDist = float.MaxValue;

        foreach (var obj in interactables)
        {
            float dist = Vector3.Distance(player.position, obj.transform.position);
            if (dist < obj.interactionRange && dist < minDist && obj.canInteract)
            {
                minDist = dist;
                closest = obj;
            }
        }

        return closest;
    }

    private void OpenUI(Interactable target)
    {
        if (target.uiPanel != null)
        {
            // Cerrar otras UIs
            Interactable[] all = FindObjectsOfType<Interactable>();
            foreach (var obj in all)
            {
                if (obj.uiPanel != null && obj != target)
                {
                    obj.uiPanel.SetActive(false);

                    var otherChar = obj.uiPanel.GetComponent<CharcoUI>();
                    if (otherChar != null) otherChar.StopMinigame();

                    var otherRed = obj.uiPanel.GetComponent<RedUI>();
                    if (otherRed != null) otherRed.StopMinigame();
                }
            }

            // Abrir UI
            target.uiPanel.SetActive(true);

            var charUI = target.uiPanel.GetComponent<CharcoUI>();
            if (charUI != null)
            {
                charUI.playerInteraction = this;
                charUI.StartMinigame();
            }

            var redUI = target.uiPanel.GetComponent<RedUI>();
            if (redUI != null)
            {
                redUI.playerInteraction = this;
                redUI.StartMinigame();
            }

            if (playerController != null)
                playerController.enabled = false;
        }
    }

    public void CloseUI(Interactable target)
    {
        if (target.uiPanel != null)
        {
            var charUI = target.uiPanel.GetComponentInChildren<CharcoUI>();
            if (charUI != null)
            {
                charUI.StopMinigame();
                target.uiPanel.SetActive(false);
                if (playerController != null) playerController.enabled = true;
                return;
            }

            var redUI = target.uiPanel.GetComponentInChildren<RedUI>();
            if (redUI != null)
            {
                redUI.StopMinigame();
                target.uiPanel.SetActive(false);
                if (playerController != null) playerController.enabled = true;
                return;
            }

            target.uiPanel.SetActive(false);
        }

        if (playerController != null)
            playerController.enabled = true;
    }

    // ------------------- Gestión de pez -------------------

    private void PickupFish(Fish fish)
    {
        if (fish == null || fish.isCarried) return;

        fish.PickUp(player);
        carriedFish = fish;

        DisableE(); 

        Debug.Log($"[Interaction] Recogido pez {fish.name}. E deshabilitada.");
    }

    public void ClearCarriedFish() // llamado desde FishBox al entregar
    {
        carriedFish = null;
        EnableE();
    }

    // ------------------- Habilitar / deshabilitar E -------------------
    private void DisableE()
    {
        eEnabled = false;
    }

    private void EnableE()
    {
        eEnabled = true;
    }
}




