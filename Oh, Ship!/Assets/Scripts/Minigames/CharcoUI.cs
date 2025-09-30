using UnityEngine;
using UnityEngine.InputSystem;

public class CharcoUI : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Interaction playerInteraction;

    [Header("Número de pulsaciones requeridas")]
    public int requiredPresses = 10;

    private int currentPresses = 0;
    private bool isMinigameActive = false;

    private void OnEnable()
    {
        currentPresses = 0;
    }

    private void Update()
    {
        if (!isMinigameActive) return;

        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
        {
            currentPresses++;
            Debug.Log($"[CharcoUI] Pulsaciones: {currentPresses}/{requiredPresses}");

            if (currentPresses >= requiredPresses)
            {
                CloseUI();
            }
        }
    }

    public void StartMinigame()
    {
        currentPresses = 0;
        isMinigameActive = true;
    }

    public void StopMinigame()
    {
        isMinigameActive = false;
    }

    private void CloseUI()
    {
        // Reactivar control del jugador
        if (playerInteraction != null && playerInteraction.playerController != null)
            playerInteraction.playerController.enabled = true;

        if (playerInteraction != null && playerInteraction.CurrentTarget != null)
        {
            // Destruye el objeto interactuable (el charco)
            Destroy(playerInteraction.CurrentTarget.gameObject);
            playerInteraction.SetCurrentTarget(null);
        }

        // Destruye el propio UI
        Destroy(gameObject);
    }
}

