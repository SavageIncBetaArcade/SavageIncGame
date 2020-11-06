using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InfoPopupHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected virtual Item Item => null;
    public GameObject itemInfoPrefab;
    private Popup popup;
    private Canvas canvas;
    
    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        if(Item) ShowItemInfo();
    }

    public void OnPointerExit(PointerEventData data)
    {
        if(popup) Destroy(popup.gameObject);
    }
    
    private void ShowItemInfo()
    {
        popup = Instantiate(itemInfoPrefab).GetComponent<Popup>();
        popup.title.text = Item.Name;
        popup.quote.text = Item.Quote;
        popup.description.text = Item.GetInfoDescription();
        popup.transform.position = new Vector3(transform.position.x + GetPopupXOffset(), transform.position.y, 0);
        popup.transform.SetParent(canvas.transform);
    }

    private float GetPopupXOffset()
    {
        var slotWidth = GetComponent<RectTransform>().rect.width;
        var popupWidth = popup.GetComponent<RectTransform>().rect.width - slotWidth * 0.2f;
        if (CanvasContainsPopup(popupWidth))
            return popupWidth;
        return -popupWidth;
    }

    private bool CanvasContainsPopup(float popupWidth)
    {
        return canvas.GetComponent<RectTransform>().rect.Contains(new Vector2(transform.position.x + popupWidth, transform.position.y));
    }
}