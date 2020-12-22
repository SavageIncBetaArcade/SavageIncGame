using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InfoPopupHandler : MonoBehaviour
{
    protected virtual Item Item => null;
    public GameObject itemInfoPrefab;
    private Popup popup;
    private GameObject canvas;
    private Rect canvasRect;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("UICanvas")?.GetComponent<Canvas>().gameObject;
    }

    private void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>().rect;
    }

    protected virtual void Update()
    {
        if(Item && !popup && EventSystem.current.currentSelectedGameObject == gameObject) 
            ShowItemInfo();
        else if(popup && EventSystem.current.currentSelectedGameObject != gameObject) 
            Destroy(popup.gameObject);
    }

    // public void OnPointerEnter(PointerEventData data)
    // {
    //     if(Item) ShowItemInfo();
    // }
    //
    // public void OnPointerExit(PointerEventData data)
    // {
    //     if(popup) Destroy(popup.gameObject);
    // }

    void OnDisable()
    {
        if (popup) Destroy(popup.gameObject);
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
        var positionInRelationToCanvas = transform.position - canvas.transform.position;
        if (positionInRelationToCanvas.x > 0) return transform.position.x - 400;
        return transform.position.x;
        // return CanvasContainsPopupWidth(popupWidth)
        //     ? transform.position.x
        //     : transform.position.x - popupWidth - slotWidth / 1.85f;
    }
    
    private bool CanvasContainsPopupWidth(float popupWidth)
    {
        return canvasRect.Contains(new Vector2(transform.position.x + popupWidth, transform.position.y));
    }
}