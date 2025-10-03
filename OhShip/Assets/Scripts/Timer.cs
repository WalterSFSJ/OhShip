using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeInitial = 300f;
    private float timeLeft;

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
            }
            else
            {
                timeLeft = 0;
                on = false;
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
