using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    private InventorySlot equippedSlot;

    public void EquipItem(InventorySlot slotToEquip)
    {
        equippedSlot = slotToEquip;
    }

    public void RemoveItem()
    {
        equippedSlot.Image = null;
        equippedSlot.InventoryItem = null;
    }

    public void Click()
    {
        var inventory = GetComponent<Inventory>();
        inventory.AddItem(equippedSlot.InventoryItem.Item);
        RemoveItem();
    }
}
