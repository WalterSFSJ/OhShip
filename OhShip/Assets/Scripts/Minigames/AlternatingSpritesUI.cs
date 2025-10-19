using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlternatingSpritesUI : MonoBehaviour
{
    [Header("Referencias a las imágenes (pueden dejarse nulas si no se usan)")]
    public Image imageA;
    public Image imageB;
    public Image imageC;
    public Image imageD;

    [Header("Sprites para alternar (solo se usan si la imagen existe)")]
    public Sprite spriteA1;
    public Sprite spriteA2;
    public Sprite spriteB1;
    public Sprite spriteB2;
    public Sprite spriteC1;
    public Sprite spriteC2;
    public Sprite spriteD1;
    public Sprite spriteD2;

    [Header("Configuración de tiempo")]
    [Tooltip("Tiempo entre cada cambio de sprite (en segundos)")]
    public float switchInterval = 0.5f;

    [Header("Sonido")]
    [Tooltip("Clip de audio que se reproducirá en loop mientras la UI esté activa")]
    public AudioClip loopSound;
    private AudioSource audioSource;

    private bool isRunning = false;
    private List<Image> activeImages = new List<Image>();
    private Dictionary<Image, Sprite[]> spritePairs = new Dictionary<Image, Sprite[]>();
    private Coroutine alternationCoroutine;

    private int index = 0;
    private int previousIndex = -1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        AddImage(imageA, spriteA1, spriteA2);
        AddImage(imageB, spriteB1, spriteB2);
        AddImage(imageC, spriteC1, spriteC2);
        AddImage(imageD, spriteD1, spriteD2);

        if (activeImages.Count == 0)
        {
            return;
        }

        foreach (var img in activeImages)
            img.sprite = spritePairs[img][0];

        StartAlternation();
    }

    private void AddImage(Image img, Sprite s1, Sprite s2)
    {
        if (img != null && s1 != null && s2 != null)
        {
            activeImages.Add(img);
            spritePairs[img] = new Sprite[] { s1, s2 };
        }
    }

    private IEnumerator AlternateSpritesSequentially()
    {
        while (true)
        {
            if (!isRunning)
            {
                yield return null;
                continue;
            }

            if (activeImages.Count == 0)
            {
                yield return null;
                continue;
            }

            if (previousIndex >= 0 && previousIndex < activeImages.Count)
            {
                var prevImg = activeImages[previousIndex];
                if (prevImg != null && prevImg.gameObject.activeInHierarchy)
                    prevImg.sprite = spritePairs[prevImg][0];
            }

            if (index >= activeImages.Count)
                index = 0;

            var currentImage = activeImages[index];

            if (currentImage != null && currentImage.gameObject.activeInHierarchy)
            {
                var pair = spritePairs[currentImage];
                if (pair != null && pair.Length > 1 && pair[1] != null)
                    currentImage.sprite = pair[1];
            }

            previousIndex = index;
            index = (index + 1) % activeImages.Count;

            yield return new WaitForSeconds(switchInterval);
        }
    }

    private void StartAlternation()
    {
        if (alternationCoroutine == null)
            alternationCoroutine = StartCoroutine(AlternateSpritesSequentially());

        isRunning = true;
    }

    public void StopAlternation()
    {
        isRunning = false;

        foreach (var img in activeImages)
        {
            if (img != null)
                img.sprite = spritePairs[img][0];
        }
    }

    public void ResumeAlternation()
    {
        isRunning = true;
    }

    public void RestartAlternation()
    {
        index = 0;
        previousIndex = -1;
        isRunning = true;
    }

    private void OnEnable()
    {
        if (activeImages.Count > 0)
        {
            index = 0;
            previousIndex = -1;
            isRunning = true;

            if (alternationCoroutine == null)
                alternationCoroutine = StartCoroutine(AlternateSpritesSequentially());
        }

        if (loopSound != null && audioSource != null)
        {
            audioSource.clip = loopSound;
            audioSource.Play();
        }
    }

    private void OnDisable()
    {
        isRunning = false;

        if (alternationCoroutine != null)
        {
            StopCoroutine(alternationCoroutine);
            alternationCoroutine = null;
        }

        foreach (var img in activeImages)
        {
            if (img != null)
                img.sprite = spritePairs[img][0];
        }

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }
}












