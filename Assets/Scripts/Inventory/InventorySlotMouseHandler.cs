using UnityEngine.EventSystems;

public class InventorySlotMouseHandler : InfoPopupHandler, IPointerClickHandler
{
    public InventorySectionHandler inventorySectionHandler;
    private Inventory Inventory => inventorySectionHandler.currentInventory;
    public int position;
    protected override Item Item => Inventory.getItemAt(position);

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
}