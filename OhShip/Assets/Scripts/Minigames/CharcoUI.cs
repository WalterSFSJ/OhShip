using UnityEngine;

public class CharcoUI : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Interaction playerInteraction;

    [Header("Número de pulsaciones requeridas")]
    public int requiredPresses = 10;

    private int currentPresses = 0;
    private bool isMinigameActive = false;
    private bool turned = false;
    private PlayerController pc;

    private void OnEnable()
    {
        currentPresses = 0;
    }

    private void Start()
    {
        if (playerInteraction != null)
            pc = playerInteraction.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!isMinigameActive || pc == null) return;

        if (pc.GetX() > 0 && !turned)
            HandleTurn(true);
        else if (pc.GetX() < 0 && turned)
            HandleTurn(false);
    }

    private void HandleTurn(bool _turned)
    {
        currentPresses++;
        turned = _turned;

        Debug.Log($"[CharcoUI] Pulsaciones: {currentPresses}/{requiredPresses}");

        if (currentPresses >= requiredPresses)
        {
            AwardPoints();
            CloseUI();
        }
    }

    private void AwardPoints()
    {
        if (playerInteraction != null && playerInteraction.name != null)
        {
            string playerName = playerInteraction.gameObject.name; 
            ScoreManager.Instance.AddScore(playerName, 100); 
            Debug.Log($"[CharcoUI] {playerName} ganó 10 puntos");
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




