using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("ID del pez (ej: 0 = pez rojo, 1 = pez azul, 2 = pez verde)")]
    public int fishID;

    [HideInInspector] public bool isCarried = false;
    private Transform player;

    public void PickUp(Transform playerTransform)
    {
        player = playerTransform;
        isCarried = true;
        transform.SetParent(player);
        transform.localPosition = new Vector3(0, 0, 1f); // pegado delante del jugador
    }

    public void Drop()
    {
        isCarried = false;
        transform.SetParent(null);
        player = null;
    }
}
