using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform player;
    public Transform PlayerTransform => player; 

    [Header("Rangos")]
    [SerializeField] private float pickupRadius = 1.5f;

    public Interactable currentTarget;
    public Interactable CurrentTarget => currentTarget;

    private Fish carriedFish;
    private bool eEnabled = true;

    private Interactable draggedObject;

    [Header("Arrastre")]
    [SerializeField] private float dragSpeedMultiplier = 0.3f;
    private float originalSpeed;

    [Header("Referencia al PlayerController")]
    public PlayerController playerController;

    private void Start()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();
    }

    public void SetCurrentTarget(Interactable target)
    {
        currentTarget = target;
    }

    private void Update()
    {
        if (carriedFish != null) return;

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

        currentTarget = FindClosestInteractable();

        if (playerController != null && playerController.GetInteracted() && eEnabled)
        {
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

            if (currentTarget != null && currentTarget.isDraggable)
            {
                StartDragging(currentTarget);
                return;
            }

            if (currentTarget != null && currentTarget.canInteract && !currentTarget.uiPanel.activeSelf)
            {
                OpenUI(currentTarget);
            }
        }
    }

    private void StartDragging(Interactable target)
    {
        draggedObject = target;

        Rigidbody rb = draggedObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

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
            rb.isKinematic = false;

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
            Interactable[] all = FindObjectsOfType<Interactable>();
            foreach (var obj in all)
            {
                if (obj == target || obj.uiPanel == null) continue;

                var otherChar = obj.uiPanel.GetComponent<CharcoUI>();
                var otherRed = obj.uiPanel.GetComponent<RedUI>();

                bool samePlayer =
                    (otherChar != null && otherChar.playerInteraction == this) ||
                    (otherRed != null && otherRed.playerInteraction == this);

                if (samePlayer)
                {
                    obj.uiPanel.SetActive(false);
                    if (otherChar != null) otherChar.StopMinigame();
                    if (otherRed != null) otherRed.StopMinigame();
                }
            }

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
                redUI.Initialize(this);
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
        {
            playerController.enabled = true;
        }
    }

    private void PickupFish(Fish fish)
    {
        if (fish == null || fish.isCarried) return;

        fish.PickUp(player);
        carriedFish = fish;

        DisableE();
    }

    public void ClearCarriedFish()
    {
        carriedFish = null;
        EnableE();
    }

    private void DisableE() => eEnabled = false;
    private void EnableE() => eEnabled = true;
}



