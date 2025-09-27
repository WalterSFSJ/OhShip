using UnityEngine;
using UnityEngine.InputSystem;

public class CharcoUI : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Interaction playerInteraction; // asignar el script del jugador en el inspector

    [Header("Número de pulsaciones requeridas")]
    public int requiredPresses = 10;

    private int currentPresses = 0;

    private void OnEnable()
    {
        // Reiniciamos el contador cada vez que se abre la UI
        currentPresses = 0;
    }

    private void Update()
    {
        // Escuchamos solo A o D
        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
        {
            currentPresses++;

            // Opcional: feedback visual o sonoro
            Debug.Log($"Pulsaciones: {currentPresses}/{requiredPresses}");

            // Cerramos la UI si se alcanza el número requerido
            if (currentPresses >= requiredPresses)
            {
                CloseUI();
            }
        }
    }

    private void CloseUI()
    {
        // Desactivar la UI
        gameObject.SetActive(false);

        // Reactivar el movimiento del jugador
        if (playerInteraction != null && playerInteraction.playerController != null)
        {
            playerInteraction.playerController.enabled = true;
        }
    }
}
