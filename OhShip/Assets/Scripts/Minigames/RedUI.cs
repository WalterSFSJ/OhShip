using UnityEngine;
using UnityEngine.InputSystem;

public class RedUI : MonoBehaviour
{
    [Header("Referencia al jugador activo")]
    public Interaction playerInteraction;

    [Header("Número de pulsaciones requeridas (conjuntos WASD)")]
    public int requiredPresses = 10;

    [Header("Cooldown en segundos antes de volver a interactuar")]
    public float cooldownTime = 5f;

    private int currentPresses = 0;
    private bool isMinigameActive = false;

    private bool wPressed, aPressed, sPressed, dPressed;
    private PlayerController pc;

    private void OnEnable()
    {
        ResetSet();
        currentPresses = 0;
    }

    public void Initialize(Interaction interaction)
    {
        playerInteraction = interaction;

        if (playerInteraction != null)
        {
            pc = playerInteraction.playerController;
            Debug.Log($"[RedUI] Vinculado a {pc.gameObject.name}");
        }

        currentPresses = 0;
        ResetSet();
        isMinigameActive = true;
        gameObject.SetActive(true);
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

    private void Update()
    {
        if (!isMinigameActive || pc == null) return;

        if (pc.GetY() > 0) wPressed = true;
        if (pc.GetX() < 0) aPressed = true;
        if (pc.GetY() < 0) sPressed = true;
        if (pc.GetX() > 0) dPressed = true;

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

    private void ResetSet()
    {
        wPressed = aPressed = sPressed = dPressed = false;
    }

    private void CloseUI()
    {
        if (playerInteraction != null)
        {
            if (playerInteraction.CurrentTarget != null && playerInteraction.PlayerTransform != null)
            {
                playerInteraction.CurrentTarget.SpawnFish(playerInteraction.PlayerTransform);
            }

            if (playerInteraction.playerController != null)
                playerInteraction.playerController.enabled = true;

            if (playerInteraction.CurrentTarget != null)
            {
                Interactable interactable = playerInteraction.CurrentTarget.GetComponent<Interactable>();
                if (interactable != null)
                {
                    if (interactable.uiPanel != null)
                        interactable.uiPanel.SetActive(false);

                    interactable.StartCooldown(cooldownTime);
                }

                playerInteraction.SetCurrentTarget(null);
            }
        }

        isMinigameActive = false;
        gameObject.SetActive(false);
    }
}








