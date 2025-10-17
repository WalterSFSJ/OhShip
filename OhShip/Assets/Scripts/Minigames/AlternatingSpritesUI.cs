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

    private bool isRunning = true;
    private List<Image> activeImages = new List<Image>();
    private Dictionary<Image, Sprite[]> spritePairs = new Dictionary<Image, Sprite[]>();

    private void Start()
    {
        // Registrar imágenes activas y sus sprites si existen
        AddImage(imageA, spriteA1, spriteA2);
        AddImage(imageB, spriteB1, spriteB2);
        AddImage(imageC, spriteC1, spriteC2);
        AddImage(imageD, spriteD1, spriteD2);

        if (activeImages.Count == 0)
        {
            Debug.LogWarning("[AlternatingSpritesUI] No hay imágenes asignadas. El script no hará nada.");
            return;
        }

        // Inicializar todas en su primer sprite
        foreach (var img in activeImages)
            img.sprite = spritePairs[img][0];

        StartCoroutine(AlternateSpritesSequentially());
    }

    private void AddImage(Image img, Sprite s1, Sprite s2)
    {
        if (img != null)
        {
            activeImages.Add(img);
            spritePairs[img] = new Sprite[] { s1, s2 };
        }
    }

    private IEnumerator AlternateSpritesSequentially()
    {
        int index = 0;
        int previousIndex = -1;

        while (isRunning)
        {
            // Restaurar la imagen anterior a su sprite base
            if (previousIndex >= 0)
            {
                Image previousImage = activeImages[previousIndex];
                previousImage.sprite = spritePairs[previousImage][0];
            }

            // Cambiar la imagen actual a su sprite alternativo
            Image currentImage = activeImages[index];
            currentImage.sprite = spritePairs[currentImage][1];

            // Guardar índice anterior
            previousIndex = index;

            // Esperar antes de pasar a la siguiente imagen
            yield return new WaitForSeconds(switchInterval);

            // Avanzar
            index = (index + 1) % activeImages.Count;
        }
    }

    public void StopAlternation()
    {
        isRunning = false;
    }

    public void ResumeAlternation()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(AlternateSpritesSequentially());
        }
    }
}





