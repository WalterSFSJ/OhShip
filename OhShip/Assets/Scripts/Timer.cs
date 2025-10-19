using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Configuración")]
    public float timeInitial = 300f; 
    private float timeLeft;
    public Image circle;
    public TMP_Text timerText;

    private bool on = true;

    public float TimeInitial => timeInitial;
    public float TimeLeft => timeLeft;
    public bool IsRunning => on;

    private void Start()
    {
        timeLeft = timeInitial;
    }

    private void Update()
    {
        if (!on) return;

        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f) timeLeft = 0f;

            ShowTime(timeLeft);

            if (circle != null)
                circle.fillAmount = timeLeft / (timeInitial > 0f ? timeInitial : 1f);

            if (circle != null)
            {
                if (timeLeft < timeInitial / 3f)
                    circle.color = Color.red;
                else if (timeLeft < (timeInitial / 3f) * 2f)
                    circle.color = Color.yellow;
            }
        }
        else
        {
            timeLeft = 0f;
            on = false;
            EndGame();
        }
    }

    private void EndGame()
    {
        if (ScoreManager.Instance == null)
        {
            SceneManager.LoadScene("GameOver");
            return;
        }

        int p1 = ScoreManager.Instance.player1Score;
        int p2 = ScoreManager.Instance.player2Score;

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
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Pause() => on = false;
    public void Resume() => on = true;
    public void ResetTimer() => timeLeft = timeInitial;
}

