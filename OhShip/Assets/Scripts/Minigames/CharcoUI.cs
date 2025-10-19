using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

public class CharcoUI : MonoBehaviour
{
    [SerializeField] private GameObject particles;

    [Header("Referencia al jugador")]
    public Interaction playerInteraction;

    [Header("Número de pulsaciones requeridas")]
    public int requiredPresses = 10;

    [Header("Efecto de sonido")]
    [Tooltip("Sonido que se reproducirá cuando salgan las partículas.")]
    public AudioClip particleSound;
    [Range(0f, 1f)] public float soundVolume = 1f;
    [Tooltip("Si está activado, el sonido seguirá al objeto de partículas.")]
    public bool attachSoundToParticles = false;

    private int currentPresses = 0;
    private bool isMinigameActive = false;
    private bool turned = false;
    private PlayerController pc;

    private void OnEnable()
    {
        currentPresses = 0;
    }

    private void Start()
    {
        if (playerInteraction != null)
            pc = playerInteraction.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!isMinigameActive || pc == null) return;

        if (pc.GetX() > 0 && !turned)
            HandleTurn(true);
        else if (pc.GetX() < 0 && turned)
            HandleTurn(false);
    }

    private void HandleTurn(bool _turned)
    {
        currentPresses++;
        turned = _turned;

        if (currentPresses >= requiredPresses)
        {
            AwardPoints();
            CloseUI();
        }
    }

    private void AwardPoints()
    {
        if (playerInteraction != null && playerInteraction.name != null)
        {
            string playerName = playerInteraction.gameObject.name;
            ScoreManager.Instance.AddScore(playerName, 100);
        }
    }

    public void StartMinigame()
    {
        currentPresses = 0;
        isMinigameActive = true;
    }

    public void StopMinigame()
    {
        isMinigameActive = false;
    }

    private void CloseUI()
    {
        if (playerInteraction != null && playerInteraction.playerController != null)
            playerInteraction.playerController.enabled = true;

        if (playerInteraction != null && playerInteraction.CurrentTarget != null)
        {
            Destroy(playerInteraction.CurrentTarget.gameObject);
            playerInteraction.SetCurrentTarget(null);
        }

        GameObject p = Instantiate(particles, transform.position, transform.rotation);

        if (particleSound != null)
        {
            if (attachSoundToParticles)
            {
                AudioSource src = p.AddComponent<AudioSource>();
                src.clip = particleSound;
                src.volume = soundVolume;
                src.spatialBlend = 0f;
                src.Play();
            }
            else
            {
                AudioSource.PlayClipAtPoint(particleSound, transform.position, soundVolume);
            }
        }
        Destroy(gameObject);
    }
}





