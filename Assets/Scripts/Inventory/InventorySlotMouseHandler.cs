using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory inventory;
    public int position;
    private bool showInfo;
    private Item item => inventory.getItemAt(position);
    public Text itemName;
    public Text itemQuote;
    public Text itemDescription;
    public GameObject itemInfo;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                inventory.LeftClickItem(position);
                break;
            case PointerEventData.InputButton.Right:
                inventory.RightClickItem(position);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData data) { showInfo = true; }
    
    public void OnPointerExit(PointerEventData data) { showInfo = false; }

    private void OnGUI()
    {
        if (showInfo && inventory.hasItemAt(position))
            ShowItemInfo();
        else
            HideItemInfo();
    }

    private void ShowItemInfo()
    {
        itemName.text = item.Name;
        itemQuote.text = item.Quote;
        itemDescription.text = item.GetInfoDescription();
        itemInfo.SetActive(true);
    }
    
    private void HideItemInfo()
    {
        itemInfo.SetActive(false);
    }
}