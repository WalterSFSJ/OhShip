using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public PlayerController playerController; // script de movimiento del jugador

    private Interactable currentTarget;

    public Interactable CurrentTarget => currentTarget;

    public void SetCurrentTarget(Interactable target)
    {
        currentTarget = target;
    }

    private void Update()
    {
        currentTarget = FindClosestInteractable();

        // Solo abrimos la UI si hay un objeto cerca y la UI no está ya activa
        if (currentTarget != null
            && Keyboard.current.eKey.wasPressedThisFrame
            && !currentTarget.uiPanel.activeSelf)
        {
            OpenUI(currentTarget);
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
            if (dist < obj.interactionRange && dist < minDist)
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
            Debug.Log($"[Interaction] Abriendo UI de {target.name}");

            // Desactivamos otras UIs
            Interactable[] all = FindObjectsOfType<Interactable>();
            foreach (var obj in all)
            {
                if (obj.uiPanel != null && obj != target)
                {
                    obj.uiPanel.SetActive(false);
                    var otherChar = obj.uiPanel.GetComponent<CharcoUI>();
                    if (otherChar != null) otherChar.StopMinigame();
                }
            }

            // Activamos la UI seleccionada
            target.uiPanel.SetActive(true);

            // Arrancamos minijuego si existe
            var charUI = target.uiPanel.GetComponent<CharcoUI>();
            if (charUI != null)
            {
                Debug.Log("[Interaction] StartMinigame() llamado");
                charUI.playerInteraction = this;
                charUI.StartMinigame();
            }

            playerController.enabled = false;
        }
    }

    // Método público para cerrar la UI desde un botón
    public void CloseUI(Interactable target)
    {
        if (target.uiPanel != null)
        {
            var charUI = target.uiPanel.GetComponentInChildren<CharcoUI>();
            if (charUI != null)
            {
                charUI.StopMinigame();
                Destroy(target.uiPanel);   // destruir en vez de SetActive(false)
            }
            else
            {
                Destroy(target.uiPanel);
            }
        }

        // Reactivar movimiento
        if (playerController != null)
            playerController.enabled = true;
    }
}
