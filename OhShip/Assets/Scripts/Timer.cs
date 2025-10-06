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
        if (on)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                ShowTime(timeLeft);

                circle.fillAmount = timeLeft / timeInitial;

                if (timeLeft < timeInitial / 3) 
                {
                    circle.color = Color.red;
                }
                else if (timeLeft < (timeInitial / 3) * 2) 
                {
                    circle.color = Color.yellow;
                }
            }
            else
            {
                timeLeft = 0;
                on = false;
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    void ShowTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
