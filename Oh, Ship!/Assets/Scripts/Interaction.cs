using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public PlayerController playerController; // script de movimiento del jugador

    private Interactable currentTarget;

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
            // Desactivamos todas las otras UIs
            Interactable[] all = FindObjectsOfType<Interactable>();
            foreach (var obj in all)
                if (obj.uiPanel != null && obj != target)
                    obj.uiPanel.SetActive(false);

            // Activamos la UI del objeto actual
            target.uiPanel.SetActive(true);

            // Bloqueamos movimiento del jugador
            if (playerController != null)
                playerController.enabled = false;

            // Si la UI es World Space, opcional: colocar sobre el jugador
            target.uiPanel.transform.position = player.position + Vector3.up * 2f;
        }
    }

    // Método público para cerrar la UI desde un botón
    public void CloseUI(Interactable target)
    {
        if (target.uiPanel != null)
        {
            target.uiPanel.SetActive(false);

            // Reactivar movimiento
            if (playerController != null)
                playerController.enabled = true;
        }
    }
}
