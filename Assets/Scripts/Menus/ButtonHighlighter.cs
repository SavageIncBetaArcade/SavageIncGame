using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour
{
    public Sprite normalImage;
    public Sprite highlightImage;
    public Image buttonImage;
    
    private void Update()
    {
        ChangeButtonImage(EventSystem.current.currentSelectedGameObject == gameObject ? highlightImage : normalImage);
    }

    private void ChangeButtonImage(Sprite image)
    {
        buttonImage.sprite = image;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeButtonImage(highlightImage);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeButtonImage(normalImage);
    }
}