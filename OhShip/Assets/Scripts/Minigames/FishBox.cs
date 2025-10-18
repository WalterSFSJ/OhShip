using UnityEngine;

public class FishBox : MonoBehaviour
{
    public int acceptedFishID;
    public int scoreReward = 50;
    [SerializeField]
    private GameObject particles;
    private void OnTriggerEnter(Collider other)
    {
        Fish fish = other.GetComponent<Fish>();
        if (fish != null && fish.isCarried)
        {
            if (fish.fishID == acceptedFishID)
            {
                Debug.Log($"[FishBox] Pez {fish.fishID} entregado en {name}");

                Interaction interaction = fish.carrier; 
                if (interaction != null)
                {
                    interaction.ClearCarriedFish();

                    if (ScoreManager.Instance != null)
                        ScoreManager.Instance.AddScore(interaction.gameObject.name, scoreReward);
                }

                Destroy(fish.gameObject);
                Instantiate(particles, this.transform);
            }
            else
            {
                Debug.Log($"[FishBox] Pez incorrecto para {name}");
            }
        }
    }
}



