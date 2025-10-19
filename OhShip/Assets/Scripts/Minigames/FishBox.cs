using UnityEngine;

public class FishBox : MonoBehaviour
{
    public int acceptedFishID;
    public int scoreReward = 50;
    [SerializeField] private GameObject particles;
    [SerializeField] private AudioClip successSound; 
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Fish fish = other.GetComponent<Fish>();
        if (fish != null && fish.isCarried)
        {
            if (fish.fishID == acceptedFishID)
            {

                Interaction interaction = fish.carrier;
                if (interaction != null)
                {
                    interaction.ClearCarriedFish();

                    if (ScoreManager.Instance != null)
                        ScoreManager.Instance.AddScore(interaction.gameObject.name, scoreReward);
                }

                Destroy(fish.gameObject);
                Instantiate(particles, transform);

                if (successSound != null)
                    audioSource.PlayOneShot(successSound);
            }
        }
    }
}




