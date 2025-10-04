using UnityEngine;
using System;
using System.Collections;

public class HandicapManager : MonoBehaviour
{
    [Header("Configuración general")]
    public float totalDuration = 20f; // Duración total de la ronda o del evento

    public static event Action OnStartHandicap;
    public static event Action OnMidHandicap;
    public static event Action OnEndHandicap;

    void Start()
    {
        StartCoroutine(HandicapRoutine());
    }

    IEnumerator HandicapRoutine()
    {
        float halfTime = totalDuration / 2f;

        // Inicio
        OnStartHandicap?.Invoke();

        yield return new WaitForSeconds(halfTime);

        // Mitad del tiempo
        OnMidHandicap?.Invoke();

        yield return new WaitForSeconds(halfTime);

        // Fin del handicap
        OnEndHandicap?.Invoke();
    }
}

