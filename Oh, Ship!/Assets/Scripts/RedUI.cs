using UnityEngine;
using UnityEngine.InputSystem;

public class RedUI : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Interaction playerInteraction;

    [Header("Número de pulsaciones requeridas (conjuntos WASD)")]
    public int requiredPresses = 10;

    private int currentPresses = 0;
    private bool isMinigameActive = false;

    // Flags para llevar el seguimiento de cada tecla en el conjunto actual
    private bool wPressed, aPressed, sPressed, dPressed;

    private void OnEnable()
    {
        ResetSet();
        currentPresses = 0;
    }

    private void Update()
    {
        if (!isMinigameActive) return;

        // Marcar cada tecla cuando se pulsa (solo en el frame de pulsación)
        if (Keyboard.current.wKey.wasPressedThisFrame) wPressed = true;
        if (Keyboard.current.aKey.wasPressedThisFrame) aPressed = true;
        if (Keyboard.current.sKey.wasPressedThisFrame) sPressed = true;
        if (Keyboard.current.dKey.wasPressedThisFrame) dPressed = true;

        // Si se han pulsado las 4 teclas al menos una vez -> cuenta como 1 y resetea el conjunto
        if (wPressed && aPressed && sPressed && dPressed)
        {
            currentPresses++;
            Debug.Log($"[RedUI] Conjunto WASD completado: {currentPresses}/{requiredPresses}");
            ResetSet();

            if (currentPresses >= requiredPresses)
            {
                CloseUI();
            }
        }
    }

    public void StartMinigame()
    {
        currentPresses = 0;
        ResetSet();
        isMinigameActive = true;
    }

    public void StopMinigame()
    {
        isMinigameActive = false;
    }

    private void ResetSet()
    {
        wPressed = aPressed = sPressed = dPressed = false;
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
