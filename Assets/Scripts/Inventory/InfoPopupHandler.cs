using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InfoPopupHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected virtual Item Item => null;
    public GameObject itemInfoPrefab;
    private Popup popup;
    private Canvas canvas;
    private Rect canvasRect;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>().rect;
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
        popup.transform.position = new Vector3(PopupXPosition(), transform.position.y, 0);
        popup.transform.SetParent(canvas.transform);
    }

    private float PopupXPosition()
    {
        var slotWidth = GetComponent<RectTransform>().rect.width;
        var popupWidth = popup.GetComponent<RectTransform>().rect.width;
        return CanvasContainsPopupWidth(popupWidth)
            ? transform.position.x
            : transform.position.x - popupWidth - slotWidth / 1.85f;
    }
    
    private bool CanvasContainsPopupWidth(float popupWidth)
    {
        return canvasRect.Contains(new Vector2(transform.position.x + popupWidth, transform.position.y));
    }
}