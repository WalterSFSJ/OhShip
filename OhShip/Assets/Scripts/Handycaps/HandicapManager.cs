using UnityEngine;
using System;
using System.Collections;

public class HandicapManager : MonoBehaviour
{
    [Header("Configuración general")]
    public Timer timer; // referencia al Timer (puedes asignarla desde el inspector)
    public float checkInterval = 0.2f; // frecuencia de comprobación en segundos

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
            Debug.LogError("[HandicapManager] No se encontró el Timer en la escena. Asigna la referencia en el inspector o añade un Timer a la escena.");
            return;
        }

        // Opcional: solo lanzar OnStartHandicap si el temporizador está en marcha
        OnStartHandicap?.Invoke();

        StartCoroutine(CheckTimerProgress());
    }

    IEnumerator CheckTimerProgress()
    {
        // Guardas el valor inicial una vez (en caso de que quieras que "mitad" se base en la inicial)
        float initial = timer.TimeInitial;
        if (initial <= 0f) initial = 1f; // evita división por cero

        while (timer != null && timer.TimeLeft > 0f)
        {
            float progress = timer.TimeLeft / initial; // entre 0 y 1 (0 = fin, 1 = inicio)

            // Mitad del tiempo (cuando TimeLeft <= initial/2)
            if (!midTriggered && progress <= 0.5f)
            {
                midTriggered = true;
                OnMidHandicap?.Invoke();
            }

            // Fin del tiempo
            if (!endTriggered && progress <= 0f)
            {
                endTriggered = true;
                OnEndHandicap?.Invoke();
                yield break;
            }

            yield return new WaitForSeconds(checkInterval);
        }

        // Si salimos del loop porque TimeLeft <= 0 pero no se disparó EndHandicap, lo forzamos.
        if (!endTriggered)
        {
            endTriggered = true;
            OnEndHandicap?.Invoke();
        }
    }
}



