using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeInitial = 300f;
    private float timeLeft;
    public Image circle;
    public TMP_Text timerText;

    private bool on = true;

    private void Start()
    {
        timeLeft = timeInitial;
    }

    private void Update()
    {
        if (!on) return;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            ShowTime(timeLeft);

            // Actualiza el círculo de tiempo
            circle.fillAmount = timeLeft / timeInitial;

            // Cambia el color según el tiempo restante
            if (timeLeft < timeInitial / 3)
                circle.color = Color.red;
            else if (timeLeft < (timeInitial / 3) * 2)
                circle.color = Color.yellow;
        }
        else
        {
            timeLeft = 0;
            on = false;
            EndGame();
        }
    }

    private void EndGame()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("No se encontró el ScoreManager. Cargando GameOver...");
            SceneManager.LoadScene("GameOver");
            return;
        }

        int p1 = ScoreManager.Instance.player1Score;
        int p2 = ScoreManager.Instance.player2Score;

        Debug.Log($"[Timer] Fin del tiempo — P1: {p1}, P2: {p2}");

        if (p1 > p2)
        {
            SceneManager.LoadScene("Player1Wins");
        }
        else if (p2 > p1)
        {
            SceneManager.LoadScene("Player2Wins");
        }
        else
        {
            SceneManager.LoadScene("Draw");
        }
    }

    private void ShowTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
