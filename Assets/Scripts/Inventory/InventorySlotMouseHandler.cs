using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventorySectionHandler inventorySectionHandler;
    private Inventory Inventory => inventorySectionHandler.currentInventory;
    public int position;
    private Item Item => Inventory.getItemAt(position);
    public GameObject itemInfo;
    private Popup popup;
    private Canvas canvas;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Inventory.LeftClickItem(position);
                break;
            case PointerEventData.InputButton.Right:
                Inventory.RightClickItem(position);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(Inventory.hasItemAt(position)) ShowItemInfo();
    }

    public void OnPointerExit(PointerEventData data)
    {
        if(popup) Destroy(popup.gameObject);
    }
    
    private void ShowItemInfo()
    {
        popup = Instantiate(itemInfo).GetComponent<Popup>();
        popup.title.text = Item.Name;
        popup.quote.text = Item.Quote;
        popup.description.text = Item.GetInfoDescription();
        popup.transform.position = new Vector3(transform.position.x + GetPopupXOffset(), transform.position.y, 0);
        popup.transform.parent = canvas.transform;
    }

    private float GetPopupXOffset()
    {
        var popupWidth = GetComponent<RectTransform>().rect.width;
        var corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);
        if (corners[2].x + popupWidth > canvas.GetComponent<RectTransform>().rect.width)
            return -popupWidth;
        return popupWidth * 0.5f;
    }
}