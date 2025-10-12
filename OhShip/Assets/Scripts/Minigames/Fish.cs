using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("ID del pez (ej: 0 = pez rojo, 1 = pez azul, 2 = pez verde)")]
    public int fishID;

    [HideInInspector] public bool isCarried = false;

    [HideInInspector] public Interaction carrier;

    private Transform player;

    public void PickUp(Transform playerTransform)
    {
        player = playerTransform;
        isCarried = true;

        carrier = player.GetComponent<Interaction>();

        transform.SetParent(player);
        transform.localPosition = new Vector3(0, 0, 1f);
    }

    public void Drop()
    {
        isCarried = false;
        transform.SetParent(null);
        player = null;
        carrier = null;
    }
}


