using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;

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

    // -------- Arrastrar objetos --------
    private Interactable draggedObject;

    [Header("Arrastre")]
    [SerializeField] private float dragSpeedMultiplier = 0.1f; // % de velocidad al arrastrar
    private float originalSpeed;

    public void SetCurrentTarget(Interactable target)
    {
        currentTarget = target;
    }

    private void Update()
    {
        // ---------- Si llevamos pez ----------
        if (carriedFish != null)
            return;

        // ---------- Si estamos arrastrando un objeto ----------
        if (draggedObject != null)
        {
            if (Keyboard.current.eKey.isPressed)
            {
                DragObject(draggedObject);
            }
            else if (Keyboard.current.eKey.wasReleasedThisFrame)
            {
                ReleaseDraggedObject();
            }
            return;
        }

        // ---------- Si no llevamos pez ni arrastramos ----------
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

            // 2) Si no hay pez, arrastrar objeto
            if (currentTarget != null && currentTarget.isDraggable)
            {
                StartDragging(currentTarget);
                return;
            }

            // 3) Si no es arrastrable, abrir la UI
            if (currentTarget != null && currentTarget.canInteract && !currentTarget.uiPanel.activeSelf)
            {
                OpenUI(currentTarget);
            }
        }
    }

    // ----------- Arrastrar objetos -----------
    private void StartDragging(Interactable target)
    {
        draggedObject = target;

        Rigidbody rb = draggedObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true; // desactivar física mientras lo llevamos

        // Reducir velocidad del jugador usando reflexión
        if (playerController != null)
        {
            FieldInfo speedField = typeof(PlayerController).GetField("speed", BindingFlags.NonPublic | BindingFlags.Instance);
            if (speedField != null)
            {
                originalSpeed = (float)speedField.GetValue(playerController);
                speedField.SetValue(playerController, originalSpeed * dragSpeedMultiplier);
            }
        }
    }

    private void DragObject(Interactable target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 targetPos = player.position + player.forward * target.dragDistance;
        rb.MovePosition(Vector3.Lerp(rb.position, targetPos, target.dragSpeed * Time.deltaTime));
    }

    private void ReleaseDraggedObject()
    {
        if (draggedObject == null) return;

        Rigidbody rb = draggedObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false; // reactivar física

        // Restaurar velocidad del jugador usando reflexión
        if (playerController != null)
        {
            FieldInfo speedField = typeof(PlayerController).GetField("speed", BindingFlags.NonPublic | BindingFlags.Instance);
            if (speedField != null)
            {
                speedField.SetValue(playerController, originalSpeed);
            }
        }

        draggedObject = null;
    }

    // ----------- Buscar interactuable más cercano -----------
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

    // ----------- Manejo de UIs -----------
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

    // ----------- Gestión de pez -----------
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

    // ----------- Habilitar / deshabilitar E -----------
    private void DisableE()
    {
        eEnabled = false;
    }

    private void EnableE()
    {
        eEnabled = true;
    }
}

