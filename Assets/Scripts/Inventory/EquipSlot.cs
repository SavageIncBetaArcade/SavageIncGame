using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    public InventorySlot equippedSlot;

    public void EquipItem(Sprite sprite, InventoryItem inventoryItem)
    {
        equippedSlot.Image.sprite = sprite;
        equippedSlot.Image.enabled = true;
        equippedSlot.InventoryItem = inventoryItem;
    }

    public void RemoveItem()
    {
        equippedSlot.Image.sprite = null;
        equippedSlot.Image.enabled = false;
        equippedSlot.InventoryItem = null;
    }

    public void Click()
    {
        var inventory = FindObjectOfType<Inventory>();
        inventory.AddItem(equippedSlot.InventoryItem.Item);
        RemoveItem();
    }
}
