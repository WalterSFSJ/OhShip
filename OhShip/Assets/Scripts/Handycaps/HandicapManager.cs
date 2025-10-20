using UnityEngine;
using System;
using System.Collections;

public class HandicapManager : MonoBehaviour
{
    [Header("Configuración general")]
    public Timer timer;
    public float checkInterval = 0.2f; 

    public static event Action OnStartHandicap;
    public static event Action OnMidHandicap;
    public static event Action OnEndHandicap;

    private bool midTriggered = false;
    private bool endTriggered = false;

    void Start()
    {
        if (timer == null)
        {
            timer = FindObjectOfType<Timer>();
        }

        if (timer == null)
        {
            return;
        }

        OnStartHandicap?.Invoke();

        StartCoroutine(CheckTimerProgress());
    }

    IEnumerator CheckTimerProgress()
    {
        float initial = timer.TimeInitial;
        if (initial <= 0f) initial = 1f; 

        while (timer != null && timer.TimeLeft > 0f)
        {
            float progress = timer.TimeLeft / initial; 

            if (!midTriggered && progress <= 0.5f)
            {
                midTriggered = true;
                OnMidHandicap?.Invoke();
            }

            if (!endTriggered && progress <= 0f)
            {
                endTriggered = true;
                OnEndHandicap?.Invoke();
                yield break;
            }

            yield return new WaitForSeconds(checkInterval);
        }

        if (!endTriggered)
        {
            endTriggered = true;
            OnEndHandicap?.Invoke();
        }
    }
}



