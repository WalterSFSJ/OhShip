using UnityEngine;

public class FishBox : MonoBehaviour
{
    [Header("Caja que acepta este tipo de pez")]
    public int acceptedFishID;

    private void OnTriggerEnter(Collider other)
    {
        Fish fish = other.GetComponent<Fish>();
        if (fish != null && fish.isCarried)
        {
            if (fish.fishID == acceptedFishID)
            {
                Debug.Log($"[FishBox] Pez {fish.fishID} entregado en {name}");
                Interaction interaction = FindObjectOfType<Interaction>();
                if (interaction != null)
                {
                    interaction.ClearCarriedFish();
                }
                Destroy(fish.gameObject);
            }
            else
            {
                Debug.Log($"[FishBox] Pez incorrecto para {name}");
            }
        }
    }
}

