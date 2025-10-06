using UnityEngine;
using System;
using System.Collections;

public class HandicapManager : MonoBehaviour
{
    [Header("Configuración general")]
    public float totalDuration = 20f; 

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

        OnStartHandicap?.Invoke();

        yield return new WaitForSeconds(halfTime);

        OnMidHandicap?.Invoke();

        yield return new WaitForSeconds(halfTime);

        OnEndHandicap?.Invoke();
    }
}

