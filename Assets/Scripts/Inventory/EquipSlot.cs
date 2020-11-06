using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    public InventorySlot equippedSlot;

    public void EquipItem(InventoryItem inventoryItem)
    {
        if (equippedSlot.InventoryItem != null) UnequipItem();
        equippedSlot.Image.sprite = inventoryItem.Item.Sprite;
        equippedSlot.Image.enabled = true;
        equippedSlot.InventoryItem = inventoryItem;
    }

    private void RemoveItem()
    {
        equippedSlot.Image.sprite = null;
        equippedSlot.Image.enabled = false;
        equippedSlot.InventoryItem = null;
    }

    public void UnequipItem()
    {
        if (equippedSlot.InventoryItem == null || equippedSlot.InventoryItem.Item == null) return;
        var inventory = FindObjectOfType<Inventory>();
        inventory.AddItem(equippedSlot.InventoryItem.Item);
        RemoveItem();
    }
}
