using UnityEngine;
using UnityEngine.InputSystem;

public class RedUI : MonoBehaviour
{
    [Header("Referencia al jugador")]
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

    private void Start()
    {
        pc = playerInteraction.gameObject.GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (!isMinigameActive) return;

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
            Interactable interactable = playerInteraction.CurrentTarget.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (interactable.uiPanel != null)
                    interactable.uiPanel.SetActive(false);

                interactable.StartCooldown(cooldownTime);
            }

            playerInteraction.SetCurrentTarget(null);
        }

        gameObject.SetActive(false);
    }
}




