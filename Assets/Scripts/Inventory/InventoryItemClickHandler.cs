using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemClickHandler : MonoBehaviour, IPointerDownHandler
{
    public int inventorySlot;

    public void OnPointerDown(PointerEventData e)
    {
        var inventory = FindObjectOfType<Inventory>();
        var itemClicked = inventory.items[inventorySlot];
        if (itemClicked.InventoryItem == null) return;
        inventory.RemoveItemAt(inventorySlot);
        itemClicked.InventoryItem.Click(inventory);
    }
}
