using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuButtonMouseHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        background.sprite = highlightedSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.sprite = normalSprite;
    }
}
