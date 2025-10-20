using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Puntajes de los jugadores")]
    public int player1Score = 0;
    public int player2Score = 0;

    [Header("Textos UI")]
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddScore(string playerName, int amount)
    {
        if (playerName == "WASD")
            player1Score += amount;
        else if (playerName == "arrows")
            player2Score += amount;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (player1Text != null)
            player1Text.text = $"{player1Score}";

        if (player2Text != null)
            player2Text.text = $"{player2Score}";
    }
}



