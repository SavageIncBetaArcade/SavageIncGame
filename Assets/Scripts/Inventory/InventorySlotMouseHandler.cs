using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventorySectionHandler inventorySectionHandler;
    private Inventory Inventory => inventorySectionHandler.currentInventory;
    public int position;
    private bool showInfo;
    private Item Item => Inventory.getItemAt(position);
    public Text itemName;
    public Text itemQuote;
    public Text itemDescription;
    public GameObject itemInfo;

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

    public void OnPointerEnter(PointerEventData data) { showInfo = true; }
    
    public void OnPointerExit(PointerEventData data) { showInfo = false; }

    private void OnGUI()
    {
        if (showInfo && Inventory.hasItemAt(position))
            ShowItemInfo();
        else
            HideItemInfo();
    }

    private void ShowItemInfo()
    {
        itemName.text = Item.Name;
        itemQuote.text = Item.Quote;
        itemDescription.text = Item.GetInfoDescription();
        itemInfo.SetActive(true);
    }
    
    private void HideItemInfo()
    {
        itemInfo.SetActive(false);
    }
}