using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RedUI : MonoBehaviour
{
    [Header("Referencia al jugador activo")]
    public Interaction playerInteraction;

    [Header("Número de pulsaciones requeridas (conjuntos WASD)")]
    public int requiredPresses = 10;

    [Header("Puntuación otorgada al completar")]
    public int scoreReward = 50;

    [Header("Cooldown en segundos antes de volver a interactuar")]
    public float cooldownTime = 5f;

    [Header("Sonido al completar el minijuego")]
    public AudioClip completionSound;

    private int currentPresses = 0;
    private bool isMinigameActive = false;

    private bool wPressed, aPressed, sPressed, dPressed;
    private PlayerController pc;
    private Interactable currentInteractable;

    private void OnEnable()
    {
        ResetSet();
        currentPresses = 0;
    }

    public void Initialize(GameObject interactionGO)
    {
        playerInteraction = interactionGO.GetComponent<Interaction>();

        if (playerInteraction != null)
        {
            pc = playerInteraction.playerController;
            currentInteractable = playerInteraction.CurrentTarget;
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
                AwardPoints();
                CloseUI();
            }
        }
    }

    private void AwardPoints()
    {
        if (playerInteraction != null)
        {
            string playerName = playerInteraction.gameObject.name;
            ScoreManager.Instance.AddScore(playerName, scoreReward);
            Debug.Log($"[RedUI] {playerName} ganó {scoreReward} puntos");
        }
    }

    private void ResetSet()
    {
        wPressed = aPressed = sPressed = dPressed = false;
    }

    private void CloseUI()
    {
        // Reproducir sonido de completado en objeto temporal
        PlayCompletionSound();

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

                    if (interactable.defaultUIPanel != null)
                        interactable.StartCoroutine(ReenableDefaultUIAfterCooldown(interactable));
                }

                playerInteraction.SetCurrentTarget(null);
            }
        }

        isMinigameActive = false;
        playerInteraction = null;
        pc = null;

        // Desactivar inmediatamente el panel
        gameObject.SetActive(false);
    }

    private void PlayCompletionSound()
    {
        if (completionSound == null) return;

        // Crear un objeto temporal solo para reproducir el audio
        GameObject tempAudio = new GameObject("TempAudio");
        AudioSource aSource = tempAudio.AddComponent<AudioSource>();
        aSource.clip = completionSound;
        aSource.Play();

        // Destruir el objeto después de que termine el clip
        Destroy(tempAudio, completionSound.length);
    }

    private IEnumerator ReenableDefaultUIAfterCooldown(Interactable interactable)
    {
        yield return new WaitForSeconds(cooldownTime);

        if (interactable != null && interactable.defaultUIPanel != null)
        {
            interactable.defaultUIPanel.SetActive(true);
            Debug.Log($"[RedUI] Reactivada la UI predeterminada de {interactable.name} tras el cooldown.");
        }
    }
}












