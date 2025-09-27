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

    // Llamar desde Interaction.OpenUI()
    public void StartMinigame()
    {
        Debug.Log("[CharcouI] Minijuego iniciado");
        currentPresses = 0;
        isMinigameActive = true;
    }

    // Llamar desde Interaction cuando se cierra otra UI
    public void StopMinigame()
    {
        isMinigameActive = false;
    }

    private void CloseUI()
    {
        if (playerInteraction != null && playerInteraction.playerController != null)
            playerInteraction.playerController.enabled = true;

        if (playerInteraction != null && playerInteraction.CurrentTarget != null)
        {
            Destroy(playerInteraction.CurrentTarget.gameObject);
            playerInteraction.SetCurrentTarget(null);
        }

        Destroy(gameObject);
    }
}
