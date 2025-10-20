using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image buttonImage;       
    public Sprite normalSprite;     
    public Sprite hoverSprite;      

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = normalSprite;
    }
}

