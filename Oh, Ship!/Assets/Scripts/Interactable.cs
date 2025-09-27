using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("UI asociada")]
    public GameObject uiPanel; // La UI que se abrirá al interactuar

    [Header("Opcional")]
    public float interactionRange = 2f; // Rango de interacción
}
